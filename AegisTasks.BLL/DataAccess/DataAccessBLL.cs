using AegisTasks.Core.Common;
using AegisTasks.DataAccess;
using AegisTasks.DataAccess.ConnectionFactory;
using AegisTasks.DataAccess.DataAccesses;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace AegisTasks.BLL.DataAccess
{
    public class DataAccessBLL
    {
        private static readonly string _ConnectionString;

        private static DBConnectionFactorySqlServer _ConnectionFactorySqlServer;
        private static DatabaseInstaller _DatabaseInstaller = new DatabaseInstaller();

        static DataAccessBLL()
        {
            _ConnectionString = ConnectionProvider.Get();
            _ConnectionFactorySqlServer = new DBConnectionFactorySqlServer(_ConnectionString);

        }

        public static bool CanConnect()
        {
            try
            {
                using (SqlConnection conn = _ConnectionFactorySqlServer.CreateConnection())
                {
                    conn.Open();
                }

                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        public static void InstallDatabase()
        {
            using (SqlConnection conn = _ConnectionFactorySqlServer.CreateConnection()) {
                conn.Open();

                _DatabaseInstaller.Install(conn);
            }
        }

        public static bool IsAppDatabaseInstalled()
        {
            bool result = false;

            try
            {
                using (SqlConnection conn = _ConnectionFactorySqlServer.CreateConnection())
                {
                    conn.Open();

                    result = _DatabaseInstaller.IsDatabaseInstalled(conn);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                result = false;
            }

            return result;
        }

        public static bool AppDatabaseExist()
        {
            bool result = false;

            try
            {
                using (SqlConnection conn = _ConnectionFactorySqlServer.CreateConnection())
                {
                    conn.Open();
                    result = _DatabaseInstaller.DatabaseExists(conn);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                result = false;
            }

            return result;
        }

        public static SqlConnection CreateConnection()
        {
            return _ConnectionFactorySqlServer.CreateConnection();
        }

    }
}
