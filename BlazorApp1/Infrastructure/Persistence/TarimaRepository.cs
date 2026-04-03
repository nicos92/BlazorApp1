using BlazorApp1.Application.Interfaces;
using BlazorApp1.Entities;

namespace BlazorApp1.Infrastructure.Persistence;

public class TarimaRepository : ITarimaRepository
{
    private readonly List<Tarima> _tarimas = new();
    private int _nextId = 1;

    public Task<IEnumerable<Tarima>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Tarima>>(_tarimas.OrderByDescending(t => t.FechaCreacion));
    }

    public Task<Tarima?> GetByIdAsync(int id)
    {
        var tarima = _tarimas.FirstOrDefault(t => t.Id == id);
        return Task.FromResult(tarima);
    }

    public Task<Tarima?> GetByCodigoBarrasAndFechaAsync(string codigoBarras, DateTime fecha)
    {
        var tarima = _tarimas.FirstOrDefault(t => 
            t.CodigoBarras == codigoBarras && 
            t.FechaCreacion.Date == fecha.Date);
        return Task.FromResult(tarima);
    }

    public Task<Tarima> CreateAsync(Tarima tarima)
    {
        tarima.Id = _nextId++;
        tarima.FechaCreacion = DateTime.Now;
        _tarimas.Add(tarima);
        return Task.FromResult(tarima);
    }

    public Task UpdateAsync(Tarima tarima)
    {
        var index = _tarimas.FindIndex(t => t.Id == tarima.Id);
        if (index >= 0)
        {
            _tarimas[index] = tarima;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var tarima = _tarimas.FirstOrDefault(t => t.Id == id);
        if (tarima != null)
        {
            _tarimas.Remove(tarima);
        }
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int id)
    {
        var exists = _tarimas.Any(t => t.Id == id);
        return Task.FromResult(exists);
    }
}
