using AegisTasks.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WorkflowCore.Interface;
using AegisTasks.TasksLibrary.TaskAction;
using AegisTasks.Core.Common;
using WorkflowCore.Primitives;
using System.IO;
using AegisTasks.Core.TaskPlan;
using WorkflowCore.Models;
using WorkflowCore.Services.ErrorHandlers;

namespace AegisTasks.TasksLibrary.TaskPlan
{
    /// <summary>
    /// Un plan de acción que escribe en un archivo dado el contenido deseado
    /// </summary>
	public class WriteInFilePlan : TaskPlanBase<WriteInFilePlanParams>
	{
		#region PROPERTIES

		#region CONSTANTS

		#region PRIVATE CONSTANTS

		private const int NUM_RETRIES = 3;
		private const int RETRY_INTERVAL_MS = 1000;

        private const string NAME_ES = "Plan de escritura en archivo";
        private const string NAME_GL = "Plan de escritura en archivo";
        private const string NAME_EN = "Write in file plan";
        
        private const string DESCRIPTION_ES = "Plan de acción que escribe en un archivo dado el contenido deseado";
        private const string DESCRIPTION_GL = "Plan de acción que escribe en un archivo dado el contenido deseado";
        private const string DESCRIPTION_EN = "Action plan that writes desired content to a given file";

        #endregion PRIVATE CONSTANTS
        #region PUBLIC CONSTANTS
        #endregion PUBLIC CONSTANTS

        #endregion CONSTANTS

        #region STATIC PROPERTIES

		public readonly static string CALL_NAME = nameof(WriteInFilePlan);
		public readonly static int VERSION = 1;

		#endregion STATIC PROPERTIES

		#endregion PROPERTIES

		#region METHODS

		#region PUBLIC METHODS

		public WriteInFilePlan() : base(Constants.TASK_CATEGORY_FILES, VERSION, CALL_NAME,
                NAME_ES, DESCRIPTION_ES,
                NAME_GL, DESCRIPTION_GL,
                NAME_EN, DESCRIPTION_EN)
        { }

		public override void Build(IWorkflowBuilder<WriteInFilePlanParams> builder)
		{
            builder
                 // 1. Gestionar la existencia del directorio (verificar + crear si procede)
                 .If(data => data.CreateDirectoryIfNotExists)
                     .Do(handleDir => handleDir
                         .StartWith<DirectoryExistTaskAction>()
                             .Input(step => step.InputParams, data => new DirectoryExistTaskActionInputParams(data.FilePath.Directory))
                             .Output(data => data.DirectoryExist, step => step.GetResult())
                             .Id(generateUniqueId("DirectoryExistStep"))
                             .OnError(WorkflowErrorHandling.Terminate)
                         .If(data => !data.DirectoryExist)
                             .Do(create => create
                                 .StartWith<CreateDirectoryTaskAction>()
                                     .Input(step => step.InputParams, data => new CreateDirectoryTaskActionInputParams(data.FilePath.Directory, false))
                                     .Id(generateUniqueId("CreateDirectoryStep"))
                                 .OnError(WorkflowErrorHandling.Terminate)
                             )
                     )
                 .If(data => !data.CreateDirectoryIfNotExists)
                     .Do(checkDir => checkDir
                         .StartWith<DirectoryExistTaskAction>()
                             .Input(step => step.InputParams, data => new DirectoryExistTaskActionInputParams(data.FilePath.Directory))
                             .Output(data => data.DirectoryExist, step => step.GetResult())
                             .Id(generateUniqueId("DirectoryExistCheckStep"))
                             .OnError(WorkflowErrorHandling.Terminate)
                         .If(data => !data.DirectoryExist)
                             .Do(error => error
                                 .StartWith(context =>
                                 {
                                     throw new InvalidOperationException("El directorio no existe y se ha establecido no crearlo.");
                                 })
                                 .OnError(WorkflowErrorHandling.Terminate)
                                 .Id(generateUniqueId("DirectoryNotCreatedError"))

                             )
                            .OnError(WorkflowErrorHandling.Terminate)

                     )

                 // 2. Gestionar la existencia del archivo (verificar + crear si procede)
                 .If(data => data.CreateFileIfNotExists)
                     .Do(handleFile => handleFile
                         .StartWith<FileExistTaskAction>()
                             .Input(step => step.InputParams, data => new FileExistTaskActionInputParams(data.FilePath))
                             .Output(data => data.FileExist, step => step.GetResult())
                             .Id(generateUniqueId("FileExistStep"))
                             .OnError(WorkflowErrorHandling.Terminate)
                         .If(data => !data.FileExist)
                             .Do(create => create
                                 .StartWith<CreateFileTaskAction>()
                                     .Input(step => step.InputParams, data => new CreateFileTaskActionInputParams(
                                         Path.GetFileNameWithoutExtension(data.FilePath.Name),
                                         data.FilePath.Extension,
                                         data.FilePath.Directory.FullName,
                                         false))
                                     .Id(generateUniqueId("CreateFileStep"))
                                     .OnError(WorkflowErrorHandling.Terminate)
                             )
                     )
                 .If(data => !data.CreateFileIfNotExists)
                     .Do(checkFile => checkFile
                         .StartWith<FileExistTaskAction>()
                             .Input(step => step.InputParams, data => new FileExistTaskActionInputParams(data.FilePath))
                             .Output(data => data.FileExist, step => step.GetResult())
                             .Id(generateUniqueId("FileExistCheckStep"))
                             .OnError(WorkflowErrorHandling.Terminate)
                         .If(data => !data.FileExist)
                             .Do(error => error
                                 .StartWith(context =>
                                 {
                                     throw new InvalidOperationException("El archivo no existe y se ha especificado no crearlo.");
                                 })
                                 .Id(generateUniqueId("FileNotCreatedError"))
                                 .OnError(WorkflowErrorHandling.Terminate)
                             )
                     )

                 // 3. Verificar si el archivo es escribible
                 .Then<IsFileWritableTaskAction>()
                     .Input(step => step.InputParams, data => new IsFileWritableTaskActionInputParams(data.FilePath, NUM_RETRIES, RETRY_INTERVAL_MS))
                     .Output(data => data.FileIsWritable, step => step.GetResult())
                     .Id(generateUniqueId("IsFileWritableStep"))
                     .OnError(WorkflowErrorHandling.Terminate)

                 // 4. Si NO es escribible, lanzar error
                 .If(data => !data.FileIsWritable)
                     .Do(then => then
                         .StartWith(context =>
                         {
                             throw new UnauthorizedAccessException("El archivo no es escribible");
                         })
                         .Id(generateUniqueId("FileNotWritableError"))
                         .OnError(WorkflowErrorHandling.Terminate)
                     )

                 // 5. Escribir en el archivo
                 .Then<WriteInFileTaskAction>()
                     .Input(step => step.InputParams, data => new WriteInFileTaskActionInputParams(data.FilePath, data.Content, data.AppendContent))
                     .Id(generateUniqueId("WriteInFileStep"))
                     .OnError(WorkflowErrorHandling.Terminate)

                 // 6. Log final
                 .Then(context =>
                 {
                     return ExecutionResult.Next();
                 })
                 .Id(generateUniqueId("FileWrittenLog"));
        }

		public override object Clone()
		{
			return new WriteInFilePlan();
		}

        public override void RegisterAtHost(IWorkflowHost host)
		{
			host.RegisterWorkflow<WriteInFilePlan, WriteInFilePlanParams>();
		}

		#endregion PUBLIC METHODS

		#region PRIVATE METHODS

		#endregion PRIVATE METHODS

		#endregion METHODS
	}
}