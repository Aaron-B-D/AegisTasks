using AegisTasks.DataAccess.ConnectionFactory;
using AegisTasks.DataAccess.Services;
using Microsoft.Data.SqlClient;
using System;


namespace AegisTasks.DataAccess
{
    /// <summary>
    /// Punto de entrada principal para el acceso a datos de la aplicación.
    /// </summary>
    public class DataAccessService
    {
        /// <summary>
        /// Fábrica de conexiones a SQL Server
        /// </summary>
        private readonly DBConnectionFactorySqlServer _Factory;

        /// <summary>
        /// Servicio de acceso a datos de usuarios.
        /// </summary>
        public UsersService Users { get; private set; }

        /// <summary>
        /// Inicializa una nueva instancia del servicio de acceso a datos.
        /// </summary>
        /// <param name="connectionString">Cadena de conexión a la base de datos SQL Server.</param>
        public DataAccessService(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("La cadena de conexión no puede estar vacía.", nameof(connectionString));

            // Intentar abrir la conexión para verificar que la base exista
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("No se pudo conectar a la base de datos. Verifica que exista y que la cadena de conexión sea correcta.", ex);
            }

            // Crear la fábrica de conexiones
            _Factory = new DBConnectionFactorySqlServer(connectionString);

            // Instanciar los servicios
            Users = new UsersService(_Factory);
        }
    }
}