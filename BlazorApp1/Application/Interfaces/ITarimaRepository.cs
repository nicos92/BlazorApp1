using BlazorApp1.Entities;

namespace BlazorApp1.Application.Interfaces;

public interface ITarimaRepository
{
    Task<IEnumerable<Tarima>> GetAllAsync();
    Task<IEnumerable<Tarima>> GetByDateAsync(DateTime fecha);
    Task<IEnumerable<Tarima>> FilterTarimasAsync(
        string? numeroProducto = null,
        string? numeroTarima = null,
        string? numeroUsuario = null,
        string? numeroVenta = null,
        DateTime? fechaRegistro = null,
        string? legajo = null,
        string? nombreUsuario = null,
        int? cantidadCajasMin = null,
        decimal? pesoMin = null);
    Task<Tarima?> GetByIdAsync(int id);
    Task<Tarima?> GetByCodigoBarrasAndFechaAsync(string codigoBarras, DateTime fecha);
    Task<Tarima> CreateAsync(Tarima tarima);
    Task UpdateAsync(Tarima tarima);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
