using System;
using System.IO;

namespace AegisTasks.DataAccess
{
    public static class ConnectionProvider
    {
        public static string Get()
        {
            // Obtiene el directorio base del ensamblado actual
            string baseDir = AppContext.BaseDirectory;

            // Construye la ruta relativa a la base de datos
            string dbPath = Path.Combine(baseDir, "Database", $"{DatabaseInstaller.DATABASE_NAME}.mdf");

            // Validación: si no existe, lanzar excepción clara
            if (!File.Exists(dbPath))
            {
                throw new FileNotFoundException(
                    $"No se encontró la base de datos en la ruta esperada: {dbPath}. " +
                    "Asegúrate de que la carpeta Database esté configurada para copiarse al directorio de salida.");
            }

            return $@"Data Source=(LocalDB)\MSSQLLocalDB;
                      AttachDbFilename={dbPath};
                      Integrated Security=True;
                      Connect Timeout=30;";
        }
    }
}