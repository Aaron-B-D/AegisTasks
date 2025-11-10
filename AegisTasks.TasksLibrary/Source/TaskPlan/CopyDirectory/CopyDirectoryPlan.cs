using AegisTasks.Core.Common;
using AegisTasks.Core.TaskPlan;
using AegisTasks.TasksLibrary.Source.TaskAction.IsDirectoryEmpty;
using AegisTasks.TasksLibrary.TaskAction;
using AegisTasks.TasksLibrary.TaskPlan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Primitives;

namespace AegisTasks.TasksLibrary.TaskPlan
{
	/// <summary>
	/// Un plan de acción que copia un directorio en un destino dado
	/// </summary>
	public class CopyDirectoryPlan : TaskPlanBase<CopyDirectoryPlanParams>
	{

		#region STATIC PRIVATE PROPERTIES

		private readonly static string NAME = "Copy directory";
		private readonly static string DESCRIPTION = "Un plan de acción que copia un directorio en un destino dado";
		private readonly static int NUM_COPY_FILE_RETRIES = 3;
        private readonly static int COPY_FILE_RETRY_INTERVAL_MS = 1000;


        #endregion STATIC PRIVATE PROPERTIES


        #region STATIC PUBLIC PROPERTIES

        public readonly static string CALL_NAME = nameof(CopyDirectoryPlan);
		public readonly static int VERSION = 1;

		#endregion STATIC PUBLIC PROPERTIES

		#region CONSTRUCTOR

		public CopyDirectoryPlan() : base(Constants.TASK_CATEGORY_FILES, VERSION, CALL_NAME, NAME, DESCRIPTION)
		{ }

		#endregion CONSTRUCTOR


		#region PUBLIC METHODS

