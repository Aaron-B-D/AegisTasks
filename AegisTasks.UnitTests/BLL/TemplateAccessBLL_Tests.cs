using AegisTasks.BLL.DataAccess;
using AegisTasks.Core.DTO;
using AegisTasks.DataAccess.Common.DTO;
using AegisTasks.DataAccess.ConnectionFactory;
using AegisTasks.DataAccess.DataAccesses;
using AegisTasks.TasksLibrary.TaskPlan;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace AegisTasks.UnitTests.BLL
{
    [TestClass]
    public class TemplateAccessBLL_Tests
    {
        private static string _ConnectionString = Properties.Settings.Default.SqlServerConnectionString;
        private static DBConnectionFactorySqlServer _FactorySqlServer = null;
        private static TemplatesAccess _TemplatesDA = null;
        private static TemplateDataAccessBLL _TemplatesBLL = null;
        private static UsersDataAccess _UsersDA = null;

        #region Constantes de prueba

        private const string TEST_USERNAME = "templateblltestuser";
        private const string TEST_USER_FIRSTNAME = "Test";
        private const string TEST_USER_LASTNAME = "User";
        private const string TEST_USER_PASSWORD = "Password123!";

        private const string TEMPLATE_NAME = "Test WriteInFile Template";
        private const string TEMPLATE_DESCRIPTION = "Template para pruebas";
        private static readonly string TEMPLATE_WORKFLOW_ID = WriteInFilePlan.CALL_NAME;
        private const string TEMPLATE_WORKFLOW_VERSION = "1.0";

        private const string TEST_FILE_PATH = @"C:\Temp\testfile.txt";
        private const string TEST_FILE_CONTENT = "Contenido de prueba";
        private const bool CREATE_FILE_IF_NOT_EXISTS = true;
        private const bool CREATE_DIR_IF_NOT_EXISTS = true;
        private const bool APPEND_CONTENT = false;

        private const string COPY_SRC_DIR = @"C:\Temp\Source";
        private const string COPY_DEST_DIR = @"C:\Temp\Dest";

        private static readonly UserDTO _TestUser = new UserDTO
        {
            Username = TEST_USERNAME,
            FirstName = TEST_USER_FIRSTNAME,
            LastName = TEST_USER_LASTNAME,
            Password = TEST_USER_PASSWORD
        };

        #endregion

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _FactorySqlServer = new DBConnectionFactorySqlServer(_ConnectionString);
            _TemplatesDA = new TemplatesAccess();
            _TemplatesBLL = new TemplateDataAccessBLL();
            _UsersDA = new UsersDataAccess();

            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();
                _UsersDA.CreateTable(conn);
                _TemplatesDA.CreateTable(conn);

                _UsersDA.DeleteUser(conn, TEST_USERNAME);
                bool inserted = _UsersDA.InsertUser(conn, _TestUser);
                Assert.IsTrue(inserted, "No se pudo crear el usuario de prueba.");
            }
        }

        #region Helper: Limpiar templates del usuario de prueba
        private void ClearUserTemplates(SqlConnection conn)
        {
            using (SqlCommand cmd = new SqlCommand(
                $"DELETE FROM {TemplatesAccess.DB_TEMPLATES_TABLE_NAME} WHERE {TemplatesAccess.DB_TEMPLATES_TABLE_FIELD_CREATEDBY} = @CreatedBy",
                conn))
            {
                cmd.Parameters.AddWithValue("@CreatedBy", TEST_USERNAME);
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region Tests TemplateDataAccessBLL

        [TestMethod]
        public void InsertTemplate_Should_Work()
        {
            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();
                ClearUserTemplates(conn);

                WriteInFilePlanInputParams inputParams = new WriteInFilePlanInputParams(
                    new FileInfo(TEST_FILE_PATH),
                    TEST_FILE_CONTENT,
                    CREATE_FILE_IF_NOT_EXISTS,
                    CREATE_DIR_IF_NOT_EXISTS,
                    APPEND_CONTENT
                );

                TemplateDTO templateDTO = new TemplateDTO
                {
                    WorkflowId = TEMPLATE_WORKFLOW_ID,
                    WorkflowVersion = TEMPLATE_WORKFLOW_VERSION,
                    CreatedBy = TEST_USERNAME,
                    Name = TEMPLATE_NAME,
                    Description = TEMPLATE_DESCRIPTION
                };
                templateDTO.SetInputParameters(inputParams);

                int templateId = _TemplatesBLL.InsertTemplate(conn, templateDTO);
                Assert.IsTrue(templateId > 0, "No se pudo insertar el template.");
            }
        }

        [TestMethod]
        public void GetTemplates_Should_ReturnInsertedTemplate()
        {
            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();
                ClearUserTemplates(conn);

                WriteInFilePlanInputParams inputParams = new WriteInFilePlanInputParams(
                    new FileInfo(TEST_FILE_PATH),
                    TEST_FILE_CONTENT,
                    CREATE_FILE_IF_NOT_EXISTS,
                    CREATE_DIR_IF_NOT_EXISTS,
                    APPEND_CONTENT
                );

                TemplateDTO templateDTO = new TemplateDTO
                {
                    WorkflowId = TEMPLATE_WORKFLOW_ID,
                    WorkflowVersion = TEMPLATE_WORKFLOW_VERSION,
                    CreatedBy = TEST_USERNAME,
                    Name = TEMPLATE_NAME,
                    Description = TEMPLATE_DESCRIPTION
                };
                templateDTO.SetInputParameters(inputParams);
                _TemplatesBLL.InsertTemplate(conn, templateDTO);

                List<TemplateDTO> templates = _TemplatesBLL.GetTemplates(conn, TEST_USERNAME);
                Assert.IsTrue(templates.Count > 0, "No se recuperó la plantilla.");

                TemplateDTO retrieved = templates[0];
                object deserializedInput = retrieved.GetInputParameters();
                Assert.IsInstanceOfType(deserializedInput, typeof(WriteInFilePlanInputParams));
            }
        }

        [TestMethod]
        public void UpdateTemplate_Should_Work()
        {
            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();
                ClearUserTemplates(conn);

                WriteInFilePlanInputParams inputParams = new WriteInFilePlanInputParams(
                    new FileInfo(TEST_FILE_PATH),
                    TEST_FILE_CONTENT,
                    CREATE_FILE_IF_NOT_EXISTS,
                    CREATE_DIR_IF_NOT_EXISTS,
                    APPEND_CONTENT
                );

                TemplateDTO templateDTO = new TemplateDTO
                {
                    WorkflowId = TEMPLATE_WORKFLOW_ID,
                    WorkflowVersion = TEMPLATE_WORKFLOW_VERSION,
                    CreatedBy = TEST_USERNAME,
                    Name = TEMPLATE_NAME,
                    Description = TEMPLATE_DESCRIPTION
                };
                templateDTO.SetInputParameters(inputParams);
                int templateId = _TemplatesBLL.InsertTemplate(conn, templateDTO);

                templateDTO.Id = templateId;
                templateDTO.Name = "Updated Name";
                templateDTO.Description = "Updated Description";
                bool updated = _TemplatesBLL.UpdateTemplate(conn, templateDTO);
                Assert.IsTrue(updated, "No se pudo actualizar la plantilla.");

                TemplateDTO updatedTemplate = _TemplatesBLL.GetTemplates(conn, TEST_USERNAME)[0];
                Assert.AreEqual("Updated Name", updatedTemplate.Name);
                Assert.AreEqual("Updated Description", updatedTemplate.Description);
            }
        }

        [TestMethod]
        public void DeleteTemplate_Should_Work()
        {
            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();
                ClearUserTemplates(conn);

                WriteInFilePlanInputParams inputParams = new WriteInFilePlanInputParams(
                    new FileInfo(TEST_FILE_PATH),
                    TEST_FILE_CONTENT,
                    CREATE_FILE_IF_NOT_EXISTS,
                    CREATE_DIR_IF_NOT_EXISTS,
                    APPEND_CONTENT
                );

                TemplateDTO templateDTO = new TemplateDTO
                {
                    WorkflowId = TEMPLATE_WORKFLOW_ID,
                    WorkflowVersion = TEMPLATE_WORKFLOW_VERSION,
                    CreatedBy = TEST_USERNAME,
                    Name = TEMPLATE_NAME,
                    Description = TEMPLATE_DESCRIPTION
                };
                templateDTO.SetInputParameters(inputParams);
                int templateId = _TemplatesBLL.InsertTemplate(conn, templateDTO);

                bool deleted = _TemplatesBLL.DeleteTemplate(conn, templateId, TEST_USERNAME);
                Assert.IsTrue(deleted, "No se pudo borrar la plantilla.");

                List<TemplateDTO> templates = _TemplatesBLL.GetTemplates(conn, TEST_USERNAME);
                Assert.IsTrue(templates.TrueForAll(t => t.Id != templateId), "La plantilla sigue apareciendo después del borrado.");
            }
        }

        #endregion

        #region Tests InputParams Intensivos

        [TestMethod]
        public void WriteInFilePlanInputParams_Should_SerializeAndDeserialize()
        {
            WriteInFilePlanInputParams inputParams = new WriteInFilePlanInputParams(
                new FileInfo(TEST_FILE_PATH),
                TEST_FILE_CONTENT,
                CREATE_FILE_IF_NOT_EXISTS,
                CREATE_DIR_IF_NOT_EXISTS,
                APPEND_CONTENT
            );

            string json = inputParams.ToJson();
            Assert.IsFalse(string.IsNullOrEmpty(json));

            WriteInFilePlanInputParams deserialized = WriteInFilePlanInputParams.FromJson(json);
            Assert.AreEqual(inputParams.FilePath.FullName, deserialized.FilePath.FullName);
            string deserializedContent = null;
            if (deserialized.Content is JsonElement je && je.ValueKind == JsonValueKind.String)
            {
                deserializedContent = je.GetString();
            }
            Assert.AreEqual(inputParams.Content, deserializedContent);
            Assert.AreEqual(inputParams.CreateFileIfNotExists, deserialized.CreateFileIfNotExists);
            Assert.AreEqual(inputParams.CreateDirectoryIfNotExists, deserialized.CreateDirectoryIfNotExists);
            Assert.AreEqual(inputParams.AppendContent, deserialized.AppendContent);
        }

        [TestMethod]
        public void CopyDirectoryPlanInputParams_Should_ConstructValidateSerializeDeserialize()
        {
            DirectoryInfo source = new DirectoryInfo(COPY_SRC_DIR);
            DirectoryInfo dest = new DirectoryInfo(COPY_DEST_DIR);

            CopyDirectoryPlanInputParams inputParams = new CopyDirectoryPlanInputParams(
                true, false, true, source, dest, 5
            );

            Assert.IsTrue(inputParams.IsValid());

            string json = inputParams.ToJson();
            Assert.IsFalse(string.IsNullOrEmpty(json));

            CopyDirectoryPlanInputParams deserialized = CopyDirectoryPlanInputParams.FromJson(json);
            Assert.AreEqual(inputParams.CreateDestinationDirectoryIfNotExists, deserialized.CreateDestinationDirectoryIfNotExists);
            Assert.AreEqual(inputParams.OverwriteDirectoriesIfExist, deserialized.OverwriteDirectoriesIfExist);
            Assert.AreEqual(inputParams.OverwriteFilesIfExist, deserialized.OverwriteFilesIfExist);
            Assert.AreEqual(inputParams.DirectoryToCopy.FullName, deserialized.DirectoryToCopy.FullName);
            Assert.AreEqual(inputParams.DestinationDirectory.FullName, deserialized.DestinationDirectory.FullName);
            Assert.AreEqual(inputParams.CopyDepth, deserialized.CopyDepth);
        }

        #endregion

        [ClassCleanup]
        public static void ClassCleanup()
        {
            if (_FactorySqlServer != null)
            {
                using (SqlConnection conn = _FactorySqlServer.CreateConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        $"DELETE FROM {TemplatesAccess.DB_TEMPLATES_TABLE_NAME} WHERE {TemplatesAccess.DB_TEMPLATES_TABLE_FIELD_CREATEDBY} = @CreatedBy",
                        conn))
                    {
                        cmd.Parameters.AddWithValue("@CreatedBy", TEST_USERNAME);
                        cmd.ExecuteNonQuery();
                    }

                    _UsersDA.DeleteUser(conn, TEST_USERNAME);
                }
            }
        }
    }
}
