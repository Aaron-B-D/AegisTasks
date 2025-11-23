using AegisTasks.Core.Common;
using AegisTasks.Core.DTO;
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

        //Por defecto "Password123!"
        private readonly UserDTO DEFAULT_ADMIN_USER;

        private UsersDataAccess _UsersAccess = new UsersDataAccess();
        private UserParametersAccess _UserParametersAccess = new UserParametersAccess();
        private TemplatesAccess _TemplatesAccess = new TemplatesAccess();
        private ExecutionHistoryDataAccess _ExecutionHistoryAccess = new ExecutionHistoryDataAccess();

        public DatabaseInstaller()
        {
            //Por defecto "Password123!"
            DEFAULT_ADMIN_USER = new UserDTO("admin", "Administrator", "", "a109e36947ad56de1dca1cc49f0ef8ac9ad9a7b1aa0df41fb3c4cb73c1ff01ea");
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

        public bool IsDatabaseInstalled(SqlConnection conn)
        {
            bool result = false;

            try
            {
                bool tablesExist = this._UsersAccess.Exists(conn) && this._UserParametersAccess.Exists(conn) && this._TemplatesAccess.Exists(conn) && this._ExecutionHistoryAccess.Exists(conn);

                bool adminUserExist = false;
                if (tablesExist)
                {
                    adminUserExist = _UsersAccess.GetUser(conn, DEFAULT_ADMIN_USER.Username) != null;
                }
                result = adminUserExist && tablesExist;
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
                result = false;
            }

            return result;

        }

        public void Install(SqlConnection conn)
        {
            _UsersAccess.CreateTable(conn);
            _UserParametersAccess.CreateTable(conn);
            _TemplatesAccess.CreateTable(conn);
            _ExecutionHistoryAccess.CreateTable(conn);

            _UsersAccess.InsertUser(conn, DEFAULT_ADMIN_USER);
            _UserParametersAccess.AddParameter(conn, DEFAULT_ADMIN_USER.Username, UserParameterType.LANGUAGE, SupportedLanguage.ENGLISH.ToString());
        }
    }
}
