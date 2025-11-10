using AegisTasks.Core.Common;
using AegisTasks.Core.TaskAction;
using AegisTasks.TasksLibrary.TaskAction;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AegisTasks.TasksLibrary.Source.TaskAction.IsDirectoryEmpty
{
    /// <summary>
    /// Acción responsable de comprobar si un directorio está vacío o no
    /// </summary>
    public class IsDirectoryEmptyTaskAction : TaskActionBase<IsDirectoryEmptyTaskActionInputParams, bool>
    {

        #region STATIC PUBLIC PROPERTIES

        public readonly static string CALL_NAME = nameof(IsDirectoryEmptyTaskAction);
        public readonly static string NAME = "Is directory empty action";
        public readonly static string DESCRIPTION = "Acción de tarea responsable de compro";

        #endregion STATIC PUBLIC PROPERTIES


        #region CONSTRUCTOR

        public IsDirectoryEmptyTaskAction()
            : base(CALL_NAME, Constants.TASK_CATEGORY_FILES, TaskActionExecution.Sync, NAME, DESCRIPTION)
        { }


        #endregion CONSTRUCTOR


        #region PUBLIC METHODS

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override ExecutionResult Compensate(IStepExecutionContext context)
        {
            throw new NoCompensationTaskActionException(this.Id);
        }

        public override bool GetResult()
        {
            if (NumExecutions == 0)
            {
                throw new NotExecutedTaskActionException(this.Id);
            }
            else
            {
                return _OutputParams;
            }
        }

        public override void RegisterAtServices(IServiceCollection services)
        {
            services.AddTransient<IsDirectoryEmptyTaskAction>();
        }

        #endregion PUBLIC METHODS


        #region PRIVATE METHODS

        protected override bool areInputParamsValid()
        {
            return !(InputParams is null) && !(InputParams.DirectoryToCheck is null);
        }

        protected override Task<ExecutionResult> internalRun(IStepExecutionContext context)
        {
            DirectoryInfo dirInfo = InputParams.DirectoryToCheck;

            // Asignamos el resultado al parámetro de salida
            _OutputParams = !dirInfo.EnumerateFileSystemInfos().Any();

            return Task.FromResult(ExecutionResult.Next());
        }

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

        #endregion PRIVATE METHODS

    }
}
