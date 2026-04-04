using BlazorApp1.Application.Interfaces;
using BlazorApp1.Configuration;
using BlazorApp1.Entities;
using Dapper;
using MySqlConnector;

namespace BlazorApp1.Infrastructure.Persistence;

public class MySqlTarimaRepository : ITarimaRepository
{
    private readonly string _connectionString;

    public MySqlTarimaRepository()
    {
        _connectionString = Settings.STRCONNECTION;
    }

    private MySqlConnection CreateConnection() => new(_connectionString);

    public async Task<IEnumerable<Tarima>> GetAllAsync()
    {
        using var connection = CreateConnection();
        const string sql = @"
            SELECT 
                t.id_tarima as Id,
                t.codigo_barras as CodigoBarras,
                t.numero_producto as NumeroProducto,
                t.numero_tarima as NumeroTarima,
                t.numero_usuario as NumeroUsuario,
                t.cantidad_cajas as CantidadCajas,
                t.peso as Peso,
                t.numero_venta as NumeroVenta,
                t.descripcion as Descripcion,
                t.fecha_registro as FechaCreacion
            FROM tarimas t
            ORDER BY t.fecha_registro DESC
            LIMIT 1000";
        
        return await connection.QueryAsync<Tarima>(sql);
    }

    public async Task<IEnumerable<Tarima>> GetByDateAsync(DateTime fecha)
    {
        using var connection = CreateConnection();
        const string sql = @"
            SELECT 
                t.id_tarima as Id,
                t.codigo_barras as CodigoBarras,
                t.numero_producto as NumeroProducto,
                t.numero_tarima as NumeroTarima,
                t.numero_usuario as NumeroUsuario,
                t.cantidad_cajas as CantidadCajas,
                t.peso as Peso,
                t.numero_venta as NumeroVenta,
                t.descripcion as Descripcion,
                t.fecha_registro as FechaCreacion
            FROM tarimas t
            WHERE DATE(t.fecha_registro) = DATE(@Fecha)
            ORDER BY t.fecha_registro DESC";
        
        return await connection.QueryAsync<Tarima>(sql, new { Fecha = fecha });
    }

    public async Task<IEnumerable<Tarima>> FilterTarimasAsync(
        string? numeroProducto = null,
        string? numeroTarima = null,
        string? numeroUsuario = null,
        string? numeroVenta = null,
        DateTime? fechaRegistro = null,
        string? legajo = null,
        string? nombreUsuario = null,
        int? cantidadCajasMin = null,
        decimal? pesoMin = null)
    {
        using var connection = CreateConnection();
        
        var sql = @"
            SELECT 
                t.id_tarima as Id,
                t.codigo_barras as CodigoBarras,
                t.numero_producto as NumeroProducto,
                t.numero_tarima as NumeroTarima,
                t.numero_usuario as NumeroUsuario,
                t.cantidad_cajas as CantidadCajas,
                t.peso as Peso,
                t.numero_venta as NumeroVenta,
                t.descripcion as Descripcion,
                t.fecha_registro as FechaCreacion
            FROM tarimas t
            LEFT JOIN usuarios u ON t.id_usuario = u.id_usuario
            WHERE 
                (@numeroProducto IS NULL OR @numeroProducto = '' OR t.numero_producto LIKE CONCAT('%', @numeroProducto, '%'))
                AND (@numeroTarima IS NULL OR @numeroTarima = '' OR t.numero_tarima LIKE CONCAT('%', @numeroTarima, '%'))
                AND (@numeroUsuario IS NULL OR @numeroUsuario = '' OR t.numero_usuario LIKE CONCAT('%', @numeroUsuario, '%'))
                AND (@numeroVenta IS NULL OR @numeroVenta = '' OR t.numero_venta LIKE CONCAT('%', @numeroVenta, '%'))
                AND (@fechaRegistro IS NULL OR DATE(t.fecha_registro) = @fechaRegistro)
                AND (@legajo IS NULL OR @legajo = '' OR u.legajo LIKE CONCAT('%', @legajo, '%'))
                AND (@nombreUsuario IS NULL OR @nombreUsuario = '' OR CONCAT(u.first_name, ' ', u.last_name) LIKE CONCAT('%', @nombreUsuario, '%'))
                AND (@cantidadCajasMin IS NULL OR t.cantidad_cajas >= @cantidadCajasMin)
                AND (@pesoMin IS NULL OR t.peso >= @pesoMin)
            ORDER BY t.fecha_registro DESC
            LIMIT 1000";

        return await connection.QueryAsync<Tarima>(sql, new
        {
            numeroProducto = string.IsNullOrEmpty(numeroProducto) ? null : numeroProducto,
            numeroTarima = string.IsNullOrEmpty(numeroTarima) ? null : numeroTarima,
            numeroUsuario = string.IsNullOrEmpty(numeroUsuario) ? null : numeroUsuario,
            numeroVenta = string.IsNullOrEmpty(numeroVenta) ? null : numeroVenta,
            fechaRegistro,
            legajo = string.IsNullOrEmpty(legajo) ? null : legajo,
            nombreUsuario = string.IsNullOrEmpty(nombreUsuario) ? null : nombreUsuario,
            cantidadCajasMin,
            pesoMin
        });
    }

