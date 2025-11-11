using System.Data;
using AegisTasks.DataAccess.ConnectionFactory;

namespace AegisTasks.DataAccess.Common
{
    /// <summary>
    /// Clase base para servicios SQL Server.
    /// Proporciona acceso a la fábrica de conexiones.
    /// </summary>
    public abstract class AegisServiceSqlServerBase
    {
        /// <summary>
        /// Fábrica de conexiones SQL Server.
        /// </summary>
        protected readonly DBConnectionFactorySqlServer _Factory;

        /// <summary>
        /// Inicializa la clase base con la fábrica de conexiones.
        /// </summary>
        protected AegisServiceSqlServerBase(DBConnectionFactorySqlServer factory)
        {
            _Factory = factory;
        }

        /// <summary>
        /// Crea una nueva conexión a la base de datos.
        /// El consumidor debe encargarse de abrir, usar y cerrar la conexión.
        /// </summary>
        protected IDbConnection CreateConnection()
        {
            return _Factory.CreateConnection();
        }
    }
}
