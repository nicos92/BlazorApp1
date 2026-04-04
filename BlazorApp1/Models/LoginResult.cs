namespace BlazorApp1.Models;

public class LoginResult
{
    public bool IsSuccess { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public UsuarioDto? User { get; set; }
}

public class UsuarioDto
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public int Legajo { get; set; }
    public string? Department { get; set; }
    public int IdRol { get; set; }
    public string? NombreRol { get; set; }
    public bool Activo { get; set; }
}

public class Usuario
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public int Legajo { get; set; }
    public string? Department { get; set; }
    public int IdRol { get; set; }
    public string? NombreRol { get; set; }
    public bool Activo { get; set; }
}
