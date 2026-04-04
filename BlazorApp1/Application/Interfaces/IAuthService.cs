using BlazorApp1.Models;

namespace BlazorApp1.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResult> LoginAsync(string username, string password);
    Task LogoutAsync();
}
