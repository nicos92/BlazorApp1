namespace BlazorApp1.Configuration;

internal static class Settings
{
    public static string STRCONNECTION
    {
        get
        {
            string connStr = Environment.GetEnvironmentVariable("GESTIONTARIMAS_CONNECTION_STRING") ?? string.Empty;
            if (string.IsNullOrEmpty(connStr))
                throw new InvalidOperationException(
                    "La variable de entorno GESTIONTARIMAS_CONNECTION_STRING no está definida.");
            return connStr;
        }
    }
}
