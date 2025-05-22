using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using Blazored.Toast.Services;
using LayerHub.Shared.Dto;
using LayerHub.Shared.Dto.Responses;
using Microsoft.AspNetCore.Components;

namespace LayerHub.Web.Application.Services;

public interface IHttpService
{
    Task<T?> Get<T>(string uri, CancellationToken token = default);
    Task<T?> Post<T>(string uri, object? body = null, CancellationToken token = default);
    Task Post(string uri, object? body = null, CancellationToken token = default);
    Task PostFile(string uri, FileStream stream, string fileName, CancellationToken token = default);
    Task<T?> Put<T>(string uri, object? body = null, CancellationToken token = default);
    Task Put(string uri, object? body = null, CancellationToken token = default);
    Task<T?> Delete<T>(string uri, object? body = null, CancellationToken token = default);
    Task Delete(string uri, object? body = null, CancellationToken token = default);
}

public class HttpService : IHttpService
{
    private HttpClient _httpClient;
    private NavigationManager _navigationManager;
    private ILocalStorageService _localStorageService;
    private IConfiguration _configuration;
    private IToastService _toastService;

    public HttpService(
        IHttpClientFactory httpClientFactory,
        NavigationManager navigationManager,
        ILocalStorageService localStorageService,
        IConfiguration configuration, IToastService toastService)
    {
        _httpClient = httpClientFactory.CreateClient("API");
        _navigationManager = navigationManager;
        _localStorageService = localStorageService;
        _configuration = configuration;
        _toastService = toastService;
    }

    public async Task<T?> Get<T>(string uri, CancellationToken token = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        return await SendRequest<T>(request, token);
    }

    public async Task<T?> Post<T>(string uri, object? body = null, CancellationToken token = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, uri);
        if (body != null)
            request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        return await SendRequest<T>(request, token);
    }
    public async Task Post(string uri, object? body = null, CancellationToken token = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, uri);
        if (body != null)
            request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        await SendRequest(request, token);
    }

    public async Task PostFile(string uri, FileStream stream, string fileName, CancellationToken token = default)
    {
        byte[] fileBytes;
        using var ms = new MemoryStream();

        stream.Position = 0; // Ensure we're reading from the start of the file stream
        await stream.CopyToAsync(ms, token);
        fileBytes = ms.ToArray();

        using var content = new MultipartFormDataContent();
        var byteContent = new ByteArrayContent(fileBytes);
        content.Add(byteContent, "tenantData", fileName);

        var request = new HttpRequestMessage(HttpMethod.Post, uri) { Content = content };
        await SendRequest(request, token);
    }

    public async Task<T?> Put<T>(string uri, object? body = null, CancellationToken token = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, uri);
        if (body != null)
            request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        return await SendRequest<T>(request, token);
    }
    public async Task Put(string uri, object? body = null, CancellationToken token = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, uri);
        if (body != null)
            request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        await SendRequest(request, token);
    }

    public async Task<T?> Delete<T>(string uri, object? body = null, CancellationToken token = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, uri);
        if (body != null)
            request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        return await SendRequest<T>(request, token);
    }
    public async Task Delete(string uri, object? body = null, CancellationToken token = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, uri);
        if (body != null)
            request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        await SendRequest(request, token);
    }

    // helper methods

    private async Task<T?> SendRequest<T>(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        // add jwt auth header if user is logged in and request is to the api url
        var token = await _localStorageService.GetItemAsync<AuthResponse>("user");
        var isApiUrl = !request.RequestUri?.IsAbsoluteUri;
        if (token != null && isApiUrl != null && isApiUrl.Value)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        // auto logout on 401 response
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _navigationManager.NavigateTo("logout");
            return default!;
        }

        // 403 response
        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            throw new Exception("You do not have permission to access this resource.");
        }

        // 404 response
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new Exception("Resource not found!");
        }

        // throw exception on error response
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
            throw new Exception(error?.Error);
        }

        if (typeof(T) == typeof(byte[]))
        {
            return (T)(object)await response.Content.ReadAsByteArrayAsync();
        }
        return await response.Content.ReadFromJsonAsync<T>();
    }
    private async Task SendRequest(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        // add jwt auth header if user is logged in and request is to the api url
        var token = await _localStorageService.GetItemAsync<AuthResponse>("user");
        var isApiUrl = !request.RequestUri?.IsAbsoluteUri;
        if (token != null && isApiUrl != null && isApiUrl.Value)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized && await TryRefreshToken(token))
        {
            // Retry the original request after refreshing the token
            token = await _localStorageService.GetItemAsync<AuthResponse>("user");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
            await SendRequest(request, cancellationToken);
        }

        // auto logout on 401 response
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _navigationManager.NavigateTo("logout");
        }

        // throw exception on error response
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            throw new Exception(error?["message"]);
        }
    }

    private async Task<bool> TryRefreshToken(AuthResponse token)
    {
        var refreshTokenRequest = new TokenRequest
        {
            Token = token.Token,
            RefreshToken = token.RefreshToken
        };

        var refreshTokenContent = new StringContent(
            JsonSerializer.Serialize(refreshTokenRequest),
            Encoding.UTF8,
            "application/json");

        using var refreshTokenResponse = await _httpClient.PostAsync("/Admin/Identity/AdminAuth/RefreshToken", refreshTokenContent);

        if (refreshTokenResponse.IsSuccessStatusCode)
        {
            var newToken = await refreshTokenResponse.Content.ReadFromJsonAsync<AuthResponse>();
            await _localStorageService.SetItemAsync("user", newToken);
            return true;
        }

        return false;
    }
}
