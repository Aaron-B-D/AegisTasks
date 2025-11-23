using System;
using System.IO;
using System.Windows.Forms;

namespace AegisTasks.DataAccess
{
    public static class ConnectionProvider
    {
        public static string Get()
        {
            string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string dbPath = Path.Combine(appDataDir, "ABD", "AegisTasks", "Database", $"{DatabaseInstaller.DATABASE_NAME}.mdf");

            if (!File.Exists(dbPath))
            {
                throw new FileNotFoundException($"Base de datos no encontrada en: {dbPath}");
            }

            return $@"Data Source=(LocalDB)\MSSQLLocalDB;
              AttachDbFilename={dbPath};
              Integrated Security=True;
              Connect Timeout=30;";
        }
    }
}