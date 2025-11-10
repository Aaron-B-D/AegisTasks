using AegisTasks.Core;
using AegisTasks.Core.Common;
using AegisTasks.Core.TaskAction;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AegisTasks.TasksLibrary.TaskAction
{
	/// <summary>
	/// Acción responsable de escribir un contenido genérico en un archivo
	/// </summary>
	public class WriteInFileTaskAction : TaskActionBase<WriteInFileTaskActionInputParams, object>
	{
		#region PROPERTIES

		#region CONSTANTS

		#region PRIVATE CONSTANTS
		#endregion PRIVATE CONSTANTS

		#region PUBLIC CONSTANTS
		#endregion PUBLIC CONSTANTS

		#endregion CONSTANTS

		#region STATIC PROPERTIES

		#region STATIC PRIVATE PROPERTIES
		#endregion STATIC PRIVATE PROPERTIES

		#region STATIC PUBLIC PROPERTIES

		public readonly static string CALL_NAME = nameof(WriteInFileTaskAction);
		public readonly static string NAME = "Write in file action";
		public readonly static string DESCRIPTION = "Acción de tarea responsable de escribir un contenido en un archivo";

		#endregion STATIC PUBLIC PROPERTIES

		#endregion STATIC PROPERTIES

		#region PRIVATE PROPERTIES
		#endregion PRIVATE PROPERTIES

		#region PUBLIC PROPERTIES
		#endregion PUBLIC PROPERTIES

		#endregion PROPERTIES

		#region METHODS

		#region CONSTRUCTOR
		public WriteInFileTaskAction()
			: base(CALL_NAME, Constants.TASK_CATEGORY_FILES, TaskActionExecution.Sync, NAME, DESCRIPTION)
		{ }
		#endregion CONSTRUCTOR

		#region STATIC METHODS

		#region STATIC PRIVATE METHODS
		#endregion STATIC PRIVATE METHODS
		#region STATIC PUBLIC METHODS
		#endregion STATIC PUBLIC METHODS

		#endregion STATIC METHODS

		#region PUBLIC METHODS

		public override object Clone()
		{
            WriteInFileTaskAction clone = new WriteInFileTaskAction();
			if (!(InputParams is null))
			{
				clone.InputParams = (WriteInFileTaskActionInputParams)InputParams.Clone();
			}
			return clone;
		}

		public override object GetResult()
		{
			throw new NoResultTaskActionException(this.CallName);
		}

        public override void RegisterAtServices(IServiceCollection services)
        {
            services.AddTransient<WriteInFileTaskAction>();
        }

        #endregion PUBLIC METHODS

        #region PRIVATE METHODS

        protected override void logError(string message, [CallerMemberName] string methodName = "")
		{
			logError(message, this.CallName, methodName: methodName);
		}
		protected override void logException(Exception exception, [CallerMemberName] string methodName = "")
		{
			logException(exception, this.CallName, methodName: methodName);
		}

		protected override void logInfo(string message, [CallerMemberName] string methodName = "")
		{
			logInfo(message, this.CallName, methodName: methodName);
		}

		protected override void logWarn(string message, [CallerMemberName] string methodName = "")
		{
			logWarn(message, this.CallName, methodName: methodName);
		}

		public override ExecutionResult Compensate(IStepExecutionContext context)
		{
			throw new NoCompensationTaskActionException(this.CallName);
		}

		protected override Task<ExecutionResult> internalRun(IStepExecutionContext context)
		{
			if(!InputParams.FilePath.Exists)
			{
				throw new FileNotFoundException($"El archivo {InputParams.FilePath.Name} no existe");
			}

            if (InputParams.Content is string textContent)
            {
                using (StreamWriter writer = new StreamWriter(InputParams.FilePath.FullName, InputParams.Append))
                {
                    writer.Write(textContent);
                }
            }
            else if (InputParams.Content is byte[] bytesContent)
            {
                using (FileStream stream = new FileStream(InputParams.FilePath.FullName, InputParams.Append ? FileMode.Append : FileMode.Create))
                {
                    stream.Write(bytesContent, 0, bytesContent.Length);
                }
            }
            else if (InputParams.Content is Stream inputStream)
            {
                using (var stream = new FileStream(InputParams.FilePath.FullName, InputParams.Append ? FileMode.Append : FileMode.Create))
                {
                    inputStream.CopyTo(stream);
                }
            }
            else
            {
                using (var writer = new StreamWriter(InputParams.FilePath.FullName, InputParams.Append))
                {
                    writer.Write(InputParams.Content.ToString());
                }
            }

            return Task.FromResult(ExecutionResult.Next());
        }

        protected override bool areInputParamsValid()
		{
			if (InputParams is null ||InputParams.FilePath is null || InputParams.Content is null)
			{
				return false;
			}
			else
			{
				return true;
			}

		}

		#endregion PRIVATE METHODS

		#endregion METHODS
	}
}
