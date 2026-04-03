using BlazorApp1.Application.Interfaces;
using BlazorApp1.Entities;
using BlazorApp1.Models;

namespace BlazorApp1.Application.Services;

public interface ITarimaService
{
    Task<bool> SaveTarimaAsync(TarimaModel model);
    Task<IEnumerable<Tarima>> GetAllTarimasAsync();
}

public class TarimaService : ITarimaService
{
    private readonly ITarimaRepository _repository;

    public TarimaService(ITarimaRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> SaveTarimaAsync(TarimaModel model)
    {
        var existingTarima = await _repository.GetByCodigoBarrasAndFechaAsync(
            model.CodigoBarras, 
            DateTime.Now);

        if (existingTarima != null)
        {
            return false;
        }

        var tarima = new Tarima
        {
            CodigoBarras = model.CodigoBarras,
            NumeroProducto = model.NumeroProducto,
            NumeroTarima = model.NumeroTarima,
            NumeroUsuario = model.NumeroUsuario,
            CantidadCajas = model.CantidadCajas,
            Peso = model.Peso,
            NumeroVenta = model.NumeroVenta,
            Descripcion = model.Descripcion
        };

        await _repository.CreateAsync(tarima);
        return true;
    }

    public async Task<IEnumerable<Tarima>> GetAllTarimasAsync()
    {
        return await _repository.GetAllAsync();
    }
}
