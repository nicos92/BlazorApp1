using System.Security.Claims;
using BlazorApp1.Application.Interfaces;
using BlazorApp1.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp1.Services;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IAuthService _authService;
    private ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(IAuthService authService)
    {
        _authService = authService;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = GetStoredUser();
        var claimsPrincipal = user != null ? CreateClaimsPrincipal(user) : _anonymous;
        return Task.FromResult(new AuthenticationState(claimsPrincipal));
    }

    public void SetAuthenticationState(UsuarioDto? user)
    {
        if (user != null)
        {
            StoreUser(user);
            var claimsPrincipal = CreateClaimsPrincipal(user);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
        else
        {
            ClearStoredUser();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }

    public async Task<LoginResult> LoginAsync(string username, string password)
    {
        var result = await _authService.LoginAsync(username, password);
        
        if (result.IsSuccess && result.User != null)
        {
            StoreUser(result.User);
            var claimsPrincipal = CreateClaimsPrincipal(result.User);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
        
        return result;
    }

    public void Logout()
    {
        ClearStoredUser();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }

    private static ClaimsPrincipal CreateClaimsPrincipal(UsuarioDto user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.GivenName, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),
            new("legajo", user.Legajo.ToString()),
            new(ClaimTypes.Role, user.NombreRol ?? "usuario")
        };

        var identity = new ClaimsIdentity(claims, "CustomAuthentication");
        return new ClaimsPrincipal(identity);
    }

    private static UsuarioDto? GetStoredUser()
    {
        return null;
    }

    private static void StoreUser(UsuarioDto user)
    {
    }

    private static void ClearStoredUser()
    {
    }
}
