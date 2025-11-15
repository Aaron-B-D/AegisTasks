using AegisTasks.BLL.DataAccess;
using AegisTasks.Core.Common;
using AegisTasks.Core.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Threading;

namespace AegisTasks.UnitTests.BLL
{
    [TestClass]
    public class SessionManager_Tests
    {
        private const string TEST_USERNAME = "testsessionuser";
        private const string TEST_FIRSTNAME = "Test";
        private const string TEST_LASTNAME = "User";
        private const string TEST_PASSWORD = "Password123!";

        [TestInitialize]
        public void TestInitialize()
        {
            // Aseguramos que el usuario no exista antes de cada test
            UserParametersBLL.DeleteUserParameters(TEST_USERNAME);
            UserDataAccessBLL.DeleteUser(TEST_USERNAME);

            // Crear usuario de prueba
            UserDTO user = new UserDTO(TEST_USERNAME, TEST_FIRSTNAME, TEST_LASTNAME, TEST_PASSWORD, DateTime.Now);
            UserDataAccessBLL.CreateUser(user);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Limpiar después de cada test
            SessionManager.Logout();
            UserParametersBLL.DeleteUserParameters(TEST_USERNAME);
            UserDataAccessBLL.DeleteUser(TEST_USERNAME);
        }

        [TestMethod]
        public void Login_ShouldSetCurrentUserAndParameters()
        {
            bool loginResult = SessionManager.Login(TEST_USERNAME, TEST_PASSWORD);

            Assert.IsTrue(loginResult, "El login debería ser exitoso.");
            Assert.IsNotNull(SessionManager.CurrentUser, "CurrentUser no se estableció.");
            Assert.IsNotNull(SessionManager.CurrentUserParameters, "CurrentUserParameters no se estableció.");
            Assert.AreEqual(TEST_USERNAME, SessionManager.CurrentUser.Username, "El usuario actual no coincide.");
        }

        [TestMethod]
        public void Logout_ShouldClearCurrentUserAndParameters()
        {
            SessionManager.Login(TEST_USERNAME, TEST_PASSWORD);
            SessionManager.Logout();

            Assert.IsNull(SessionManager.CurrentUser, "CurrentUser debería ser null después del logout.");
            Assert.IsNull(SessionManager.CurrentUserParameters, "CurrentUserParameters debería ser null después del logout.");
        }

        [TestMethod]
        public void IsLoggedIn_ShouldReturnCorrectStatus()
        {
            Assert.IsFalse(SessionManager.IsLoggedIn, "No debería estar logueado inicialmente.");

            SessionManager.Login(TEST_USERNAME, TEST_PASSWORD);
            Assert.IsTrue(SessionManager.IsLoggedIn, "Debería estar logueado después del login.");

            SessionManager.Logout();
            Assert.IsFalse(SessionManager.IsLoggedIn, "No debería estar logueado después del logout.");
        }

        [TestMethod]
        public void Login_ShouldApplyUserLanguageSettings()
        {
            // Modificar el parámetro de idioma del usuario
            UserParameterDTO<object> langParam = new UserParameterDTO<object>(UserParameterType.LANGUAGE, Language.SPANISH);
            UserParametersBLL.ModifyParameter(TEST_USERNAME, langParam);

            SessionManager.Login(TEST_USERNAME, TEST_PASSWORD);

            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;

            Assert.AreEqual("es-ES", currentCulture.Name, "La cultura actual no coincide con la del usuario.");
            Assert.AreEqual("es-ES", currentUICulture.Name, "La cultura de la interfaz no coincide con la del usuario.");
        }

        [TestMethod]
        public void Login_InvalidPassword_ShouldReturnFalse()
        {
            bool loginResult = SessionManager.Login(TEST_USERNAME, "WrongPassword");

            Assert.IsFalse(loginResult, "El login con contraseña incorrecta debería fallar.");
            Assert.IsNull(SessionManager.CurrentUser, "CurrentUser debería ser null si el login falla.");
            Assert.IsNull(SessionManager.CurrentUserParameters, "CurrentUserParameters debería ser null si el login falla.");
        }
    }
}
