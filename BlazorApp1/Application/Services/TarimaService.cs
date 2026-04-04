using BlazorApp1.Application.Interfaces;
using BlazorApp1.Entities;
using BlazorApp1.Models;

namespace BlazorApp1.Application.Services;

public interface ITarimaService
{
    Task<bool> SaveTarimaAsync(TarimaModel model);
    Task<bool> UpdateTarimaAsync(TarimaModel model);
    Task<IEnumerable<Tarima>> GetAllTarimasAsync();
    Task<int> GetTarimasCountAsync();
    Task<IEnumerable<Tarima>> GetTarimasByDateAsync(DateTime date);
    Task<int> GetTarimasCountByDateAsync(DateTime date);
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

        try
        {
            await _repository.CreateAsync(tarima);
        }
        catch (DuplicateCodeBarrasException)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> UpdateTarimaAsync(TarimaModel model)
    {
        if (model.Id == 0)
        {
            return false;
        }

        var tarima = new Tarima
        {
            Id = model.Id,
            CodigoBarras = model.CodigoBarras,
            NumeroProducto = model.NumeroProducto,
            NumeroTarima = model.NumeroTarima,
            NumeroUsuario = model.NumeroUsuario,
            CantidadCajas = model.CantidadCajas,
            Peso = model.Peso,
            NumeroVenta = model.NumeroVenta,
            Descripcion = model.Descripcion
        };

        await _repository.UpdateAsync(tarima);
        return true;
    }

    public async Task<IEnumerable<Tarima>> GetAllTarimasAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<int> GetTarimasCountAsync()
    {
        return await _repository.GetCountAsync();
    }

    public async Task<IEnumerable<Tarima>> GetTarimasByDateAsync(DateTime date)
    {
        return await _repository.GetByDateAsync(date);
    }

    public async Task<int> GetTarimasCountByDateAsync(DateTime date)
    {
        var tarimas = await _repository.GetByDateAsync(date);
        return tarimas.Count();
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
        return await _repository.FilterTarimasAsync(
            numeroProducto,
            numeroTarima,
            numeroUsuario,
            numeroVenta,
            fechaRegistro,
            legajo,
            nombreUsuario,
            cantidadCajasMin,
            pesoMin);
    }
}
