using AegisTasks.Core.Events;
using AegisTasks.TasksLibrary.TaskPlan;
using AegisTasks.TasksLibrary.WorkflowHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AegisTasks.UnitTests.PlansTests
{
    [TestClass]
    public class CopyDirectoryPlan_Test
    {
        #region CONFIGURACIÓN GLOBAL

        private static PreconfiguredAegisWorkflowsManager _Host;

        // Parámetros de test
        private const int WORKFLOW_TIMEOUT_SECONDS = 30;

        private readonly string WORKFLOW_NAME = CopyDirectoryPlan.CALL_NAME;

        // Carpetas y archivos
        private static readonly string TEST_FOLDER_NAME = Path.Combine(
            Properties.Settings.Default.TestRootFolder,
            "CopyDirectoryPlanTests"
        );

        private const int CREATE_FOLDERS_DEPTH = 10;

        private const bool CLEAN_TEST_DIRECTORY = true;


        #endregion CONFIGURACIÓN GLOBAL

        #region CONFIGURACIÓN DE CLASE

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            _Host = new PreconfiguredAegisWorkflowsManager();
            _Host.Start();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            _Host?.Stop();
        }

        #endregion CONFIGURACIÓN DE CLASE

        #region TEST METHODS

        [TestMethod]
        [Description("Ejecuta una tarea de copiado de directorio en un directorio que no existe. Debe fallar al no existir el directorio a copiar.")]
        public async Task CopyDirectoryPlan_ShouldFail_WhenSourceDirectoryDoesNotExist()
        {
            string projectRoot = getProjectRootDirectory();
            DirectoryInfo nonExistentSource = new DirectoryInfo(Path.Combine(projectRoot, TEST_FOLDER_NAME, "NonExistentSource_" + Guid.NewGuid()));
            DirectoryInfo destination = new DirectoryInfo(Path.Combine(projectRoot, TEST_FOLDER_NAME, "Destination_" + Guid.NewGuid()));

            Assert.IsFalse(nonExistentSource.Exists, "El directorio de origen no debería existir.");

            CopyDirectoryPlanParams input = new CopyDirectoryPlanParams
            (
                new CopyDirectoryPlanInputParams(
                    createDestinationDirectoryIfNotExists: true,
                    directoryToCopy: nonExistentSource,
                    destinationDirectory: destination,
                    overwriteDirectoriesIfExist: false,
                    overwriteFilesIfExist: false
                )
            );

            string workflowId = await _Host.StartWorkflow(WORKFLOW_NAME, input);

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            void onTerminated(object sender, TaskPlanEventArgs e)
            {
                if (e.WorkflowInstanceId == workflowId)
                    tcs.TrySetResult(true);
            }

            _Host.TaskPlanTerminated += onTerminated;

            using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(WORKFLOW_TIMEOUT_SECONDS)))
            {
                await Task.WhenAny(tcs.Task, Task.Delay(Timeout.Infinite, cts.Token));
            }

            _Host.TaskPlanTerminated -= onTerminated;

            Assert.IsTrue(tcs.Task.IsCompleted, "El workflow no terminó en estado Terminated.");
            Assert.IsFalse(destination.Exists, "El directorio de destino no debería haberse creado.");
        }

        [TestMethod]
        [Description("El directorio de origen existe, pero el destino no y no se permite su creación. Debe fallar al intentar crear el directorio de destino inexistente.")]
        public async Task CopyDirectoryPlan_ShouldFail_WhenDestinationDoesNotExistAndCreationNotAllowed()
        {
            string projectRoot = getProjectRootDirectory();
            DirectoryInfo sourceDir = new DirectoryInfo(Path.Combine(projectRoot, TEST_FOLDER_NAME, "Source_" + Guid.NewGuid()));
            DirectoryInfo nonExistentDestination = new DirectoryInfo(Path.Combine(projectRoot, TEST_FOLDER_NAME, "NonExistentDestination_" + Guid.NewGuid()));

            Directory.CreateDirectory(sourceDir.FullName);
            File.WriteAllText(Path.Combine(sourceDir.FullName, "test.txt"), "Contenido de prueba");

            Assert.IsTrue(sourceDir.Exists, "El directorio de origen debería existir.");
            Assert.IsFalse(nonExistentDestination.Exists, "El directorio de destino no debería existir.");

            CopyDirectoryPlanParams input = new CopyDirectoryPlanParams
            (
                new CopyDirectoryPlanInputParams(
                    createDestinationDirectoryIfNotExists: false,
                    directoryToCopy: sourceDir,
                    destinationDirectory: nonExistentDestination,
                    overwriteDirectoriesIfExist: false,
                    overwriteFilesIfExist: false
                )                
            );

            string workflowId = await _Host.StartWorkflow(WORKFLOW_NAME, input);

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            void onTerminated(object sender, TaskPlanEventArgs e)
            {
                if (e.WorkflowInstanceId == workflowId)
                    tcs.TrySetResult(true);
            }

            _Host.TaskPlanTerminated += onTerminated;

            using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(WORKFLOW_TIMEOUT_SECONDS)))
            {
                await Task.WhenAny(tcs.Task, Task.Delay(Timeout.Infinite, cts.Token));
            }

            _Host.TaskPlanTerminated -= onTerminated;

            Assert.IsTrue(tcs.Task.IsCompleted, "El workflow no terminó en estado Terminated.");
            Assert.IsFalse(nonExistentDestination.Exists, "El directorio de destino no debería haberse creado.");

            if (CLEAN_TEST_DIRECTORY)
            {
                cleanupTestDirectory(sourceDir.FullName);
            }
        }

        [TestMethod]
        [Description("El directorio de origen existe pero está vacío, y el destino no existe pero se permite su creación. El workflow debe completarse correctamente y el destino debe quedar vacío.")]
        public async Task CopyDirectoryPlan_ShouldComplete_WhenSourceEmptyAndDestinationCreated()
        {
            string projectRoot = getProjectRootDirectory();
            DirectoryInfo sourceDir = new DirectoryInfo(Path.Combine(projectRoot, TEST_FOLDER_NAME, "EmptySource_" + Guid.NewGuid()));
            DirectoryInfo destinationDir = new DirectoryInfo(Path.Combine(projectRoot, TEST_FOLDER_NAME, "Destination_" + Guid.NewGuid()));

            // Creamos el directorio de origen vacío
            Directory.CreateDirectory(sourceDir.FullName);

            Assert.IsTrue(sourceDir.Exists, "El directorio de origen debería existir.");
            Assert.IsFalse(destinationDir.Exists, "El directorio de destino no debería existir.");

            CopyDirectoryPlanParams input = new CopyDirectoryPlanParams
            (
                new CopyDirectoryPlanInputParams(
                    createDestinationDirectoryIfNotExists: true,
                    directoryToCopy: sourceDir,
                    destinationDirectory: destinationDir,
                    overwriteDirectoriesIfExist: false,
                    overwriteFilesIfExist: false
                )
            );

            string workflowId = await _Host.StartWorkflow(WORKFLOW_NAME, input);

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            void onCompleted(object sender, TaskPlanEventArgs e)
            {
                if (e.WorkflowInstanceId == workflowId)
                    tcs.TrySetResult(true);
            }

            _Host.TaskPlanCompleted += onCompleted;

            using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(WORKFLOW_TIMEOUT_SECONDS)))
            {
                await Task.WhenAny(tcs.Task, Task.Delay(Timeout.Infinite, cts.Token));
            }

            _Host.TaskPlanCompleted -= onCompleted;

            // Assert
            Assert.IsTrue(tcs.Task.IsCompleted, "El workflow no terminó en estado Completed.");
            destinationDir.Refresh();
            Assert.IsTrue(destinationDir.Exists, "El directorio de destino debería haberse creado.");
            Assert.AreEqual(0, Directory.GetFiles(destinationDir.FullName).Length, "El directorio de destino debería estar vacío.");

            if (CLEAN_TEST_DIRECTORY)
            {
                cleanupTestDirectory(sourceDir.FullName);
                cleanupTestDirectory(destinationDir.FullName);
            }
        }

        [TestMethod]
        [Description("Copia una estructura de directorios vacíos hasta una cierta profundidad. Verifica que todos los directorios sean creados correctamente en el destino.")]
        public async Task CopyDirectoryPlan_ShouldCopyAllDirectories()
        {
            string projectRoot = getProjectRootDirectory();
            Guid guid = Guid.NewGuid();
            string testOrigin = Path.Combine(projectRoot, TEST_FOLDER_NAME, "TestOriginDepth_" + guid);
            string testDestination = Path.Combine(projectRoot, TEST_FOLDER_NAME, "TestDestinationDepth_" + guid);

            // Crear árbol de directorios
            List<DirectoryInfo> originDirectories = createDirectoriesWithDepth(testOrigin, CREATE_FOLDERS_DEPTH);
            Assert.IsTrue(allDirectoriesExist(originDirectories), "No todos los directorios de origen fueron creados.");

            CopyDirectoryPlanParams input = new CopyDirectoryPlanParams
            (
                new CopyDirectoryPlanInputParams(
                    createDestinationDirectoryIfNotExists: true,
                    directoryToCopy: new DirectoryInfo(testOrigin),
                    destinationDirectory: new DirectoryInfo(testDestination),
                    overwriteDirectoriesIfExist: false,
                    overwriteFilesIfExist: false
                )
            );

            string workflowId = await _Host.StartWorkflow(WORKFLOW_NAME, input);

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            void onCompleted(object sender, TaskPlanEventArgs e)
            {
                if (e.WorkflowInstanceId == workflowId)
                    tcs.TrySetResult(true);
            }

            _Host.TaskPlanCompleted += onCompleted;

            using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(WORKFLOW_TIMEOUT_SECONDS)))
            {
                await Task.WhenAny(tcs.Task, Task.Delay(Timeout.Infinite, cts.Token));
            }

            _Host.TaskPlanCompleted -= onCompleted;

            // Verificar que todos los directorios fueron copiados
            Assert.IsTrue(tcs.Task.IsCompleted, "El workflow no terminó en estado Completed.");

            // Reconstruimos la lista de destino equivalente
            List<DirectoryInfo> destinationDirectories = originDirectories
                .Select(d => new DirectoryInfo(d.FullName.Replace(testOrigin, testDestination)))
                .ToList();

            Assert.IsTrue(allDirectoriesExist(destinationDirectories), "No todos los directorios fueron copiados al destino.");

            if (CLEAN_TEST_DIRECTORY)
            {
                cleanupTestDirectory(testOrigin);
                cleanupTestDirectory(testDestination);
            }
        }

        [TestMethod]
        [Description("Copia una estructura de directorios con archivos en todas las profundidades. Verifica que tanto los directorios como los archivos se copien completamente y sin errores.")]
        public async Task CopyDirectoryPlan_ShouldCopyAll()
        {
            string projectRoot = getProjectRootDirectory();
            Guid guid = Guid.NewGuid();
            string testOrigin = Path.Combine(projectRoot, TEST_FOLDER_NAME, "TestOriginDepth_" + guid);
            string testDestination = Path.Combine(projectRoot, TEST_FOLDER_NAME, "TestDestinationDepth_" + guid);

            // Crear árbol de directorios con archivos
            List<DirectoryInfo> originDirectories = createDirectoriesWithDepthAndFiles(testOrigin, CREATE_FOLDERS_DEPTH);
            Assert.IsTrue(allDirectoriesExist(originDirectories), "No todos los directorios de origen fueron creados.");

            CopyDirectoryPlanParams input = new CopyDirectoryPlanParams
            (
                new CopyDirectoryPlanInputParams(
                    createDestinationDirectoryIfNotExists: true,
                    directoryToCopy: new DirectoryInfo(testOrigin),
                    destinationDirectory: new DirectoryInfo(testDestination),
                    overwriteDirectoriesIfExist: false,
                    overwriteFilesIfExist: false
                )
            );


            string workflowId = await _Host.StartWorkflow(WORKFLOW_NAME, input);

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            void onCompleted(object sender, TaskPlanEventArgs e)
            {
                if (e.WorkflowInstanceId == workflowId)
                    tcs.TrySetResult(true);
            }

            _Host.TaskPlanCompleted += onCompleted;

            using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(WORKFLOW_TIMEOUT_SECONDS)))
            {
                await Task.WhenAny(tcs.Task, Task.Delay(Timeout.Infinite, cts.Token));
            }

            _Host.TaskPlanCompleted -= onCompleted;

            // Verificar que todos los directorios fueron copiados
            Assert.IsTrue(tcs.Task.IsCompleted, "El workflow no terminó en estado Completed.");

            // Verificar directorios
            List<DirectoryInfo> destinationDirectories = originDirectories
                .Select(d => new DirectoryInfo(d.FullName.Replace(testOrigin, testDestination)))
                .ToList();

            Assert.IsTrue(allDirectoriesExist(destinationDirectories), "No todos los directorios fueron copiados al destino.");

            // Verificar que todos los archivos fueron copiados
            int totalOriginFiles = Directory.GetFiles(testOrigin, "*.*", SearchOption.AllDirectories).Length;
            int totalDestinationFiles = Directory.GetFiles(testDestination, "*.*", SearchOption.AllDirectories).Length;

            Assert.AreEqual(totalOriginFiles, totalDestinationFiles, "El número de archivos copiados no coincide con el origen.");
            Assert.IsTrue(totalDestinationFiles > 0, "Deberían haberse creado archivos en el origen.");

            if (CLEAN_TEST_DIRECTORY)
            {
                cleanupTestDirectory(testOrigin);
                cleanupTestDirectory(testDestination);
            }
        }

        #endregion TEST METHODS

        #region MÉTODOS AUXILIARES

        private static void cleanupTestDirectory(string testDirectory)
        {
            if (Directory.Exists(testDirectory))
            {
                try
                {
                    Directory.Delete(testDirectory, true);
                    Debug.WriteLine("Directorio limpiado: " + testDirectory);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error al limpiar directorio: " + ex.Message);
                }
            }
        }

        private static string getProjectRootDirectory()
        {
            string currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo projectRoot = Directory.GetParent(currentDir).Parent?.Parent;
            return projectRoot?.FullName ?? currentDir;
        }

        /// <summary>
        /// Crea un árbol de directorios hasta la profundidad especificada.
        /// </summary>
        /// <param name="rootDirectory">Directorio raíz donde iniciar la creación.</param>
        /// <param name="depth">Profundidad del árbol de directorios a crear.</param>
        /// <returns>Lista de directorios creados.</returns>
        private static List<DirectoryInfo> createDirectoriesWithDepth(string rootDirectory, int depth)
        {
            List<DirectoryInfo> createdDirectories = new List<DirectoryInfo>();
            DirectoryInfo current = Directory.CreateDirectory(rootDirectory);
            createdDirectories.Add(current);

            for (int i = 1; i <= depth; i++)
            {
                string subDirName = "Level_" + i;
                current = current.CreateSubdirectory(subDirName);
                createdDirectories.Add(current);
            }

            return createdDirectories;
        }

        /// <summary>
        /// Verifica si todos los directorios de la lista existen.
        /// </summary>
        /// <param name="directories">Lista de directorios a verificar.</param>
        /// <returns>True si todos existen, false si alguno no existe.</returns>
        private static bool allDirectoriesExist(List<DirectoryInfo> directories)
        {
            foreach (DirectoryInfo dir in directories)
            {
                dir.Refresh();
                if (!dir.Exists) return false;
            }
            return true;
        }

        /// <summary>
        /// Crea un árbol de directorios hasta la profundidad especificada con archivos.
        /// En cada nivel genera depth * currentLevel archivos con variedad de extensiones.
        /// </summary>
        /// <param name="rootDirectory">Directorio raíz donde iniciar la creación.</param>
        /// <param name="depth">Profundidad del árbol de directorios a crear.</param>
        /// <returns>Lista de directorios creados.</returns>
        private static List<DirectoryInfo> createDirectoriesWithDepthAndFiles(string rootDirectory, int depth)
        {
            List<DirectoryInfo> createdDirectories = new List<DirectoryInfo>();
            DirectoryInfo current = Directory.CreateDirectory(rootDirectory);
            createdDirectories.Add(current);

            // Extensiones variadas para los archivos
            string[] extensions = { ".txt", ".json", ".xml", ".csv", ".log", ".dat", ".cfg", ".ini", ".md", ".html" };

            // Crear archivos en el directorio raíz (nivel 0)
            createFilesInDirectory(current.FullName, depth * 1, extensions);

            for (int i = 1; i <= depth; i++)
            {
                string subDirName = "Level_" + i;
                current = current.CreateSubdirectory(subDirName);
                createdDirectories.Add(current);

                // Crear depth * i archivos en este nivel
                int fileCount = depth * i;
                createFilesInDirectory(current.FullName, fileCount, extensions);
            }

            return createdDirectories;
        }

        /// <summary>
        /// Crea una cantidad específica de archivos en un directorio con extensiones variadas.
        /// </summary>
        /// <param name="directory">Directorio donde crear los archivos.</param>
        /// <param name="fileCount">Cantidad de archivos a crear.</param>
        /// <param name="extensions">Array de extensiones a usar.</param>
        private static void createFilesInDirectory(string directory, int fileCount, string[] extensions)
        {
            for (int i = 0; i < fileCount; i++)
            {
                string extension = extensions[i % extensions.Length];
                string fileName = $"file_{i + 1}{extension}";
                string filePath = Path.Combine(directory, fileName);

                // Contenido variado según la extensión
                string content = generateFileContent(extension, i);
                File.WriteAllText(filePath, content);
            }
        }

        /// <summary>
        /// Genera contenido apropiado según la extensión del archivo.
        /// </summary>
        private static string generateFileContent(string extension, int index)
        {
            switch (extension)
            {
                case ".txt":
                    return $"Archivo de texto número {index}\nContenido de prueba.";
                case ".json":
                    return $"{{\"id\": {index}, \"name\": \"test_{index}\", \"active\": true}}";
                case ".xml":
                    return $"<?xml version=\"1.0\"?>\n<root><id>{index}</id><name>test_{index}</name></root>";
                case ".csv":
                    return $"id,name,value\n{index},test_{index},{index * 10}";
                case ".log":
                    return $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] LOG Entry {index}: Test log message";
                case ".md":
                    return $"# Documento {index}\n\nEste es un archivo markdown de prueba.";
                case ".html":
                    return $"<!DOCTYPE html>\n<html><body><h1>Test {index}</h1></body></html>";
                default:
                    return $"Test data for file {index}";
            }
        }

        #endregion MÉTODOS AUXILIARES

    }
}
