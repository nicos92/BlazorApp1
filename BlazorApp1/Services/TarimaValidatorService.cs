using System.Text.RegularExpressions;
using BlazorApp1.Models;

namespace BlazorApp1.Services;

public class TarimaValidationResult
{
    public bool IsValid { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
}

public interface ITarimaValidatorService
{
    TarimaValidationResult ValidateForSubmit(TarimaModel tarima, string codigoBarras, string numeroVenta);
    bool IsFormReady(bool barcodeAutocompleted, string? barcodeError, int cantidadCajas, string numeroVenta);
}

public class TarimaValidatorService : ITarimaValidatorService
{
    private static readonly Regex NumeroVentaRegex = new(@"^\d{2}-\d{6}$", RegexOptions.Compiled);

    private const int MinCajas = 1;
    private const int MaxCajas = 999;

    public TarimaValidationResult ValidateForSubmit(TarimaModel tarima, string codigoBarras, string numeroVenta)
    {
        var result = new TarimaValidationResult { IsValid = true };

        if (string.IsNullOrEmpty(codigoBarras) || codigoBarras.Length != 30)
        {
            result.IsValid = false;
            result.ErrorCode = "required_fields_missing";
            result.ErrorMessage = "El código de barras es obligatorio.";
            return result;
        }

        if (codigoBarras[0] != '0')
        {
            result.IsValid = false;
            result.ErrorCode = "invalid_barcode";
            result.ErrorMessage = "El código de barras debe iniciar con cero.";
            return result;
        }

        if (codigoBarras.Substring(13, 4) != "9998")
        {
            result.IsValid = false;
            result.ErrorCode = "invalid_barcode";
            result.ErrorMessage = "Las posiciones 14-17 del código de barras deben ser 9998.";
            return result;
        }

        if (tarima.CantidadCajas < MinCajas || tarima.CantidadCajas > MaxCajas)
        {
            result.IsValid = false;
            result.ErrorCode = "invalid_cantidad";
            result.ErrorMessage = $"La cantidad debe ser entre {MinCajas} y {MaxCajas}.";
            return result;
        }

        if (!NumeroVentaRegex.IsMatch(numeroVenta))
        {
            result.IsValid = false;
            result.ErrorCode = "invalid_venta";
            result.ErrorMessage = "El formato del número de venta debe ser XX-XXXXXX.";
            return result;
        }

        return result;
    }

    public bool IsFormReady(bool barcodeAutocompleted, string? barcodeError, int cantidadCajas, string numeroVenta)
    {
        if (!barcodeAutocompleted)
            return false;

        if (barcodeError != null)
            return false;

        if (cantidadCajas < MinCajas || cantidadCajas > MaxCajas)
            return false;

        return NumeroVentaRegex.IsMatch(numeroVenta);
    }
}
