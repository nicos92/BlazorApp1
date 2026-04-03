namespace BlazorApp1.Entities;

public class Tarima
{
    public int Id { get; set; }
    public string CodigoBarras { get; set; } = "";
    public string NumeroProducto { get; set; } = "";
    public string NumeroTarima { get; set; } = "";
    public string NumeroUsuario { get; set; } = "";
    public int CantidadCajas { get; set; }
    public decimal Peso { get; set; }
    public string NumeroVenta { get; set; } = "";
    public string? Descripcion { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
}
