using AegisTasks.Core.DTO;
using AegisTasks.DataAccess.ConnectionFactory;
using AegisTasks.DataAccess.DataAccesses;
using AegisTasks.TasksLibrary.TaskPlan;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace AegisTasks.UnitTests.DataAccess
{
    [TestClass]
    public class TemplatesDataAccess_Tests
    {
        private static string _ConnectionString = Properties.Settings.Default.SqlServerConnectionString;
        private static DBConnectionFactorySqlServer _FactorySqlServer = null;
        private static TemplatesAccess _TemplatesDA = null;
        private static UsersDataAccess _UsersDA = null;

        private const string TEST_USERNAME = "templatesdatatestuser";
        private static UserDTO _TestUser = new UserDTO
        {
            Username = TEST_USERNAME,
            FirstName = "Test",
            LastName = "User",
            Password = "Password123!"
        };

        private static readonly WriteInFilePlanInputParams WRITE_PARAMS = new WriteInFilePlanInputParams(
            new FileInfo(@"C:\Temp\testfile.txt"),
            "Contenido de prueba",
            true,
            true,
            false
        );

        private static readonly CopyDirectoryPlanInputParams COPY_PARAMS = new CopyDirectoryPlanInputParams(
            true,
            true,
            true,
            new DirectoryInfo(@"C:\Temp\Source"),
            new DirectoryInfo(@"C:\Temp\Dest"),
            2
        );

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _FactorySqlServer = new DBConnectionFactorySqlServer(_ConnectionString);
            _TemplatesDA = new TemplatesAccess();
            _UsersDA = new UsersDataAccess();

            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();

                _UsersDA.CreateTable(conn);
                _TemplatesDA.CreateTable(conn);

                Assert.IsTrue(_UsersDA.Exists(conn), $"La tabla {UsersDataAccess.DB_USERS_TABLE_NAME} no se creó correctamente.");
                Assert.IsTrue(_TemplatesDA.Exists(conn), $"La tabla {TemplatesAccess.DB_TEMPLATES_TABLE_NAME} no se creó correctamente.");

                _UsersDA.DeleteUser(conn, _TestUser.Username);
                bool inserted = _UsersDA.InsertUser(conn, _TestUser);
                Assert.IsTrue(inserted, "No se pudo crear el usuario de prueba.");
            }
        }

        [TestMethod]
        public void TemplatesAccess_Should_InsertAndReadTemplate()
        {
            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();

                // Limpiar templates del usuario
                DeleteTemplatesForUser(conn);

                // Insertar template WriteInFile
                int templateId = _TemplatesDA.InsertTemplate(
                    conn,
                    WriteInFilePlan.CALL_NAME,
                    "1.0",
                    TEST_USERNAME,
                    "Write File Template",
                    "Test template write file",
                    WRITE_PARAMS.ToJson()
                );
                Assert.IsTrue(templateId > 0, "No se pudo insertar template WriteInFile.");

                // Insertar template CopyDirectory
                int templateId2 = _TemplatesDA.InsertTemplate(
                    conn,
                    CopyDirectoryPlan.CALL_NAME,
                    "1.0",
                    TEST_USERNAME,
                    "Copy Dir Template",
                    "Test template copy directory",
                    COPY_PARAMS.ToJson()
                );
                Assert.IsTrue(templateId2 > 0, "No se pudo insertar template CopyDirectory.");

                // Leer templates
                List<Dictionary<string, object>> templates = _TemplatesDA.GetTemplates(conn, TEST_USERNAME);
                Assert.AreEqual(2, templates.Count);
            }
        }

        [TestMethod]
        public void TemplatesAccess_Should_UpdateTemplate()
        {
            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();
                DeleteTemplatesForUser(conn);

                int templateId = _TemplatesDA.InsertTemplate(
                    conn,
                    WriteInFilePlan.CALL_NAME,
                    "1.0",
                    TEST_USERNAME,
                    "Write File Template",
                    "Test template write file",
                    WRITE_PARAMS.ToJson()
                );

                bool updated = _TemplatesDA.UpdateTemplate(
                    conn,
                    templateId,
                    TEST_USERNAME,
                    "Updated Template",
                    "Descripción actualizada",
                    WRITE_PARAMS.ToJson()
                );
                Assert.IsTrue(updated, "No se pudo actualizar el template.");

                List<Dictionary<string, object>> templates = _TemplatesDA.GetTemplates(conn, TEST_USERNAME);
                Dictionary<string, object> updatedTemplate = templates.Find(t => (int)t[TemplatesAccess.DB_TEMPLATES_TABLE_FIELD_ID] == templateId);
                Assert.AreEqual("Updated Template", updatedTemplate[TemplatesAccess.DB_TEMPLATES_TABLE_FIELD_NAME]);
            }
        }

        [TestMethod]
        public void TemplatesAccess_Should_DeleteTemplate()
        {
            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();
                DeleteTemplatesForUser(conn);

                int templateId = _TemplatesDA.InsertTemplate(
                    conn,
                    WriteInFilePlan.CALL_NAME,
                    "1.0",
                    TEST_USERNAME,
                    "Write File Template",
                    "Test template write file",
                    WRITE_PARAMS.ToJson()
                );

                bool deleted = _TemplatesDA.DeleteTemplate(conn, templateId, TEST_USERNAME);
                Assert.IsTrue(deleted, "No se pudo eliminar template.");

                List<Dictionary<string, object>> templates = _TemplatesDA.GetTemplates(conn, TEST_USERNAME);
                Assert.AreEqual(0, templates.Count);
            }
        }

        private void DeleteTemplatesForUser(SqlConnection conn)
        {
            using (SqlCommand cmd = new SqlCommand(
                $"UPDATE {TemplatesAccess.DB_TEMPLATES_TABLE_NAME} SET {TemplatesAccess.DB_TEMPLATES_TABLE_FIELD_ACTIVE} = 0 WHERE {TemplatesAccess.DB_TEMPLATES_TABLE_FIELD_CREATEDBY} = @Username",
                conn))
            {
                cmd.Parameters.AddWithValue("@Username", TEST_USERNAME);
                cmd.ExecuteNonQuery();
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
                    DeleteTemplatesForUserStatic(conn);
                    _UsersDA.DeleteUser(conn, TEST_USERNAME);
                }
            }
        }

        private static void DeleteTemplatesForUserStatic(SqlConnection conn)
        {
            using (SqlCommand cmd = new SqlCommand(
                $"UPDATE {TemplatesAccess.DB_TEMPLATES_TABLE_NAME} SET {TemplatesAccess.DB_TEMPLATES_TABLE_FIELD_ACTIVE} = 0 WHERE {TemplatesAccess.DB_TEMPLATES_TABLE_FIELD_CREATEDBY} = @Username",
                conn))
            {
                cmd.Parameters.AddWithValue("@Username", TEST_USERNAME);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
