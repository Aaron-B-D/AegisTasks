using Microsoft.VisualStudio.TestTools.UnitTesting;
using AegisTasks.DataAccess;
using System;
using AegisTasks.DataAccess.ConnectionFactory;
using Microsoft.Data.SqlClient;
using AegisTasks.DataAccess.DataAccesses;

namespace AegisTasks.UnitTests.DataAccess
{
    [TestClass]
    public class DataAccess_Tests
    {
        private static string _ConnectionString = Properties.Settings.Default.SqlServerConnectionString;
        private static DBConnectionFactorySqlServer _FactorySqlServer = null;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context) {
            _FactorySqlServer = new DBConnectionFactorySqlServer(_ConnectionString);
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            if (_FactorySqlServer != null)
            {
                using (SqlConnection conn = _FactorySqlServer.CreateConnection())
                {
                    conn.Open();
                    new UserParametersAccess().DropTable(conn);
                    new ExecutionHistoryDataAccess().DropTable(conn);
                    new TemplatesAccess().DropTable(conn);
                    new UsersDataAccess().DropTable(conn);
                }
            }
        }

        [TestMethod]
        public void DataAccessService_Should_Connect()
        {
            try
            {
                using (SqlConnection conn = _FactorySqlServer.CreateConnection())
                {
                    conn.Open();
                }

            }
            catch (Exception ex)
            {
                Assert.Fail($"Fallo la inicialización de DataAccessService: {ex.Message}");
            }
        }

        [TestMethod]
        public void DataAccessService_Should_CreateTables()
        {
            try
            {
                using (SqlConnection conn = _FactorySqlServer.CreateConnection())
                {
                    conn.Open();

                    Assert.IsTrue(new DatabaseInstaller().DatabaseExists(conn));

                    new UsersDataAccess().CreateTable(conn);
                    new UserParametersAccess().CreateTable(conn);
                    new ExecutionHistoryDataAccess().CreateTable(conn);
                    new TemplatesAccess().CreateTable(conn);

                    Assert.IsTrue(tableExists(conn, $"{UsersDataAccess.DB_USERS_TABLE_NAME}"), "La tabla Users no fue creada.");
                    Assert.IsTrue(tableExists(conn, $"{UserParametersAccess.DB_USER_PARAMETERS_TABLE_NAME}"), "La tabla UserParameters no fue creada.");
                    Assert.IsTrue(tableExists(conn, ExecutionHistoryDataAccess.DB_EXECUTION_HISTORY_TABLE_NAME), "La tabla ExecutionHistory no fue creada.");
                    Assert.IsTrue(tableExists(conn, TemplatesAccess.DB_TEMPLATES_TABLE_NAME), "La tabla Templates no fue creada.");
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fallo la creación de las tablas: {ex.Message}");
            }
        }

        private bool tableExists(SqlConnection conn, string tableName)
        {
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName", conn))
            {
                cmd.Parameters.AddWithValue("@TableName", tableName);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
    }
}
