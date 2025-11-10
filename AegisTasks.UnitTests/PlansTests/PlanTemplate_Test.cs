using AegisTasks.TasksLibrary.TaskPlan;
using AegisTasks.TasksLibrary.WorkflowHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegisTasks.UnitTests.PlansTests
{
    [TestClass]
#pragma warning disable S2187 // Test classes should contain at least one test case
    public class PlanTemplate_Test
#pragma warning restore S2187 // Test classes should contain at least one test case
    {
        #region CONFIGURACIÓN GLOBAL

        //private static PreconfiguredAegisWorkflowsManager _Host;

        //// Parámetros de test
        //private const int WORKFLOW_TIMEOUT_SECONDS = 30;

        //private readonly string WORKFLOW_NAME = WriteInFilePlan.CALL_NAME;

        #endregion CONFIGURACIÓN GLOBAL

        #region CONFIGURACIÓN DE CLASE

        //[ClassInitialize]
        //public static void Init(TestContext context)
        //{
        //    _Host = new PreconfiguredAegisWorkflowsManager();
        //    _Host.Start();
        //}

        //[ClassCleanup]
        //public static void Cleanup()
        //{
        //    _Host?.Stop();
        //}

        #endregion CONFIGURACIÓN DE CLASE

        #region TEST METHODS

        // Aquí puedes agregar tus nuevos métodos de prueba
        //[TestMethod]
        //public async Task TestPlaceholder()
        //{
        //    // Ejemplo de placeholder para nuevas pruebas
        //    await Task.CompletedTask;
        //}

        #endregion TEST METHODS

        #region MÉTODOS AUXILIARES

        //private static void verifyFileWasCreated(string filePath, DateTime testStart, string expectedContent)
        //{
        //    Assert.IsTrue(File.Exists(filePath), "El archivo no fue creado: " + filePath);
        //    string content = File.ReadAllText(filePath);
        //    Assert.AreEqual(expectedContent, content);
        //    Assert.IsTrue(File.GetLastWriteTime(filePath) >= testStart);
        //}

        //private static void cleanupTestDirectory(string testDirectory)
        //{
        //    if (Directory.Exists(testDirectory))
        //    {
        //        try
        //        {
        //            Directory.Delete(testDirectory, true);
        //            Debug.WriteLine("Directorio limpiado: " + testDirectory);
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.WriteLine("Error al limpiar directorio: " + ex.Message);
        //        }
        //    }
        //}

        //private static string getProjectRootDirectory()
        //{
        //    string currentDir = Directory.GetCurrentDirectory();
        //    DirectoryInfo projectRoot = Directory.GetParent(currentDir).Parent?.Parent;
        //    return projectRoot?.FullName ?? currentDir;
        //}

        #endregion MÉTODOS AUXILIARES
    }
}
