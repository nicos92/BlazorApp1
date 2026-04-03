using BlazorApp1.Entities;

namespace BlazorApp1.Application.Interfaces;

public interface ITarimaRepository
{
    Task<IEnumerable<Tarima>> GetAllAsync();
    Task<Tarima?> GetByIdAsync(int id);
    Task<Tarima?> GetByCodigoBarrasAndFechaAsync(string codigoBarras, DateTime fecha);
    Task<Tarima> CreateAsync(Tarima tarima);
    Task UpdateAsync(Tarima tarima);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
