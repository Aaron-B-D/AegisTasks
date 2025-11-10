using AegisTasks.Core.Events;
using AegisTasks.TasksLibrary;
using AegisTasks.TasksLibrary.TaskPlan;
using AegisTasks.TasksLibrary.WorkflowHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AegisTasks.UnitTests.PlansTests
{
	[TestClass]
	public class WriteFilePlanTests
	{
		#region CONFIGURACIÓN GLOBAL

		private static PreconfiguredAegisWorkflowsManager  _Host;

		// Parámetros de test
		private const int WORKFLOW_TIMEOUT_SECONDS = 30;
		private const int EXECUTION_COUNT = 10;
		private const int MULTIPLE_FILE_COUNT = 5;

        // Carpetas y archivos
        private static readonly string TEST_FOLDER_NAME = Path.Combine(
            Properties.Settings.Default.TestRootFolder,
            "WriteInFilePlanTests"
        ); 
		private const string FILE_PREFIX = "archivo_test_";
		private const string FILE_EXTENSION = ".txt";

		// Límites y comportamiento
		private const double TOLERATED_DIFFERENCE_MS = 800;
		private const bool CLEAN_TEST_DIRECTORY = true;

		// Cantidad de partes para las pruebas de concatenación
		private const int CONCATENATION_PART_COUNT = 5;

		private readonly string WORKFLOW_NAME = WriteInFilePlan.CALL_NAME;

		#endregion CONFIGURACIÓN GLOBAL


		#region CONFIGURACIÓN DE CLASE

		[ClassInitialize]
		public static void Init(TestContext context)
		{
			_Host = new PreconfiguredAegisWorkflowsManager ();

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
        [Description("Ejecuta una tarea de escritura sin permitir la creación del directorio. Debe fallar al no existir el directorio y no debe crearse nada.")]
        public async Task ExecuteWriteTask_ShouldFail_WhenDirectoryDoesNotExistAndCreationNotAllowed()
		{
			// Arrange
			string projectRoot = getProjectRootDirectory();
			string testDirectory = Path.Combine(projectRoot, TEST_FOLDER_NAME, "NonExistent_" + Guid.NewGuid().ToString());
			string filePath = Path.Combine(testDirectory, FILE_PREFIX + "no_dir" + FILE_EXTENSION);

			Assert.IsFalse(Directory.Exists(testDirectory), "El directorio de prueba no debería existir antes del test.");

            WriteInFilePlanParams input = new WriteInFilePlanParams(
				new WriteInFilePlanInputParams(
					new FileInfo(filePath),
					"Contenido que no debería escribirse",
					createDirectoryIfNotExists: false,
					createFileIfNotExists: true,
					appendContent: false
				)
			);

			try
			{
				string workflowId = await _Host.StartWorkflow(WORKFLOW_NAME, input);

				// Esperamos resultado — debería lanzar excepción o terminar abruptamente
				using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(WORKFLOW_TIMEOUT_SECONDS)))
				{
                    TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
                    _Host.TaskPlanTerminated += (sender, e) =>
                    {
                        if (e.WorkflowInstanceId == workflowId)
                        {
                            tcs.TrySetResult(true);
                        }
                    };

                    await Task.WhenAny(tcs.Task, Task.Delay(Timeout.Infinite, cts.Token));
				}
			}
			catch (Exception)
			{
				//No reaccionamos
			}

			// Assert
			Assert.IsTrue(!File.Exists(filePath), $"El archivo {filePath} no debería existir");
			Assert.IsFalse(Directory.Exists(testDirectory), "El directorio no debería haberse creado.");
		}

		[TestMethod]
        [Description("Ejecuta una tarea de escritura permitiendo crear el directorio pero no el archivo. Debe crear el directorio vacío y fallar al no poder crear el archivo.")]
        public async Task ExecuteWriteTask_ShouldFail_WhenFileCreationNotAllowedButDirectoryAllowed()
		{
			// Arrange
			string projectRoot = getProjectRootDirectory();
			string testDirectory = Path.Combine(projectRoot, TEST_FOLDER_NAME, "EmptyDir_" + Guid.NewGuid().ToString());
			string filePath = Path.Combine(testDirectory, FILE_PREFIX + "no_file" + FILE_EXTENSION);

			Assert.IsFalse(Directory.Exists(testDirectory), "El directorio no debería existir antes del test.");

            WriteInFilePlanParams input = new WriteInFilePlanParams(
				new WriteInFilePlanInputParams(
					new FileInfo(filePath),
					"Contenido que no debería escribirse",
					createDirectoryIfNotExists: true,
					createFileIfNotExists: false,
					appendContent: false
				)
			);

			try
			{
				string workflowId = await _Host.StartWorkflow(WORKFLOW_NAME, input);

				// Esperamos resultado — debe terminar abruptamente
				using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(WORKFLOW_TIMEOUT_SECONDS)))
				{
                    TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
                    _Host.TaskPlanTerminated += (sender, e) =>
                    {
                        if (e.WorkflowInstanceId == workflowId)
                        {
                            tcs.TrySetResult(true);
                        }
                    };

                    await Task.WhenAny(tcs.Task, Task.Delay(Timeout.Infinite, cts.Token));
				}
			}
			catch (Exception)
			{
				//No es necesario reaccionar, basta con que los asserts cumplan
			}

			// Assert
			Assert.IsTrue(Directory.Exists(testDirectory), "El directorio debería haberse creado.");
			Assert.IsTrue(!File.Exists(filePath), "El archivo no debería haberse creado.");
			Assert.AreEqual(0, Directory.GetFiles(testDirectory).Length, "El directorio debería estar vacío.");
		}

		[TestMethod]
        [Description("Ejecuta múltiples tareas de escritura en archivo de forma paralela mediante WorkflowCore y valida que los archivos fueron creados correctamente.")]
        public async Task ExecuteMultipleWriteTasks_WorkflowCore_ShouldCreateAllFiles()
		{
			DateTime testStart = DateTime.Now;
			string projectRoot = getProjectRootDirectory();
			string testDirectory = Path.Combine(projectRoot, TEST_FOLDER_NAME, Guid.NewGuid().ToString());

			try
			{
				string[] workflowIds = new string[MULTIPLE_FILE_COUNT];
				string[] filePaths = new string[MULTIPLE_FILE_COUNT];
				string[] expectedContents = new string[MULTIPLE_FILE_COUNT];

				// Creamos un diccionario para mapear workflowId a TaskCompletionSource
				Dictionary<string, TaskCompletionSource<bool>> workflowCompletionSources = new Dictionary<string, TaskCompletionSource<bool>>(MULTIPLE_FILE_COUNT);

                // Suscribimos eventos del host
                TaskPlanEventHandler onCompleted = (sender, e) =>
                {
                    if (workflowCompletionSources.ContainsKey(e.WorkflowInstanceId))
                        workflowCompletionSources[e.WorkflowInstanceId].TrySetResult(true);
                };

                TaskPlanEventHandler onTerminated = (sender, e) =>
                {
                    if (workflowCompletionSources.ContainsKey(e.WorkflowInstanceId))
                        workflowCompletionSources[e.WorkflowInstanceId].TrySetException(
                            new Exception($"Workflow {e.WorkflowInstanceId} terminado abruptamente."));
                };

                _Host.TaskPlanCompleted += onCompleted;
				_Host.TaskPlanTerminated += onTerminated;

				// Iniciamos los workflows
				for (int i = 0; i < MULTIPLE_FILE_COUNT; i++)
				{
					string filePath = Path.Combine(testDirectory, FILE_PREFIX + (i + 1) + FILE_EXTENSION);
					string expectedContent = "Contenido de prueba " + (i + 1) + " - " + DateTime.Now.ToString("O");

					filePaths[i] = filePath;
					expectedContents[i] = expectedContent;

                    WriteInFilePlanParams input = new WriteInFilePlanParams(
						new WriteInFilePlanInputParams(
							new FileInfo(filePath),
							expectedContent,
							createDirectoryIfNotExists: true,
							createFileIfNotExists: true,
							appendContent: true
						)
					);

					workflowIds[i] = await _Host.StartWorkflow(WORKFLOW_NAME, input);

					// Creamos el TaskCompletionSource para este workflowId
					workflowCompletionSources[workflowIds[i]] = new TaskCompletionSource<bool>();

					Debug.WriteLine("Workflow " + (i + 1) + " iniciado con Id: " + workflowIds[i]);
				}

				// Esperamos a que todos los workflows se completen usando eventos
				IEnumerable<Task<bool>> waitTasks = workflowCompletionSources.Values.Select(tcs => tcs.Task);
				await Task.WhenAll(waitTasks);

				// Validamos que los archivos fueron creados correctamente
				for (int i = 0; i < MULTIPLE_FILE_COUNT; i++)
				{
					verifyFileWasCreated(filePaths[i], testStart, expectedContents[i]);
				}

				Debug.WriteLine("Todas las tareas se ejecutaron y validaron correctamente.");
			}
			finally
			{
				if (CLEAN_TEST_DIRECTORY)
					cleanupTestDirectory(testDirectory);
			}
		}

		[TestMethod]
        [Description("Ejecuta múltiples tareas de escritura en el mismo archivo de forma asincrónica garantizando la concatenación en orden de las partes escritas.")]
        public async Task ExecuteConcatenatedWriteTasks_ShouldCreateFileWithConcatenatedContent()
		{
			const int CONCAT_PART_COUNT = CONCATENATION_PART_COUNT;

			// Arrange
			string projectRoot = getProjectRootDirectory();
			string testDirectory = Path.Combine(projectRoot, TEST_FOLDER_NAME, Guid.NewGuid().ToString());
			string filePath = Path.Combine(testDirectory, FILE_PREFIX + "concatenated" + FILE_EXTENSION);
			DateTime testStart = DateTime.Now;

			string[] parts = Enumerable.Range(1, CONCAT_PART_COUNT).Select(i => $"Parte{i}").ToArray();
			string expectedContent = string.Join("", parts);

			Directory.CreateDirectory(testDirectory);

			// Diccionario para esperar cada workflow
			Dictionary<string, TaskCompletionSource<bool>> workflowCompletionSources = new Dictionary<string, TaskCompletionSource<bool>>();

            // Suscribimos eventos del host
            TaskPlanEventHandler onCompleted = (sender, e) =>
            {
                if (workflowCompletionSources.ContainsKey(e.WorkflowInstanceId))
                    workflowCompletionSources[e.WorkflowInstanceId].TrySetResult(true);
            };

            TaskPlanEventHandler onTerminated = (sender, e) =>
            {
                if (workflowCompletionSources.ContainsKey(e.WorkflowInstanceId))
                    workflowCompletionSources[e.WorkflowInstanceId].TrySetException(
                        new Exception($"Workflow {e.WorkflowInstanceId} terminado abruptamente."));
            };

            _Host.TaskPlanCompleted += onCompleted;
			_Host.TaskPlanTerminated += onTerminated;

			try
			{
				// Lanzamos los workflows en orden y esperamos secuencialmente para preservar la concatenación correcta
				foreach (string part in parts)
				{
					WriteInFilePlanParams input = new WriteInFilePlanParams(
						new WriteInFilePlanInputParams(
							new FileInfo(filePath),
							part,
							createDirectoryIfNotExists: true,
							createFileIfNotExists: true,
							appendContent: true
						)
					);

					string workflowId = await _Host.StartWorkflow(WORKFLOW_NAME, input);
                    TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
					workflowCompletionSources[workflowId] = tcs;

					// Esperamos la finalización de cada workflow antes de lanzar el siguiente
					await tcs.Task;
				}

				// Assert
				Assert.IsTrue(File.Exists(filePath), "El archivo concatenado no fue creado.");
				string content = File.ReadAllText(filePath);
				Assert.AreEqual(expectedContent, content, "El contenido concatenado no coincide con el esperado.");
				Assert.IsTrue(File.GetLastWriteTime(filePath) >= testStart, "El archivo no se modificó durante la prueba.");
			}
			finally
			{
				_Host.TaskPlanCompleted -= onCompleted;
				_Host.TaskPlanTerminated -= onTerminated;

				if (CLEAN_TEST_DIRECTORY)
					cleanupTestDirectory(testDirectory);
			}
		}

		[TestMethod]
        [Description("Ejecuta múltiples tareas de escritura asincrónica sobre el mismo archivo sin garantizar orden. Verifica que todas las partes están presentes aunque no necesariamente en el orden esperado.")]
        public async Task ExecuteConcatenatedWriteTasks_ShouldContainAllPartsRegardlessOfOrder()
		{
			const int CONCAT_PART_COUNT = CONCATENATION_PART_COUNT;

			// Arrange
			string projectRoot = getProjectRootDirectory();
			string testDirectory = Path.Combine(projectRoot, TEST_FOLDER_NAME, Guid.NewGuid().ToString());
			string filePath = Path.Combine(testDirectory, FILE_PREFIX + "unordered" + FILE_EXTENSION);
			DateTime testStart = DateTime.Now;

			string[] parts = Enumerable.Range(1, CONCAT_PART_COUNT).Select(i => $"Parte{i}").ToArray();

			Directory.CreateDirectory(testDirectory);

			Dictionary<string, TaskCompletionSource<bool>> workflowCompletionSources = new Dictionary<string, TaskCompletionSource<bool>>();

            // Suscribimos eventos del host
            TaskPlanEventHandler onCompleted = (sender, e) =>
            {
                if (workflowCompletionSources.ContainsKey(e.WorkflowInstanceId))
                    workflowCompletionSources[e.WorkflowInstanceId].TrySetResult(true);
            };

            TaskPlanEventHandler onTerminated = (sender, e) =>
            {
                if (workflowCompletionSources.ContainsKey(e.WorkflowInstanceId))
                    workflowCompletionSources[e.WorkflowInstanceId].TrySetException(
                        new Exception($"Workflow {e.WorkflowInstanceId} terminado abruptamente."));
            };

            _Host.TaskPlanCompleted += onCompleted;
			_Host.TaskPlanTerminated += onTerminated;

			try
			{
				// Lanzamos todos los workflows en paralelo (sin garantizar orden)
				foreach (string part in parts)
				{
                    WriteInFilePlanParams input = new WriteInFilePlanParams(
						new WriteInFilePlanInputParams(
							new FileInfo(filePath),
							part,
							createDirectoryIfNotExists: true,
							createFileIfNotExists: true,
							appendContent: true
						)
					);

					string workflowId = await _Host.StartWorkflow(WORKFLOW_NAME, input);
					workflowCompletionSources[workflowId] = new TaskCompletionSource<bool>();
				}

				await Task.WhenAll(workflowCompletionSources.Values.Select(tcs => tcs.Task));

				// Assert
				Assert.IsTrue(File.Exists(filePath), "El archivo concatenado no fue creado.");
				string content = File.ReadAllText(filePath);

				foreach (string part in parts)
					Assert.IsTrue(content.Contains(part), $"El contenido no contiene la parte '{part}'.");

				Assert.IsTrue(File.GetLastWriteTime(filePath) >= testStart, "El archivo no se modificó durante la prueba.");
			}
			finally
			{
				_Host.TaskPlanCompleted -= onCompleted;
				_Host.TaskPlanTerminated -= onTerminated;

				if (CLEAN_TEST_DIRECTORY)
					cleanupTestDirectory(testDirectory);
			}
		}

		[TestMethod]
        [Description("Compara el rendimiento entre la escritura directa en archivo y la escritura mediante WorkflowCore realizando múltiples ejecuciones y promediando los tiempos.")]
        public async Task CompareWritePerformance_WorkflowCore_Vs_DirectWrite()
		{
			Stopwatch swWorkflowTotal = new Stopwatch();
			Stopwatch swDirectTotal = new Stopwatch();

			string projectRoot = getProjectRootDirectory();

			// Suscripciones a eventos (usadas para esperar finalización de workflows)
			Dictionary<string, TaskCompletionSource<bool>> workflowCompletionSources =
				new Dictionary<string, TaskCompletionSource<bool>>();

            // Suscribimos eventos del host
            TaskPlanEventHandler onCompleted = (sender, e) =>
            {
                if (workflowCompletionSources.ContainsKey(e.WorkflowInstanceId))
                    workflowCompletionSources[e.WorkflowInstanceId].TrySetResult(true);
            };

            TaskPlanEventHandler onTerminated = (sender, e) =>
            {
                if (workflowCompletionSources.ContainsKey(e.WorkflowInstanceId))
                    workflowCompletionSources[e.WorkflowInstanceId].TrySetException(
                        new Exception($"Workflow {e.WorkflowInstanceId} terminado abruptamente."));
            };

            _Host.TaskPlanCompleted += onCompleted;
			_Host.TaskPlanTerminated += onTerminated;

			try
			{
				for (int i = 1; i <= EXECUTION_COUNT; i++)
				{
					Debug.WriteLine($"Ejecución {i} de {EXECUTION_COUNT}");

					string workflowTestDirectory = Path.Combine(projectRoot, TEST_FOLDER_NAME, "Workflow_" + Guid.NewGuid().ToString());
					string directTestDirectory = Path.Combine(projectRoot, TEST_FOLDER_NAME, "Direct_" + Guid.NewGuid().ToString());

					string workflowFile = Path.Combine(workflowTestDirectory, "workflow_write.txt");
					string directFile = Path.Combine(directTestDirectory, "direct_write.txt");

					string longContent = string.Join(Environment.NewLine,
						Enumerable.Range(0, 2_500_000).Select(x => $"Línea {x} - {Guid.NewGuid()}"));

                    // --- WorkflowCore ---
                    WriteInFilePlanParams workflowInput = new WriteInFilePlanParams(
						new WriteInFilePlanInputParams(
							new FileInfo(workflowFile),
							longContent,
							createDirectoryIfNotExists: true,
							createFileIfNotExists: true,
							appendContent: false
						)
					);

					swWorkflowTotal.Start();

					string workflowId = await _Host.StartWorkflow(WORKFLOW_NAME, workflowInput);
                    TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
					workflowCompletionSources[workflowId] = tcs;

					Debug.WriteLine($"Workflow {i} iniciado con Id: {workflowId}");

					// Esperamos a que el workflow se complete mediante evento (sin polling)
					using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(WORKFLOW_TIMEOUT_SECONDS * 3)))
					{
						Task completedTask = await Task.WhenAny(tcs.Task, Task.Delay(Timeout.Infinite, cts.Token));

						if (completedTask != tcs.Task)
							Assert.Fail($"Workflow {workflowId} no completó en el tiempo esperado.");
					}

					swWorkflowTotal.Stop();
					Debug.WriteLine($"WorkflowCore ejecución {i} completada");

					// --- Escritura directa ---
					swDirectTotal.Start();

					if (!Directory.Exists(directTestDirectory))
						Directory.CreateDirectory(directTestDirectory);

					if (!File.Exists(directFile))
						using (File.Create(directFile)) { }

					bool isWritable = false;
					try
					{
						using (FileStream fs = File.Open(directFile, FileMode.Open, FileAccess.Write))
						{
							isWritable = fs.CanWrite;
						}
					}
					catch (IOException)
					{
						isWritable = false;
					}
					catch (UnauthorizedAccessException)
					{
						isWritable = false;
					}

					if (isWritable)
					{
						File.WriteAllText(directFile, longContent);
						Debug.WriteLine($"Archivo {directFile} escrito correctamente.");
					}
					else
					{
						Assert.Fail($"El archivo {directFile} no tiene permisos de escritura.");
					}

					swDirectTotal.Stop();
					Debug.WriteLine($"Escritura directa ejecución {i} completada");

					// Validaciones
					Assert.IsTrue(File.Exists(workflowFile));
					Assert.IsTrue(File.Exists(directFile));

					long sizeWorkflow = new FileInfo(workflowFile).Length;
					long sizeDirect = new FileInfo(directFile).Length;
					Assert.AreEqual(sizeDirect, sizeWorkflow);

					if (CLEAN_TEST_DIRECTORY)
					{
						cleanupTestDirectory(workflowTestDirectory);
						cleanupTestDirectory(directTestDirectory);
					}

					Debug.WriteLine($"Ejecución {i} finalizada\n");
				}

				double totalWorkflowMS = swWorkflowTotal.Elapsed.TotalMilliseconds;
				double totalDirectMS = swDirectTotal.Elapsed.TotalMilliseconds;
				double avgWorkflow = totalWorkflowMS / EXECUTION_COUNT;
				double avgDirect = totalDirectMS / EXECUTION_COUNT;
				double avgDiff = avgWorkflow - avgDirect;

				Debug.WriteLine("Resultados finales:");
				Debug.WriteLine($"Promedio WorkflowCore: {avgWorkflow:F2} ms");
				Debug.WriteLine($"Promedio Directo: {avgDirect:F2} ms");
				Debug.WriteLine($"Overhead promedio: {avgDiff:F2} ms");

				Assert.IsTrue(avgDiff <= TOLERATED_DIFFERENCE_MS, "Overhead promedio demasiado alto.");
			}
			finally
			{
				// Limpieza de suscripciones para no generar fugas entre tests
				_Host.TaskPlanCompleted -= onCompleted;
				_Host.TaskPlanTerminated -= onTerminated;
			}
		}


		#endregion TEST METHODS


		#region MÉTODOS AUXILIARES

		private static void verifyFileWasCreated(string filePath, DateTime testStart, string expectedContent)
		{
			Assert.IsTrue(File.Exists(filePath), "El archivo no fue creado: " + filePath);
			string content = File.ReadAllText(filePath);
			Assert.AreEqual(expectedContent, content);
			Assert.IsTrue(File.GetLastWriteTime(filePath) >= testStart);
		}

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

		#endregion MÉTODOS AUXILIARES
	}
}
