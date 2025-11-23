using AegisTasks.BLL.DataAccess;
using AegisTasks.Core.DTO;
using AegisTasks.DataAccess.ConnectionFactory;
using AegisTasks.DataAccess.DataAccesses;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AegisTasks.UnitTests.BLL
{
    [TestClass]
    public class ExecutionHistoryBLL_Tests
    {
        private static string _ConnectionString = Properties.Settings.Default.SqlServerConnectionString;
        private static DBConnectionFactorySqlServer _FactorySqlServer = null;
        private static UsersDataAccess _UsersDA = null;
        private static ExecutionHistoryDataAccess _ExecutionHistoryDA = null;

        // Constantes reutilizables
        private const string TEST_USERNAME = "executionhistoryblltestuser";
        private const string TEST_FIRSTNAME = "Test";
        private const string TEST_LASTNAME = "User";
        private const string TEST_PASSWORD = "Password123!";

        private const string WF_ID_1 = "WF001";
        private const string WF_NAME_1 = "Test Workflow";

        private const string WF_ID_2 = "WF002";
        private const string WF_NAME_2 = "Workflow Update Test";

        private const string WF_ID_3 = "WF003";
        private const string WF_NAME_3 = "Workflow Active";

        private const string WF_ID_4 = "WF004";
        private const string WF_NAME_4 = "Workflow Inactive";

        private static UserDTO _TestUser = new UserDTO
        {
            Username = TEST_USERNAME,
            FirstName = TEST_FIRSTNAME,
            LastName = TEST_LASTNAME,
            Password = TEST_PASSWORD
        };

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _FactorySqlServer = new DBConnectionFactorySqlServer(_ConnectionString);
            _UsersDA = new UsersDataAccess();
            _ExecutionHistoryDA = new ExecutionHistoryDataAccess();

            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();

                _UsersDA.CreateTable(conn);
                _ExecutionHistoryDA.CreateTable(conn);

                Assert.IsTrue(_UsersDA.Exists(conn), $"La tabla {UsersDataAccess.DB_USERS_TABLE_NAME} no se creó correctamente.");
                Assert.IsTrue(_ExecutionHistoryDA.Exists(conn), $"La tabla {ExecutionHistoryDataAccess.DB_EXECUTION_HISTORY_TABLE_NAME} no se creó correctamente.");

                _UsersDA.DeleteUser(conn, TEST_USERNAME);
                bool inserted = _UsersDA.InsertUser(conn, _TestUser);
                Assert.IsTrue(inserted, "No se pudo crear el usuario de prueba.");
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            CleanupExecutions(TEST_USERNAME);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            CleanupExecutions(TEST_USERNAME);
        }

        private void CleanupExecutions(string username)
        {
            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();
                string deleteQuery = $"DELETE FROM {ExecutionHistoryDataAccess.DB_EXECUTION_HISTORY_TABLE_NAME} WHERE {ExecutionHistoryDataAccess.DB_EXECUTION_HISTORY_TABLE_FIELD_USERNAME} = @Username";
                using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        [TestMethod]
        public void RegisterExecution_Should_CreateExecution()
        {
            ExecutionHistoryDataAccessBLL.RegisterExecution(WF_ID_1, WF_NAME_1, TEST_USERNAME);

            List<ExecutionHistoryLineDTO> executions = ExecutionHistoryDataAccessBLL.GetExecutionsByUser(TEST_USERNAME);
            Assert.IsNotNull(executions);
            Assert.AreEqual(1, executions.Count);

            var exec = executions[0];
            Assert.AreEqual(WF_ID_1, exec.WorkflowId);
            Assert.AreEqual(WF_NAME_1, exec.WorkflowName);
            Assert.IsTrue(exec.Active);
            Assert.IsFalse(exec.Success);
        }

        [TestMethod]
        public void UpdateExecution_Should_SetSuccessAndEndDate()
        {
            // Aseguramos que no haya un workflow activo previo con este ID
            CleanupWorkflow(WF_ID_2);

            var execId = ExecutionHistoryDataAccessBLL.RegisterExecution(WF_ID_2, WF_NAME_2, TEST_USERNAME);
            var executions = ExecutionHistoryDataAccessBLL.GetExecutionsByUser(TEST_USERNAME);
            var exec = executions.Find(e => e.WorkflowId == WF_ID_2);
            Assert.IsNotNull(exec);

            ExecutionHistoryDataAccessBLL.UpdateExecution(exec.Id, true);

            executions = ExecutionHistoryDataAccessBLL.GetExecutionsByUser(TEST_USERNAME);
            var updatedExec = executions.Find(e => e.Id == exec.Id);

            Assert.IsTrue(updatedExec.Success);
            Assert.IsNotNull(updatedExec.EndDate);
        }

        private void CleanupWorkflow(string workflowId)
        {
            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();
                string deactivateQuery = $"UPDATE {ExecutionHistoryDataAccess.DB_EXECUTION_HISTORY_TABLE_NAME} SET {ExecutionHistoryDataAccess.DB_EXECUTION_HISTORY_TABLE_FIELD_ACTIVE} = 0 WHERE {ExecutionHistoryDataAccess.DB_EXECUTION_HISTORY_TABLE_FIELD_WORKFLOWID} = @WorkflowId AND {ExecutionHistoryDataAccess.DB_EXECUTION_HISTORY_TABLE_FIELD_ACTIVE} = 1";
                using (SqlCommand cmd = new SqlCommand(deactivateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@WorkflowId", workflowId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        [TestMethod]
        public void GetExecutionsByUser_Should_IncludeInactive_WhenSpecified()
        {
            ExecutionHistoryDataAccessBLL.RegisterExecution(WF_ID_3, WF_NAME_3, TEST_USERNAME);
            ExecutionHistoryDataAccessBLL.RegisterExecution(WF_ID_4, WF_NAME_4, TEST_USERNAME);

            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();
                string deactivateQuery = $"UPDATE {ExecutionHistoryDataAccess.DB_EXECUTION_HISTORY_TABLE_NAME} SET {ExecutionHistoryDataAccess.DB_EXECUTION_HISTORY_TABLE_FIELD_ACTIVE} = 0 WHERE {ExecutionHistoryDataAccess.DB_EXECUTION_HISTORY_TABLE_FIELD_WORKFLOWID} = @WorkflowId";
                using (SqlCommand cmd = new SqlCommand(deactivateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@WorkflowId", WF_ID_4);
                    cmd.ExecuteNonQuery();
                }
            }

            var activeExecutions = ExecutionHistoryDataAccessBLL.GetExecutionsByUser(TEST_USERNAME);
            Assert.IsTrue(activeExecutions.Exists(e => e.WorkflowId == WF_ID_3));
            Assert.IsFalse(activeExecutions.Exists(e => e.WorkflowId == WF_ID_4));

            var allExecutions = ExecutionHistoryDataAccessBLL.GetExecutionsByUser(TEST_USERNAME, true);
            Assert.IsTrue(allExecutions.Exists(e => e.WorkflowId == WF_ID_3));
            Assert.IsTrue(allExecutions.Exists(e => e.WorkflowId == WF_ID_4));
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            if (_FactorySqlServer != null)
            {
                using (SqlConnection conn = _FactorySqlServer.CreateConnection())
                {
                    conn.Open();
                    _UsersDA.DeleteUser(conn, TEST_USERNAME);
                }
            }
        }
    }
}
