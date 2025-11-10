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
    /// Acción responsable de comprobar si un archivo se puede leer
    /// </summary>
    public class IsFileReadableTaskAction: TaskActionBase<IsFileReadableTaskActionInputParams, bool>
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
        public readonly static string CALL_NAME = nameof(IsFileReadableTaskAction);
        public readonly static string NAME = "Is file readable action";
        public readonly static string DESCRIPTION = "Acción de tarea responsable de comprobar si un archivo se puede leer";
        #endregion STATIC PUBLIC PROPERTIES

        #endregion STATIC PROPERTIES

        #region PRIVATE PROPERTIES
        #endregion PRIVATE PROPERTIES

        #region PUBLIC PROPERTIES
        #endregion PUBLIC PROPERTIES

        #endregion PROPERTIES



        #region METHODS

        #region CONSTRUCTOR
        public IsFileReadableTaskAction()
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
            IsFileReadableTaskAction clone = new IsFileReadableTaskAction();
            if (!(InputParams is null))
            {
                clone.InputParams = (IsFileReadableTaskActionInputParams)InputParams.Clone();
            }
            return clone;
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
            services.AddTransient<IsFileWritableTaskAction>();
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
            string fileFullName = InputParams.FilePath.FullName;
            if (!InputParams.FilePath.Exists)
            {
                throw new FileNotFoundException($"El archivo {fileFullName} no existe");
            }

            try
            {
                using (FileStream stream = InputParams.FilePath.Open(FileMode.Open, FileAccess.Read))
                {
                    _OutputParams = true;
                }
            }
            catch (UnauthorizedAccessException)
            {
                _OutputParams = false;
            }

            return Task.FromResult(ExecutionResult.Next());
        }

        protected override bool areInputParamsValid()
        {
            if (InputParams is null || InputParams.FilePath is null)
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
