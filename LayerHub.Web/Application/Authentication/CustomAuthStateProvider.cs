using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using LayerHub.Shared.Dto;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace LayerHub.Web.Application.Authentication;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;

    public CustomAuthStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var sessionData = await _localStorage.GetItemAsync<AuthResponse>("user");

            if (sessionData is null)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var identity = GetClaimsIdentity(sessionData);
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }
        catch (Exception)
        {
            await MarkUserAsLoggedOut();
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public async Task MarkUserAsAuthenticated(AuthResponse authResponse)
    {
        await _localStorage.SetItemAsync("user", authResponse);
        var identity = GetClaimsIdentity(authResponse);
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    private ClaimsIdentity GetClaimsIdentity(AuthResponse authResponse)
    {
        // Extract claims from JWT token
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(authResponse.Token);
        var claims = jwtToken.Claims.ToList();

        // Add additional claims from the AuthResponse if needed
        claims.Add(new Claim(ClaimTypes.NameIdentifier, authResponse.UserId.ToString()));
        claims.Add(new Claim(ClaimTypes.Email, authResponse.Email));

        // Add role claims
        foreach (var role in authResponse.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return new ClaimsIdentity(claims, "jwt");
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _localStorage.RemoveItemAsync("user");
        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    // Add this method to your CustomAuthStateProvider class
    public async Task<AuthResponse> GetAuthDataAsync()
    {
        try
        {
            var sessionData = await _localStorage.GetItemAsync<AuthResponse>("user");
            return sessionData;
        }
        catch
        {
            return null;
        }
    }
}