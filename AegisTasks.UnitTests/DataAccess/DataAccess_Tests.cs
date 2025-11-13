using Microsoft.VisualStudio.TestTools.UnitTesting;
using AegisTasks.DataAccess.ConnectionFactory;
using Microsoft.Data.SqlClient;
using AegisTasks.DataAccess.DataAccesses;
using System;

namespace AegisTasks.UnitTests.DataAccess
{
    [TestClass]
    public class DataAccess_Tests
    {
        private static string _ConnectionString = Properties.Settings.Default.SqlServerConnectionString;
        private static DBConnectionFactorySqlServer _FactorySqlServer = null;

        private static UsersDataAccess _UserDA = null;
        private static UserParametersAccess _UserParamsDA = null;
        private static ExecutionHistoryDataAccess _ExeHistoryDA = null;
        private static TemplatesAccess _TemplatesAccess = null;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _FactorySqlServer = new DBConnectionFactorySqlServer(_ConnectionString);

            _UserDA = new UsersDataAccess();
            _UserParamsDA = new UserParametersAccess();
            _ExeHistoryDA = new ExecutionHistoryDataAccess();
            _TemplatesAccess = new TemplatesAccess();
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            if (_FactorySqlServer != null)
            {
                using (SqlConnection conn = _FactorySqlServer.CreateConnection())
                {
                    conn.Open();
                    _UserParamsDA.DropTable(conn);
                    _ExeHistoryDA.DropTable(conn);
                    _TemplatesAccess.DropTable(conn);
                    _UserDA.DropTable(conn);
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

                    _UserDA.CreateTable(conn);
                    _UserParamsDA.CreateTable(conn);
                    _ExeHistoryDA.CreateTable(conn);
                    _TemplatesAccess.CreateTable(conn);

                    Assert.IsTrue(_UserDA.Exists(conn), "La tabla Users no fue creada.");
                    Assert.IsTrue(_UserParamsDA.Exists(conn), "La tabla UserParameters no fue creada.");
                    Assert.IsTrue(_ExeHistoryDA.Exists(conn), "La tabla ExecutionHistory no fue creada.");
                    Assert.IsTrue(_TemplatesAccess.Exists(conn), "La tabla Templates no fue creada.");
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fallo la creación de las tablas: {ex.Message}");
            }
        }
    }
}
