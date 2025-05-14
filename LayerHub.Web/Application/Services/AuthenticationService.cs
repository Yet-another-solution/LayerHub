using System.Net.Http.Headers;
using Blazored.LocalStorage;
using LayerHub.Shared.Dto;
using LayerHub.Web.Application.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace LayerHub.Web.Application.Services;

public interface IAuthenticationService
{
    Task Login(string username, string password, CancellationToken token);
    Task Logout();
}

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly CustomAuthStateProvider _authStateProvider;
        
    public AuthenticationService(
        HttpClient httpClient, 
        AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _authStateProvider = (CustomAuthStateProvider)authStateProvider;
    }
        
    public async Task Login(string username, string password, CancellationToken cancellationToken)
    {
        // Your existing login logic
        // Example request structure (adjust based on your actual API):
        var loginRequest = new LoginRequest()
        {
            Email = username,
            Password = password
        };
            
        var response = await _httpClient.PostAsJsonAsync("/Identity/Auth/login", loginRequest, cancellationToken);
        response.EnsureSuccessStatusCode();
            
        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>(cancellationToken: cancellationToken);
            
        if (authResponse != null)
        {
            // Persist authentication state
            await _authStateProvider.MarkUserAsAuthenticated(authResponse);
                
            // Set bearer token for future API requests
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", authResponse.Token);
        }
    }

    public async Task Logout()
    {
        // Optional: Call logout API endpoint if needed
            
        // Clear authentication state
        await _authStateProvider.MarkUserAsLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
        
    // Add other methods as required by IAuthenticationService
}

