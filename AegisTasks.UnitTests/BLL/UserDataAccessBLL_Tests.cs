using AegisTasks.BLL.DataAccess;
using AegisTasks.Core.DTO;
using AegisTasks.DataAccess.ConnectionFactory;
using AegisTasks.DataAccess.DataAccesses;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AegisTasks.UnitTests.BLL
{
    [TestClass]
    public class UserDataAccessBLL_Tests
    {
        private static string _ConnectionString = Properties.Settings.Default.SqlServerConnectionString;
        private static DBConnectionFactorySqlServer _FactorySqlServer = null;

        private const string TEST_USERNAME = "testuser";
        private const string TEST_FIRSTNAME = "Test";
        private const string TEST_LASTNAME = "User";
        private const string TEST_PASSWORD = "Password123!";
        private const string NEW_TEST_PASSWORD = "NewPassword456!";

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

        [TestMethod]
        public void CreateUser_Should_CreateUserSuccessfully()
        {
            UserDTO testUser = new UserDTO
            {
                Username = TEST_USERNAME,
                FirstName = TEST_FIRSTNAME,
                LastName = TEST_LASTNAME,
                Password = TEST_PASSWORD
            };

            bool created = UserDataAccessBLL.CreateUser(testUser);
            Assert.IsTrue(created, "No se pudo crear el usuario.");

            // Cleanup
            UserDataAccessBLL.DeleteUser(TEST_USERNAME);
        }

        [TestMethod]
        public void GetUser_Should_ReturnUserWithoutPassword()
        {
            UserDTO testUser = new UserDTO
            {
                Username = TEST_USERNAME,
                FirstName = TEST_FIRSTNAME,
                LastName = TEST_LASTNAME,
                Password = TEST_PASSWORD
            };
            UserDataAccessBLL.DeleteUser(TEST_USERNAME);
            UserDataAccessBLL.CreateUser(testUser);

            UserDTO fetchedUser = UserDataAccessBLL.GetUser(TEST_USERNAME);
            Assert.IsNotNull(fetchedUser);
            Assert.AreEqual(TEST_USERNAME, fetchedUser.Username);
            Assert.IsNull(fetchedUser.Password, "La contraseña no debería devolverse.");

            // Cleanup
            UserDataAccessBLL.DeleteUser(TEST_USERNAME);
        }

        [TestMethod]
        public void CheckPassword_Should_ReturnTrueForCorrectPassword()
        {
            UserDataAccessBLL.DeleteUser(TEST_USERNAME);
            UserDTO testUser = new UserDTO
            {
                Username = TEST_USERNAME,
                FirstName = TEST_FIRSTNAME,
                LastName = TEST_LASTNAME,
                Password = TEST_PASSWORD
            };
            UserDataAccessBLL.CreateUser(testUser);

            bool result = UserDataAccessBLL.IsValidPassword(TEST_USERNAME, TEST_PASSWORD);
            Assert.IsTrue(result, "La contraseña válida no pasó la verificación.");

            // Cleanup
            UserDataAccessBLL.DeleteUser(TEST_USERNAME);
        }

        [TestMethod]
        public void CheckPassword_Should_ReturnFalseForIncorrectPassword()
        {
            UserDataAccessBLL.DeleteUser(TEST_USERNAME);
            UserDTO testUser = new UserDTO
            {
                Username = TEST_USERNAME,
                FirstName = TEST_FIRSTNAME,
                LastName = TEST_LASTNAME,
                Password = TEST_PASSWORD
            };
            UserDataAccessBLL.CreateUser(testUser);

            bool result = UserDataAccessBLL.IsValidPassword(TEST_USERNAME, "WrongPassword!");
            Assert.IsFalse(result, "Una contraseña incorrecta pasó la verificación.");

            // Cleanup
            UserDataAccessBLL.DeleteUser(TEST_USERNAME);
        }

        [TestMethod]
        public void UpdatePassword_Should_UpdateSuccessfully()
        {
            UserDataAccessBLL.DeleteUser(TEST_USERNAME);
            UserDTO testUser = new UserDTO
            {
                Username = TEST_USERNAME,
                FirstName = TEST_FIRSTNAME,
                LastName = TEST_LASTNAME,
                Password = TEST_PASSWORD
            };
            UserDataAccessBLL.CreateUser(testUser);

            bool updated = UserDataAccessBLL.UpdatePassword(TEST_USERNAME, TEST_PASSWORD, NEW_TEST_PASSWORD);
            Assert.IsTrue(updated, "No se pudo actualizar la contraseña.");

            Assert.IsTrue(UserDataAccessBLL.IsValidPassword(TEST_USERNAME, NEW_TEST_PASSWORD), "La nueva contraseña no coincide.");
            Assert.IsFalse(UserDataAccessBLL.IsValidPassword(TEST_USERNAME, TEST_PASSWORD), "La antigua contraseña sigue siendo válida.");

            // Cleanup
            UserDataAccessBLL.DeleteUser(TEST_USERNAME);
        }

        [TestMethod]
        public void DeleteUser_Should_DeleteUserSuccessfully()
        {
            UserDataAccessBLL.DeleteUser(TEST_USERNAME);
            UserDTO testUser = new UserDTO
            {
                Username = TEST_USERNAME,
                FirstName = TEST_FIRSTNAME,
                LastName = TEST_LASTNAME,
                Password = TEST_PASSWORD
            };
            UserDataAccessBLL.CreateUser(testUser);

            bool deleted = UserDataAccessBLL.DeleteUser(TEST_USERNAME);
            Assert.IsTrue(deleted, "No se pudo borrar el usuario.");

            UserDTO fetchedUser = UserDataAccessBLL.GetUser(TEST_USERNAME);
            Assert.IsNull(fetchedUser, "El usuario todavía existe después de borrar.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdatePassword_Should_Fail_When_CurrentPasswordIncorrect()
        {
            UserDataAccessBLL.DeleteUser(TEST_USERNAME);
            UserDTO testUser = new UserDTO
            {
                Username = TEST_USERNAME,
                FirstName = TEST_FIRSTNAME,
                LastName = TEST_LASTNAME,
                Password = TEST_PASSWORD
            };
            UserDataAccessBLL.CreateUser(testUser);

            // Intentar actualizar con la contraseña actual incorrecta
            UserDataAccessBLL.UpdatePassword(TEST_USERNAME, "WrongCurrentPassword!", NEW_TEST_PASSWORD);

            // Cleanup
            UserDataAccessBLL.DeleteUser(TEST_USERNAME);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdatePassword_Should_Fail_When_NewPasswordInvalid()
        {
            UserDataAccessBLL.DeleteUser(TEST_USERNAME);
            UserDTO testUser = new UserDTO
            {
                Username = TEST_USERNAME,
                FirstName = TEST_FIRSTNAME,
                LastName = TEST_LASTNAME,
                Password = TEST_PASSWORD
            };
            UserDataAccessBLL.CreateUser(testUser);

            // Intentar actualizar con una nueva contraseña que no cumple criterios
            string invalidPassword = "short"; // demasiado corta, sin mayúsculas, números ni especiales
            UserDataAccessBLL.UpdatePassword(TEST_USERNAME, TEST_PASSWORD, invalidPassword);

            // Cleanup
            UserDataAccessBLL.DeleteUser(TEST_USERNAME);
        }


        [ClassCleanup]
        public static void ClassCleanup()
        {
            UserDataAccessBLL.DeleteUser(TEST_USERNAME);
        }
    }
}
