namespace BlazorApp1.Entities;

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
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
