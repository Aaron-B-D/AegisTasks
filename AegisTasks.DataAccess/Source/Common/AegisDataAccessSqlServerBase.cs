using System.Data;
using AegisTasks.DataAccess.ConnectionFactory;
using Microsoft.Data.SqlClient;

namespace AegisTasks.DataAccess.Common
{
    /// <summary>
    /// Clase base para servicios SQL Server.
    /// Proporciona acceso a la fábrica de conexiones.
    /// </summary>
    public abstract class AegisDataAccessSqlServerBase
    {
        protected AegisDataAccessSqlServerBase()
        {}


        public abstract void CreateTable(SqlConnection conn);

        public abstract void DropTable(SqlConnection conn);

        public abstract bool Exists(SqlConnection conn);
    }
}
