using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Models;

public class TarimaModel
{
    public int Id { get; set; }
    
    public string CodigoBarras { get; set; } = "";

    public long? CodigoBarrasLong
    {
        get => long.TryParse(CodigoBarras, out var result) ? result : null;
        set => CodigoBarras = value?.ToString() ?? "";
    }

    public string NumeroProducto { get; set; } = "";

    public int? NumeroProductoInt
    {
        get => int.TryParse(NumeroProducto, out var result) ? result : null;
        set => NumeroProducto = value?.ToString() ?? "";
    }

    public string NumeroTarima { get; set; } = "";

    public int? NumeroTarimaInt
    {
        get => int.TryParse(NumeroTarima, out var result) ? result : null;
        set => NumeroTarima = value?.ToString() ?? "";
    }

    public string NumeroUsuario { get; set; } = "";

    public int? NumeroUsuarioInt
    {
        get => int.TryParse(NumeroUsuario, out var result) ? result : null;
        set => NumeroUsuario = value?.ToString() ?? "";
    }

    [Range(1, 999, ErrorMessage = "La cantidad debe ser entre 1 y 999")]
    public int CantidadCajas { get; set; }

    [Range(0, 9999.99, ErrorMessage = "El peso debe ser entre 0 y 9999.99")]
    public decimal Peso { get; set; }

    [Required(ErrorMessage = "El número de venta es requerido")]
    [RegularExpression(@"^\d{2}-\d{6}$", ErrorMessage = "El formato debe ser XX-XXXXXX")]
    public string NumeroVenta { get; set; } = "";

    public string Descripcion { get; set; } = "";
}