    public async Task<Tarima?> GetByIdAsync(int id)
    {
        using var connection = CreateConnection();
        const string sql = @"
            SELECT 
                id_tarima as Id,
                codigo_barras as CodigoBarras,
                numero_producto as NumeroProducto,
                numero_tarima as NumeroTarima,
                numero_usuario as NumeroUsuario,
                cantidad_cajas as CantidadCajas,
                peso as Peso,
                numero_venta as NumeroVenta,
                descripcion as Descripcion,
                fecha_registro as FechaCreacion
            FROM tarimas 
            WHERE id_tarima = @Id";
        
        return await connection.QueryFirstOrDefaultAsync<Tarima>(sql, new { Id = id });
    }

    public async Task<Tarima?> GetByCodigoBarrasAndFechaAsync(string codigoBarras, DateTime fecha)
    {
        using var connection = CreateConnection();
        const string sql = @"
            SELECT 
                id_tarima as Id,
                codigo_barras as CodigoBarras,
                numero_producto as NumeroProducto,
                numero_tarima as NumeroTarima,
                numero_usuario as NumeroUsuario,
                cantidad_cajas as CantidadCajas,
                peso as Peso,
                numero_venta as NumeroVenta,
                descripcion as Descripcion,
                fecha_registro as FechaCreacion
            FROM tarimas 
            WHERE codigo_barras = @CodigoBarras AND DATE(fecha_registro) = DATE(@Fecha)";
        
        return await connection.QueryFirstOrDefaultAsync<Tarima>(sql, new { CodigoBarras = codigoBarras, Fecha = fecha });
    }

    public async Task<Tarima> CreateAsync(Tarima tarima)
    {
        using var connection = CreateConnection();
        const string sql = @"
            INSERT INTO tarimas (codigo_barras, numero_producto, numero_tarima, numero_usuario, cantidad_cajas, peso, numero_venta, descripcion)
            VALUES (@CodigoBarras, @NumeroProducto, @NumeroTarima, @NumeroUsuario, @CantidadCajas, @Peso, @NumeroVenta, @Descripcion);
            SELECT LAST_INSERT_ID();";

        try
        {
            tarima.Id = await connection.ExecuteScalarAsync<int>(sql, new
            {
                tarima.CodigoBarras,
                tarima.NumeroProducto,
                tarima.NumeroTarima,
                tarima.NumeroUsuario,
                tarima.CantidadCajas,
                tarima.Peso,
                tarima.NumeroVenta,
                tarima.Descripcion
            });
        }
        catch (MySqlConnector.MySqlException ex) when (ex.Number == 1062)
        {
            throw new DuplicateCodeBarrasException("Ya existe una tarima con este código de barras.");
        }

        return tarima;
    }

    public async Task UpdateAsync(Tarima tarima)
    {
        using var connection = CreateConnection();
        const string sql = @"
            UPDATE tarimas SET 
                numero_producto = @NumeroProducto,
                numero_tarima = @NumeroTarima,
                numero_usuario = @NumeroUsuario,
                cantidad_cajas = @CantidadCajas,
                peso = @Peso,
                numero_venta = @NumeroVenta,
                descripcion = @Descripcion
            WHERE id_tarima = @Id";

        await connection.ExecuteAsync(sql, new
        {
            tarima.Id,
            tarima.NumeroProducto,
            tarima.NumeroTarima,
            tarima.NumeroUsuario,
            tarima.CantidadCajas,
            tarima.Peso,
            tarima.NumeroVenta,
            tarima.Descripcion
        });
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = CreateConnection();
        const string sql = "DELETE FROM tarimas WHERE id_tarima = @Id";
        await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<bool> ExistsAsync(int id)
    {
        using var connection = CreateConnection();
        const string sql = "SELECT COUNT(1) FROM tarimas WHERE id_tarima = @Id";
        var count = await connection.ExecuteScalarAsync<int>(sql, new { Id = id });
        return count > 0;
    }
}
