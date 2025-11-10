using AegisTasks.Core;
using AegisTasks.Core.Common;
using AegisTasks.Core.TaskAction;
using AegisTasks.TasksLibrary;
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
    /// Acción responsable de comprobar si un directorio existe en la ruta especificada
    /// </summary>
    public class DirectoryExistTaskAction : TaskActionBase<DirectoryExistTaskActionInputParams, bool>
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
        public readonly static string CALL_NAME = nameof(DirectoryExistTaskAction);
        public readonly static string NAME = "Directory exists action";
        public readonly static string DESCRIPTION = "Una acción de tarea responsable de comprobar la existencia de un directorio en una ruta especificada";


        #endregion STATIC PUBLIC PROPERTIES
        #endregion STATIC PROPERTIES

        #region PRIVATE PROPERTIES
        #endregion PRIVATE PROPERTIES

        #region PUBLIC PROPERTIES
        #endregion PUBLIC PROPERTIES

        #endregion PROPERTIES



        #region METHODS

        #region CONSTRUCTOR

        public DirectoryExistTaskAction()
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
            DirectoryExistTaskAction clone = new DirectoryExistTaskAction();

            if (!(InputParams is null))
            {
                clone.InputParams = (DirectoryExistTaskActionInputParams) InputParams.Clone();
            }
            return clone;
        }

        public override bool GetResult()
        {
            if (NumExecutions == 0)
            {
                throw new NotExecutedTaskActionException(this.CallName);
            }
            else
            {
                return _OutputParams;
            }
        }

        public override ExecutionResult Compensate(IStepExecutionContext context)
        {
            throw new NoCompensationTaskActionException(this.CallName);
        }

        public override void RegisterAtServices(IServiceCollection services)
        {
            services.AddTransient<DirectoryExistTaskAction>();
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

        protected override Task<ExecutionResult> internalRun(IStepExecutionContext context)
        {
            _OutputParams = Directory.Exists(InputParams.DirectoryPath.FullName);

            return Task.FromResult(ExecutionResult.Next());
        }

        protected override bool areInputParamsValid()
        {
            if (InputParams is null ||InputParams.DirectoryPath is null)
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
