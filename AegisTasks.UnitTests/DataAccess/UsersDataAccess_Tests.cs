using Microsoft.VisualStudio.TestTools.UnitTesting;
using AegisTasks.DataAccess.ConnectionFactory;
using Microsoft.Data.SqlClient;
using AegisTasks.DataAccess.DataAccesses;
using AegisTasks.Core.DTO;
using System;

namespace AegisTasks.UnitTests.DataAccess
{
    [TestClass]
    public class UsersDataAccess_Tests
    {
        private static string _ConnectionString = Properties.Settings.Default.SqlServerConnectionString;
        private static DBConnectionFactorySqlServer _FactorySqlServer = null;
        private static UsersDataAccess _UserDA = null;

        private const string TEST_USERNAME = "userdataaccesstestuser";
        private const string TEST_FIRSTNAME = "Test";
        private const string TEST_LASTNAME = "User";
        private const string TEST_PASSWORD = "Password123!";
        private const string NEW_TEST_PASSWORD = "NewPassword456!";

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _FactorySqlServer = new DBConnectionFactorySqlServer(_ConnectionString);
            _UserDA = new UsersDataAccess();

            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();
                _UserDA.CreateTable(conn);

                bool tableExists = _UserDA.Exists(conn);
                Assert.IsTrue(tableExists, $"La tabla {UsersDataAccess.DB_USERS_TABLE_NAME} no se creó correctamente.");
            }
        }

        [TestMethod]
        public void UsersDataAccess_Should_InsertAndReadUser()
        {
            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();

                _UserDA.DeleteUser(conn, TEST_USERNAME);

                UserDTO testUser = new UserDTO
                {
                    Username = TEST_USERNAME,
                    FirstName = TEST_FIRSTNAME,
                    LastName = TEST_LASTNAME,
                    Password = TEST_PASSWORD
                };

                bool inserted = _UserDA.InsertUser(conn, testUser);
                Assert.IsTrue(inserted, "No se pudo insertar el usuario.");

                UserDTO fetchedUser = _UserDA.GetUser(conn, TEST_USERNAME);
                Assert.IsNotNull(fetchedUser, "No se pudo leer el usuario insertado.");
                Assert.AreEqual(TEST_USERNAME, fetchedUser.Username);
                Assert.AreEqual(TEST_FIRSTNAME, fetchedUser.FirstName);
                Assert.AreEqual(TEST_LASTNAME, fetchedUser.LastName);
                Assert.AreEqual(TEST_PASSWORD, fetchedUser.Password);

                _UserDA.DeleteUser(conn, TEST_USERNAME);
            }
        }

        [TestMethod]
        public void UsersDataAccess_Should_UpdatePassword()
        {
            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();

                _UserDA.DeleteUser(conn, TEST_USERNAME);

                UserDTO testUser = new UserDTO
                {
                    Username = TEST_USERNAME,
                    FirstName = TEST_FIRSTNAME,
                    LastName = TEST_LASTNAME,
                    Password = TEST_PASSWORD
                };

                bool inserted = _UserDA.InsertUser(conn, testUser);
                Assert.IsTrue(inserted, "No se pudo insertar el usuario para actualizar la contraseña.");

                bool updated = _UserDA.UpdatePassword(conn, TEST_USERNAME, NEW_TEST_PASSWORD);
                Assert.IsTrue(updated, "No se pudo actualizar la contraseña.");

                string fetchedPassword = _UserDA.GetUserPassword(conn, TEST_USERNAME);
                Assert.AreEqual(NEW_TEST_PASSWORD, fetchedPassword, "La contraseña no coincide con la actualizada.");

                _UserDA.DeleteUser(conn, TEST_USERNAME);
            }
        }

        [TestMethod]
        public void UsersDataAccess_Should_DeleteUser()
        {
            using (SqlConnection conn = _FactorySqlServer.CreateConnection())
            {
                conn.Open();

                _UserDA.DeleteUser(conn, TEST_USERNAME);

                UserDTO testUser = new UserDTO
                {
                    Username = TEST_USERNAME,
                    FirstName = TEST_FIRSTNAME,
                    LastName = TEST_LASTNAME,
                    Password = TEST_PASSWORD
                };

                bool inserted = _UserDA.InsertUser(conn, testUser);
                Assert.IsTrue(inserted, "No se pudo insertar el usuario para borrar.");

                bool deleted = _UserDA.DeleteUser(conn, TEST_USERNAME);
                Assert.IsTrue(deleted, "No se pudo borrar el usuario.");

                UserDTO fetchedUser = _UserDA.GetUser(conn, TEST_USERNAME);
                Assert.IsNull(fetchedUser, "El usuario todavía existe después de borrar.");
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
                    _UserDA.DeleteUser(conn, TEST_USERNAME);
                }
            }
        }
    }
}
