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
        private static string _ConnectionString = $"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog={DatabaseInstaller.DATABASE_NAME};Integrated Security=True;Connect Timeout=30;";

        private static DBConnectionFactorySqlServer _ConnectionFactorySqlServer;
        private static DatabaseInstaller _DatabaseInstaller = new DatabaseInstaller();

        static DataAccessBLL()
        {
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
                _DatabaseInstaller.Install(conn);
            }
        }

        public static SqlConnection CreateConnection()
        {
            return _ConnectionFactorySqlServer.CreateConnection();
        }

    }
}
