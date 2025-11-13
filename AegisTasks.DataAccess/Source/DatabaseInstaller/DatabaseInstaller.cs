using AegisTasks.DataAccess.DataAccesses;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace AegisTasks.DataAccess
{
    public class DatabaseInstaller
    {
        public static readonly string DATABASE_NAME = "AegisTasks";

        private UsersDataAccess _UsersAccess = new UsersDataAccess();
        private UserParametersAccess _UserParametersAccess = new UserParametersAccess();
        private TemplatesAccess _TemplatesAccess = new TemplatesAccess();
        private ExecutionHistoryDataAccess _ExecutionHistoryAccess = new ExecutionHistoryDataAccess();

        public DatabaseInstaller()
        {
        }

        /// <summary>
        /// Comprueba si la base de datos existe en el servidor.
        /// </summary>
        /// <param name="conn">Conexión abierta a SQL Server (normalmente a 'master').</param>
        /// <returns>True si la base de datos existe, False en caso contrario.</returns>
        public bool DatabaseExists(SqlConnection conn)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM sys.databases WHERE name = @dbName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@dbName", DATABASE_NAME);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Install(SqlConnection conn)
        {
            _UsersAccess.CreateTable(conn);
            _UserParametersAccess.CreateTable(conn);
            _TemplatesAccess.CreateTable(conn);
            _ExecutionHistoryAccess.CreateTable(conn);
        }
    }
}
