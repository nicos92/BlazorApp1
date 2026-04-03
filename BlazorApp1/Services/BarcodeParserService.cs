namespace BlazorApp1.Services;

public class BarcodeParseResult
{
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
    public string NumeroProducto { get; set; } = "";
    public string NumeroTarima { get; set; } = "";
    public string NumeroUsuario { get; set; } = "";
    public int CantidadCajas { get; set; }
    public decimal Peso { get; set; }
    public string NumeroVentaPrefijo { get; set; } = "";
}

public interface IBarcodeParserService
{
    BarcodeParseResult Parse(string barcode);
    bool ValidateFormat(string barcode);
}

public class BarcodeParserService : IBarcodeParserService
{
    private const int ExpectedLength = 30;
    private const string ExpectedPrefix = "9998";
    private const char ExpectedStartChar = '0';

    public BarcodeParseResult Parse(string barcode)
    {
        var result = new BarcodeParseResult();

        if (string.IsNullOrEmpty(barcode))
        {
            result.IsValid = false;
            result.ErrorMessage = "El código de barras está vacío.";
            return result;
        }

        if (barcode.Length < ExpectedLength)
        {
            result.IsValid = false;
            return result;
        }

        if (barcode.Length > ExpectedLength)
        {
            barcode = barcode.Substring(0, ExpectedLength);
        }

        if (!ValidateFormat(barcode))
        {
            result.IsValid = false;
            return result;
        }

        result.NumeroProducto = barcode.Substring(1, 6);
        result.NumeroTarima = barcode.Substring(7, 6);
        result.NumeroUsuario = barcode.Substring(19, 2);

        var cantidadCajasStr = barcode.Substring(21, 3);
        result.CantidadCajas = int.TryParse(cantidadCajasStr, out var cajas) ? cajas : 0;

        var pesoStr = barcode.Substring(24, 6);
        result.Peso = decimal.TryParse(pesoStr, out var peso) ? peso / 100 : 0;

        var currentYear = DateTime.Now.Year.ToString().Substring(2);
        result.NumeroVentaPrefijo = $"{currentYear}-";

        result.IsValid = true;
        return result;
    }

    public bool ValidateFormat(string barcode)
    {
        if (barcode.Length != ExpectedLength)
            return false;

        if (barcode[0] != ExpectedStartChar)
            return false;

        if (barcode.Substring(13, 4) != ExpectedPrefix)
            return false;

        return true;
    }

    public static string GetValidationError(string barcode)
    {
        if (string.IsNullOrEmpty(barcode))
            return "El código de barras está vacío.";

        if (barcode.Length < 30)
            return null!;

        if (barcode[0] != '0')
            return "El código de barras debe iniciar con cero.";

        if (barcode.Substring(13, 4) != "9998")
            return "Las posiciones 14-17 del código de barras deben ser 9998.";

        return null!;
    }
}
