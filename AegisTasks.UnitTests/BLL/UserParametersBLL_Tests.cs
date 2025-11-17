using AegisTasks.BLL.DataAccess;
using AegisTasks.Core.Common;
using AegisTasks.Core.DTO;
using AegisTasks.DataAccess.ConnectionFactory;
using AegisTasks.DataAccess.DataAccesses;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AegisTasks.UnitTests.BLL
{
    [TestClass]
    public class UserParametersBLL_Tests
    {
        private static string _ConnectionString = Properties.Settings.Default.SqlServerConnectionString;
        private static DBConnectionFactorySqlServer _FactorySqlServer = null;

        private const string TEST_USERNAME = "testuserparambll";
        private const string TEST_FIRSTNAME = "Test";
        private const string TEST_LASTNAME = "User";
        private const string TEST_PASSWORD = "Password123!";

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _FactorySqlServer = new DBConnectionFactorySqlServer(_ConnectionString);

            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();
                UsersDataAccess userDA = new UsersDataAccess();
                userDA.CreateTable(conn);
                Assert.IsTrue(userDA.Exists(conn), $"La tabla {UsersDataAccess.DB_USERS_TABLE_NAME} no se creó correctamente.");

                UserParametersAccess userParametersAccess = new UserParametersAccess();
                userParametersAccess.CreateTable(conn);
                Assert.IsTrue(userDA.Exists(conn), $"La tabla {UserParametersAccess.DB_USER_PARAMETERS_TABLE_NAME} no se creó correctamente.");
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            UserDataAccessBLL.DeleteUser(TEST_USERNAME);
            UserParametersBLL.DeleteUserParameters(TEST_USERNAME);
        }

        [TestMethod]
        public void CreateUser_ShouldAutomaticallyCreateParameters()
        {
            UserDTO user = new UserDTO(TEST_USERNAME, TEST_FIRSTNAME, TEST_LASTNAME, TEST_PASSWORD, DateTime.Now);

            bool created = UserDataAccessBLL.CreateUser(user);
            Assert.IsTrue(created, "No se pudo crear el usuario.");

            UserParameterDTO<SupportedLanguage> langParam = UserParametersBLL.GetParameter<SupportedLanguage>(TEST_USERNAME, UserParameterType.LANGUAGE);
            Assert.IsNotNull(langParam, "No se crearon los parámetros automáticamente.");
            Assert.AreEqual(UserParameterType.LANGUAGE, langParam.Type);
            Assert.AreEqual(SupportedLanguage.ENGLISH, langParam.Value);
        }

        [TestMethod]
        public void ModifyParameter_ShouldUpdateValue()
        {
            UserDTO user = new UserDTO(TEST_USERNAME, TEST_FIRSTNAME, TEST_LASTNAME, TEST_PASSWORD, DateTime.Now);
            UserDataAccessBLL.CreateUser(user);

            UserParameterDTO<object> newParam = new UserParameterDTO<object>(UserParameterType.LANGUAGE, SupportedLanguage.SPANISH);
            bool updated = UserParametersBLL.ModifyParameter(TEST_USERNAME, newParam);
            Assert.IsTrue(updated, "No se pudo modificar el parámetro.");

            UserParameterDTO<SupportedLanguage> langParam = UserParametersBLL.GetParameter<SupportedLanguage>(TEST_USERNAME, UserParameterType.LANGUAGE);
            Assert.AreEqual(SupportedLanguage.SPANISH, langParam.Value);
        }

        [TestMethod]
        public void DeleteUserParameters_ShouldRemoveAllParameters()
        {
            UserDTO user = new UserDTO(TEST_USERNAME, TEST_FIRSTNAME, TEST_LASTNAME, TEST_PASSWORD, DateTime.Now);
            UserDataAccessBLL.CreateUser(user);

            bool deleted = UserParametersBLL.DeleteUserParameters(TEST_USERNAME);
            Assert.IsTrue(deleted, "No se pudieron borrar los parámetros del usuario.");

            UserParameterDTO<SupportedLanguage> langParam = UserParametersBLL.GetParameter<SupportedLanguage>(TEST_USERNAME, UserParameterType.LANGUAGE);
            Assert.IsNull(langParam, "El parámetro todavía existe después de borrar.");
        }

        [TestMethod]
        public void CreateUserParameters_ShouldInsertParametersForExistingUser()
        {
            UserDTO user = new UserDTO(TEST_USERNAME, TEST_FIRSTNAME, TEST_LASTNAME, TEST_PASSWORD, DateTime.Now);
            UserDataAccessBLL.CreateUser(user);

            UserParametersBLL.DeleteUserParameters(TEST_USERNAME);

            bool inserted = UserParametersBLL.CreateUserParameters(TEST_USERNAME);
            Assert.IsTrue(inserted, "No se pudieron crear los parámetros del usuario.");

            UserParameterDTO<SupportedLanguage> langParam = UserParametersBLL.GetParameter<SupportedLanguage>(TEST_USERNAME, UserParameterType.LANGUAGE);
            Assert.IsNotNull(langParam, "El parámetro no se insertó correctamente.");
            Assert.AreEqual(SupportedLanguage.ENGLISH, langParam.Value);
        }

        [TestMethod]
        public void GetParameters_ShouldReturnAllUserParameters()
        {
            UserDTO user = new UserDTO(TEST_USERNAME, TEST_FIRSTNAME, TEST_LASTNAME, TEST_PASSWORD, DateTime.Now);
            UserDataAccessBLL.CreateUser(user);

            UserParameterDTO<object> newParam = new UserParameterDTO<object>(UserParameterType.LANGUAGE, SupportedLanguage.SPANISH);
            UserParametersBLL.ModifyParameter(TEST_USERNAME, newParam);

            UserParametersDTO parameters = UserParametersBLL.GetParameters(TEST_USERNAME);

            Assert.IsNotNull(parameters);
            Assert.AreEqual(TEST_USERNAME, parameters.Username);

            Assert.IsTrue(parameters.TryGetParameter<SupportedLanguage>(UserParameterType.LANGUAGE, out var langParam));
            Assert.IsNotNull(langParam);
            Assert.AreEqual(UserParameterType.LANGUAGE, langParam.Type);
            Assert.AreEqual(SupportedLanguage.SPANISH, langParam.Value);
        }


        [TestCleanup]
        public void TestCleanup()
        {
            UserParametersBLL.DeleteUserParameters(TEST_USERNAME);
            UserDataAccessBLL.DeleteUser(TEST_USERNAME);
        }
    }
}
