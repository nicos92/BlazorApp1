using BlazorApp1.Application.Interfaces;
using BlazorApp1.Models;

namespace BlazorApp1.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public AuthService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<LoginResult> LoginAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return new LoginResult
            {
                IsSuccess = false,
                ErrorCode = "empty_fields",
                ErrorMessage = "Por favor, complete todos los campos."
            };
        }

        var usuario = await _usuarioRepository.GetByUsernameAsync(username);

        if (usuario == null)
        {
            return new LoginResult
            {
                IsSuccess = false,
                ErrorCode = "invalid_credentials",
                ErrorMessage = "Credenciales incorrectas. Por favor, verifique su nombre de usuario y contraseña."
            };
        }

        if (!usuario.Activo)
        {
            return new LoginResult
            {
                IsSuccess = false,
                ErrorCode = "inactive_user",
                ErrorMessage = "El usuario está inactivo. Por favor, contacte con el administrador."
            };
        }

        if (!BCrypt.Net.BCrypt.Verify(password, usuario?.Password ?? ""))
        {
            return new LoginResult
            {
                IsSuccess = false,
                ErrorCode = "invalid_credentials",
                ErrorMessage = "Credenciales incorrectas. Por favor, verifique su nombre de usuario y contraseña."
            };
        }

        return new LoginResult
        {
            IsSuccess = true,
            User = new UsuarioDto
            {
                Id = usuario.Id,
                Username = usuario.Username,
                Email = usuario.Email,
                FirstName = usuario.FirstName,
                LastName = usuario.LastName,
                Legajo = usuario.Legajo,
                Department = usuario.Department,
                IdRol = usuario.IdRol,
                NombreRol = usuario.NombreRol,
                Activo = usuario.Activo
            }
        };
    }

    public Task LogoutAsync()
    {
        return Task.CompletedTask;
    }
}
