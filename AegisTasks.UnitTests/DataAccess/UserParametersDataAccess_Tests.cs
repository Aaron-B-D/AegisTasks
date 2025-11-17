using AegisTasks.Core.Common;
using AegisTasks.Core.DTO;
using AegisTasks.DataAccess.ConnectionFactory;
using AegisTasks.DataAccess.DataAccesses;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AegisTasks.UnitTests.DataAccess
{
    [TestClass]
    public class UserParametersDataAccess_Tests
    {
        private static string _ConnectionString = Properties.Settings.Default.SqlServerConnectionString;
        private static DBConnectionFactorySqlServer _FactorySqlServer = null;
        private static UserParametersAccess _ParamDA = null;
        private static UsersDataAccess _UsersDA = null;

        private const string TEST_USERNAME = "userparametersdataaccesstestuser";
        private static UserDTO _TestUser = new UserDTO
        {
            Username = TEST_USERNAME,
            FirstName = "Test",
            LastName = "User",
            Password = "Password123!"
        };

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _FactorySqlServer = new DBConnectionFactorySqlServer(_ConnectionString);
            _ParamDA = new UserParametersAccess();
            _UsersDA = new UsersDataAccess();

            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();

                // Crear tablas
                _UsersDA.CreateTable(conn);
                _ParamDA.CreateTable(conn);

                Assert.IsTrue(_UsersDA.Exists(conn), $"La tabla {UsersDataAccess.DB_USERS_TABLE_NAME} no se creó correctamente.");
                Assert.IsTrue(_ParamDA.Exists(conn), $"La tabla {UserParametersAccess.DB_USER_PARAMETERS_TABLE_NAME} no se creó correctamente.");

                // Crear usuario de prueba
                _UsersDA.DeleteUser(conn, _TestUser.Username); // asegurar que no existe
                bool inserted = _UsersDA.InsertUser(conn, _TestUser);
                Assert.IsTrue(inserted, "No se pudo crear el usuario de prueba.");
            }
        }

        [TestMethod]
        public void UserParametersAccess_Should_InsertAndReadParameter()
        {
            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();

                _ParamDA.DeleteUserParameters(conn, _TestUser.Username);

                bool inserted = _ParamDA.AddParameter(conn, _TestUser.Username, UserParameterType.LANGUAGE, SupportedLanguage.ENGLISH.ToString());
                Assert.IsTrue(inserted, "No se pudo insertar el parámetro del usuario.");

                string value = _ParamDA.GetParameter(conn, _TestUser.Username, UserParameterType.LANGUAGE);
                Assert.AreEqual(SupportedLanguage.ENGLISH.ToString(), value, "El valor del parámetro no coincide.");

                _ParamDA.DeleteUserParameters(conn, _TestUser.Username);
            }
        }

        [TestMethod]
        public void UserParametersAccess_Should_UpdateParameter()
        {
            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();

                _ParamDA.DeleteUserParameters(conn, _TestUser.Username);
                _ParamDA.AddParameter(conn, _TestUser.Username, UserParameterType.LANGUAGE, SupportedLanguage.ENGLISH.ToString());

                bool updated = _ParamDA.UpdateParameter(conn, _TestUser.Username, UserParameterType.LANGUAGE, SupportedLanguage.SPANISH.ToString());
                Assert.IsTrue(updated, "No se pudo actualizar el parámetro.");

                string value = _ParamDA.GetParameter(conn, _TestUser.Username, UserParameterType.LANGUAGE);
                Assert.AreEqual(SupportedLanguage.SPANISH.ToString(), value, "El valor actualizado del parámetro no coincide.");

                _ParamDA.DeleteUserParameters(conn, _TestUser.Username);
            }
        }

        [TestMethod]
        public void UserParametersAccess_Should_DeleteParameter()
        {
            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();

                _ParamDA.DeleteUserParameters(conn, _TestUser.Username);
                _ParamDA.AddParameter(conn, _TestUser.Username, UserParameterType.LANGUAGE, SupportedLanguage.ENGLISH.ToString());

                bool deleted = _ParamDA.DeleteUserParameters(conn, _TestUser.Username);
                Assert.IsTrue(deleted, "No se pudo eliminar los parámetros del usuario.");

                string value = _ParamDA.GetParameter(conn, _TestUser.Username, UserParameterType.LANGUAGE);
                Assert.IsNull(value, "El parámetro todavía existe después de eliminarlo.");
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
                    _ParamDA.DeleteUserParameters(conn, _TestUser.Username);
                    _UsersDA.DeleteUser(conn, _TestUser.Username);
                }
            }
        }
    }
}
