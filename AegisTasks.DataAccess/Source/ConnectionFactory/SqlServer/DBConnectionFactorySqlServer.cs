using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AegisTasks.DataAccess.ConnectionFactory
{
    public class DBConnectionFactorySqlServer
    {
        private readonly string _ConnectionString;

        public DBConnectionFactorySqlServer(string connectionString)
        {
            _ConnectionString = connectionString;
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_ConnectionString);
        }
    }
}
