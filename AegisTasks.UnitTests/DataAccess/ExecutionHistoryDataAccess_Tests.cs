using AegisTasks.Core.DTO;
using AegisTasks.DataAccess.ConnectionFactory;
using AegisTasks.DataAccess.DataAccesses;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AegisTasks.UnitTests.DataAccess
{
    [TestClass]
    public class ExecutionHistoryDataAccess_Tests
    {
        private static string _ConnectionString = Properties.Settings.Default.SqlServerConnectionString;
        private static DBConnectionFactorySqlServer _FactorySqlServer = null;
        private static ExecutionHistoryDataAccess _ExecutionHistoryDA = null;
        private static UsersDataAccess _UsersDA = null;

        // Constantes de usuario de prueba
        private const string TEST_USERNAME = "executionhistorytestuser";
        private const string TEST_USER_FIRSTNAME = "Test";
        private const string TEST_USER_LASTNAME = "User";
        private const string TEST_USER_PASSWORD = "Password123!";

        // Constantes de workflows de prueba
        private const string WORKFLOW_ID_1 = "WF001";
        private const string WORKFLOW_NAME_1 = "Test Workflow";
        private const string WORKFLOW_ID_2 = "WF002";
        private const string WORKFLOW_NAME_2 = "Workflow Update Test";
        private const string WORKFLOW_ID_3 = "WF003";
        private const string WORKFLOW_NAME_3 = "Workflow Active";
        private const string WORKFLOW_ID_4 = "WF004";
        private const string WORKFLOW_NAME_4 = "Workflow Inactive";

        private static UserDTO _TestUser = new UserDTO
        {
            Username = TEST_USERNAME,
            FirstName = TEST_USER_FIRSTNAME,
            LastName = TEST_USER_LASTNAME,
            Password = TEST_USER_PASSWORD
        };

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _FactorySqlServer = new DBConnectionFactorySqlServer(_ConnectionString);
            _ExecutionHistoryDA = new ExecutionHistoryDataAccess();
            _UsersDA = new UsersDataAccess();

            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();

                // Crear tablas
                _UsersDA.CreateTable(conn);
                _ExecutionHistoryDA.CreateTable(conn);

                Assert.IsTrue(_UsersDA.Exists(conn), $"La tabla {UsersDataAccess.DB_USERS_TABLE_NAME} no se creó correctamente.");
                Assert.IsTrue(_ExecutionHistoryDA.Exists(conn), $"La tabla {ExecutionHistoryDataAccess.DB_EXECUTION_HISTORY_TABLE_NAME} no se creó correctamente.");

                // Crear usuario de prueba
                _UsersDA.DeleteUser(conn, TEST_USERNAME); // asegurar que no exista
                bool inserted = _UsersDA.InsertUser(conn, _TestUser);
                Assert.IsTrue(inserted, "No se pudo crear el usuario de prueba.");
            }
        }

        private void ClearExecutionsForTestUser(SqlConnection conn)
        {
            using (SqlCommand cmd = new SqlCommand(
                $"DELETE FROM {ExecutionHistoryDataAccess.DB_EXECUTION_HISTORY_TABLE_NAME} WHERE {ExecutionHistoryDataAccess.DB_EXECUTION_HISTORY_TABLE_FIELD_USERNAME} = @Username",
                conn))
            {
                cmd.Parameters.AddWithValue("@Username", TEST_USERNAME);
                cmd.ExecuteNonQuery();
            }
        }

        [TestMethod]
        public void ExecutionHistoryDataAccess_Should_RegisterAndGetExecution()
        {
            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();
                ClearExecutionsForTestUser(conn);

                // Registrar ejecución
                _ExecutionHistoryDA.RegisterExecution(conn, WORKFLOW_ID_1, WORKFLOW_NAME_1, TEST_USERNAME);

                // Obtener ejecuciones activas
                List<ExecutionHistoryLineDTO> executions = _ExecutionHistoryDA.GetExecutionsByUser(conn, TEST_USERNAME);
                Assert.IsNotNull(executions);
                Assert.AreEqual(1, executions.Count);

                var exec = executions[0];
                Assert.AreEqual(WORKFLOW_ID_1, exec.WorkflowId);
                Assert.AreEqual(WORKFLOW_NAME_1, exec.WorkflowName);
                Assert.IsTrue(exec.Active);
                Assert.IsFalse(exec.Success);
            }
        }

        [TestMethod]
        public void ExecutionHistoryDataAccess_Should_UpdateExecution()
        {
            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();
                ClearExecutionsForTestUser(conn);

                // Registrar ejecución
                _ExecutionHistoryDA.RegisterExecution(conn, WORKFLOW_ID_2, WORKFLOW_NAME_2, TEST_USERNAME);
                var executions = _ExecutionHistoryDA.GetExecutionsByUser(conn, TEST_USERNAME);
                var exec = executions.Find(e => e.WorkflowId == WORKFLOW_ID_2);

                Assert.IsNotNull(exec);

                // Actualizar ejecución
                _ExecutionHistoryDA.UpdateExecution(conn, exec.Id, true);

                // Leer de nuevo
                executions = _ExecutionHistoryDA.GetExecutionsByUser(conn, TEST_USERNAME);
                var updatedExec = executions.Find(e => e.Id == exec.Id);

                Assert.IsTrue(updatedExec.Success);
                Assert.IsNotNull(updatedExec.EndDate);
            }
        }

        [TestMethod]
        public void ExecutionHistoryDataAccess_Should_GetExecutionsIncludingInactive()
        {
            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();
                ClearExecutionsForTestUser(conn);

                // Registrar dos ejecuciones
                _ExecutionHistoryDA.RegisterExecution(conn, WORKFLOW_ID_3, WORKFLOW_NAME_3, TEST_USERNAME);
                _ExecutionHistoryDA.RegisterExecution(conn, WORKFLOW_ID_4, WORKFLOW_NAME_4, TEST_USERNAME);

                // Marcamos la segunda como inactiva
                string deactivateQuery = $"UPDATE {ExecutionHistoryDataAccess.DB_EXECUTION_HISTORY_TABLE_NAME} SET {ExecutionHistoryDataAccess.DB_EXECUTION_HISTORY_TABLE_FIELD_ACTIVE} = 0 WHERE {ExecutionHistoryDataAccess.DB_EXECUTION_HISTORY_TABLE_FIELD_WORKFLOWID} = @WorkflowId";
                using (SqlCommand cmd = new SqlCommand(deactivateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@WorkflowId", WORKFLOW_ID_4);
                    cmd.ExecuteNonQuery();
                }

                // Obtener solo activas
                var activeExecutions = _ExecutionHistoryDA.GetExecutionsByUser(conn, TEST_USERNAME);
                Assert.IsTrue(activeExecutions.Exists(e => e.WorkflowId == WORKFLOW_ID_3));
                Assert.IsFalse(activeExecutions.Exists(e => e.WorkflowId == WORKFLOW_ID_4));

                // Obtener todas incluyendo inactivas
                var allExecutions = _ExecutionHistoryDA.GetExecutionsByUser(conn, TEST_USERNAME, true);
                Assert.IsTrue(allExecutions.Exists(e => e.WorkflowId == WORKFLOW_ID_3));
                Assert.IsTrue(allExecutions.Exists(e => e.WorkflowId == WORKFLOW_ID_4));
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            if (_FactorySqlServer != null)
            {
                using (SqlConnection conn = _FactorySqlServer.CreateConnection())
                {
                    conn.Open();

                    // Limpiar registros de ejecución del usuario
                    string deleteExecutionsQuery = $"DELETE FROM {ExecutionHistoryDataAccess.DB_EXECUTION_HISTORY_TABLE_NAME} WHERE {ExecutionHistoryDataAccess.DB_EXECUTION_HISTORY_TABLE_FIELD_USERNAME} = @Username";
                    using (SqlCommand cmd = new SqlCommand(deleteExecutionsQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", TEST_USERNAME);
                        cmd.ExecuteNonQuery();
                    }

                    // Eliminar usuario
                    _UsersDA.DeleteUser(conn, TEST_USERNAME);
                }
            }
        }
    }
}
