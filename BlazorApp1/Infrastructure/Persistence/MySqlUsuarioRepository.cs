using BlazorApp1.Application.Interfaces;
using BlazorApp1.Configuration;
using BlazorApp1.Models;
using Dapper;
using MySqlConnector;

namespace BlazorApp1.Infrastructure.Persistence;

public class MySqlUsuarioRepository : IUsuarioRepository
{
    private readonly string _connectionString;

    public MySqlUsuarioRepository()
    {
        _connectionString = Settings.STRCONNECTION;
    }

    private MySqlConnection CreateConnection() => new(_connectionString);

    public async Task<Usuario?> GetByUsernameAsync(string username)
    {
        using var connection = CreateConnection();
        const string sql = @"
            SELECT 
                u.id_usuario as Id,
                u.username as Username,
                u.email as Email,
                u.password as Password,
                u.first_name as FirstName,
                u.last_name as LastName,
                u.legajo as Legajo,
                u.department as Department,
                u.id_rol as IdRol,
                r.nombre_rol as NombreRol,
                u.activo as Activo,
                u.created_at as CreatedAt,
                u.updated_at as UpdatedAt
            FROM usuarios u
            LEFT JOIN roles r ON u.id_rol = r.id_rol
            WHERE u.username = @Username";

        return await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Username = username });
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        using var connection = CreateConnection();
        const string sql = @"
            SELECT 
                u.id_usuario as Id,
                u.username as Username,
                u.email as Email,
                u.password as Password,
                u.first_name as FirstName,
                u.last_name as LastName,
                u.legajo as Legajo,
                u.department as Department,
                u.id_rol as IdRol,
                r.nombre_rol as NombreRol,
                u.activo as Activo,
                u.created_at as CreatedAt,
                u.updated_at as UpdatedAt
            FROM usuarios u
            LEFT JOIN roles r ON u.id_rol = r.id_rol
            ORDER BY u.username";

        return await connection.QueryAsync<Usuario>(sql);
    }

    public async Task<int> GetCountAsync()
    {
        using var connection = CreateConnection();
        const string sql = "SELECT COUNT(*) FROM usuarios";
        return await connection.ExecuteScalarAsync<int>(sql);
    }
}
