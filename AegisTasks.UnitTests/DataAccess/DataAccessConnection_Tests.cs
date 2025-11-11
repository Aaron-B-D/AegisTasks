using Microsoft.VisualStudio.TestTools.UnitTesting;
using AegisTasks.DataAccess;
using System;

namespace AegisTasks.UnitTests.DataAccess
{
    [TestClass]
    public class DataAccessConnection_Tests
    {
        private static string _ConnectionString = Properties.Settings.Default.SqlServerConnectionString;

        [TestMethod]
        public void DataAccessService_Should_InitializeCorrectly()
        {
            try
            {
                // Inicializar el servicio de acceso a datos
                var dataService = new DataAccessService(_ConnectionString);

                // Verificar que la propiedad UsersService no sea null
                Assert.IsNotNull(dataService.Users, "El servicio UsersService no se inicializó correctamente.");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fallo la inicialización de DataAccessService: {ex.Message}");
            }
        }
    }
}