		public override void Build(IWorkflowBuilder<CopyDirectoryPlanParams> builder)
		{
			//7a. Nada que copiar, así que terminamos con el directorio vacío
			IStepBuilder<CopyDirectoryPlanParams, InlineStepBody> branchEmpty = builder.CreateBranch()
				.StartWith(context =>
				{
					Logger.LogWarn("El directorio a copiar está vacío. Se considerará correctamente copiado");
					return ExecutionResult.Next();
				})
				.Id(generateUniqueId($"DirectoryToCopyEmpty"))
				.OnError(WorkflowErrorHandling.Terminate);

			//7b. Hay información que copiar, debemos proceder con cuidado
			IStepBuilder<CopyDirectoryPlanParams, Foreach> branchHasData = builder.CreateBranch()

				//8. Escanear el directorio a copiar para tener todo su contenido listado
				.StartWith<ScanDirectoryTaskAction>()
					.Input(step => step.InputParams, data => new ScanDirectoryTaskActionInputParams(data.DirectoryToCopy))
					.Output(data => data.Directories, step => step.GetResult().Directories)
					.Output(data => data.Files, step => step.GetResult().Files)
					.Id(generateUniqueId($"DirectoryToCopyHasData"))
					.OnError(WorkflowErrorHandling.Terminate)

				//9. Replicar la estructura de directorios en el destino
                .ForEach(data => data.Directories)
					.Do(createDirectories => createDirectories
						.StartWith<CreateDirectoryTaskAction>()
						.Input(
							step => step.InputParams,
							(data, context) => new CreateDirectoryTaskActionInputParams(
								new DirectoryInfo(
									((DirectoryInfo)context.Item).FullName.Replace(
										data.DirectoryToCopy.FullName,
										data.DestinationDirectory.FullName
									)
								),
								false
							)
						)
						.Id(generateUniqueId("CreateSubDirectory"))
						.OnError(WorkflowErrorHandling.Terminate)
					)

				//10. Copiar todos los archivos
				.Then(context => ExecutionResult.Next())
				.ForEach(data => data.Files)
					.Do(copyFiles => copyFiles
						.StartWith<CopyFileTaskAction>()
						.Input(
							step => step.InputParams,
							(data, context) => new CopyFileTaskActionInputParams(
								(FileInfo)context.Item,
								new DirectoryInfo(
									Path.GetDirectoryName(
										((FileInfo)context.Item).FullName.Replace(
											data.DirectoryToCopy.FullName,
											data.DestinationDirectory.FullName
										)
									)
								),
								false,
                                NUM_COPY_FILE_RETRIES,
                                COPY_FILE_RETRY_INTERVAL_MS
                            )
						)
						.Id(generateUniqueId("CopyFile"))
                        .OnError(WorkflowErrorHandling.Terminate)
                        .CompensateWith<CopyFileTaskAction>()
						.OnError(WorkflowErrorHandling.Terminate)
					);


            builder
				// 1. Comprobar si el directorio de destino existe o no
				.StartWith<DirectoryExistTaskAction>()
					.Input(step => step.InputParams, data => new DirectoryExistTaskActionInputParams(data.DestinationDirectory))
					.Output(data => data.DestinationDirectoryExist, step => step.GetResult())
					.Id(generateUniqueId("CheckDestinationDirectoryExist"))
					.OnError(WorkflowErrorHandling.Terminate)

				// 2. Si el directorio de destino no existe y no se puede crear, terminamos el proceso
				.If(data => !data.DestinationDirectoryExist && !data.CreateDestinationDirectoryIfNotExists)
					.Do(cannotCreateDestination => cannotCreateDestination
						.StartWith(context =>
						{
							throw new InvalidOperationException("El directorio de destino no existe y se ha especificado no crearlo");
						})
						.Id(generateUniqueId("CannotCreateDestinationDirectory"))
						.OnError(WorkflowErrorHandling.Terminate)
					)

				// 3. Comprobar si existe el directorio a copiar
				.Then<DirectoryExistTaskAction>()
					 .Input(step => step.InputParams, data => new DirectoryExistTaskActionInputParams(data.DirectoryToCopy))
					 .Output(data => data.DirectoryToCopyExist, step => step.GetResult())
					 .Id(generateUniqueId("CheckDirectoryToCopyExist"))
					 .OnError(WorkflowErrorHandling.Terminate)


				// 4. Si el directorio a copiar no existe, terminamos el workflow
				.If(data => !data.DirectoryToCopyExist)
					.Do(directoryToCopyNotExist => directoryToCopyNotExist
						.StartWith(context =>
						{
							throw new InvalidOperationException("El directorio a copiar no existe");
						})
						.Id(generateUniqueId("DirectoryToCopyNotExist"))
						.OnError(WorkflowErrorHandling.Terminate)
					)

				// 5. Si el directorio de destino no existe y se permite crear, lo creamos antes de empezar
				.If(data => !data.DestinationDirectoryExist && data.CreateDestinationDirectoryIfNotExists)
					.Do(createDestinationDirectory => createDestinationDirectory
						.StartWith<CreateDirectoryTaskAction>()
							.Input(step => step.InputParams, data => new CreateDirectoryTaskActionInputParams(data.DestinationDirectory, false))
						.Id(generateUniqueId("CreateDestinationDirectory"))
						.OnError(WorkflowErrorHandling.Terminate)
					)

				// 6. Comprobamos si el directorio a copiar está vacío. De ser el caso, podemos concluir el flujo
				.Then<IsDirectoryEmptyTaskAction>()
					.Input(step => step.InputParams, data => new IsDirectoryEmptyTaskActionInputParams(data.DirectoryToCopy))
					.Output(data => data.DirectoryToCopyIsEmpty, step => step.GetResult())
					.Id(generateUniqueId("CheckIfDirectoryToCopyIsEmpty"))
				.Decide(data => data.DirectoryToCopyIsEmpty)
					// Directorio vacío. Nada que copiar
					.Branch(true, branchEmpty)

					// Directorio a copiar tiene datos. Empezamos el proceso de copiado
					.Branch(false, branchHasData);

		}

		public override object Clone()
		{
			throw new NotImplementedException();
		}

		public override void RegisterAtHost(IWorkflowHost host)
		{
			host.RegisterWorkflow<CopyDirectoryPlan, CopyDirectoryPlanParams>();
		}

		#endregion PUBLIC METHODS

	}
}
