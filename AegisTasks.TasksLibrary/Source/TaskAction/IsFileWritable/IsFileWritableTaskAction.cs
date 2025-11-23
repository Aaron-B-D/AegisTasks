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
    /// Acción responsable de comprobar si un archivo se puede escribir
    /// </summary>
    public class IsFileWritableTaskAction : TaskActionBase<IsFileWritableTaskActionInputParams, bool>
    {
        #region PROPERTIES

        #region CONSTANTS

        #region PRIVATE CONSTANTS

        // Nombres en distintos idiomas
        private const string NAME_ES = "Archivo escribible";
        private const string NAME_GL = "Arquivo escribible";
        private const string NAME_EN = "Is file writable";

        // Descripciones en distintos idiomas
        private const string DESCRIPTION_ES = "Comprueba si un archivo se puede escribir en la ruta especificada";
        private const string DESCRIPTION_GL = "Comproba se un arquivo se pode escribir na ruta especificada";
        private const string DESCRIPTION_EN = "Checks if a file can be written at the specified path";

        #endregion PRIVATE CONSTANTS

        #region PUBLIC CONSTANTS
        #endregion PUBLIC CONSTANTS

        #endregion CONSTANTS

        #region STATIC PROPERTIES

        #region STATIC PRIVATE PROPERTIES
        #endregion STATIC PRIVATE PROPERTIES

        #region STATIC PUBLIC PROPERTIES
        public readonly static string CALL_NAME = nameof(IsFileWritableTaskAction);
        public readonly static string NAME = "Is file writable action";
        public readonly static string DESCRIPTION = "Acción de tarea responsable de comprobar si un archivo se puede escribir";
        #endregion STATIC PUBLIC PROPERTIES

        #endregion STATIC PROPERTIES

        #region PRIVATE PROPERTIES
        #endregion PRIVATE PROPERTIES

        #region PUBLIC PROPERTIES
        #endregion PUBLIC PROPERTIES

        #endregion PROPERTIES

        #region METHODS

        #region CONSTRUCTOR
        public IsFileWritableTaskAction()
            : base(CALL_NAME, Constants.TASK_CATEGORY_FILES,
                NAME_ES,
                DESCRIPTION_ES,
                NAME_GL,
                DESCRIPTION_GL,
                NAME_EN,
                DESCRIPTION_EN)
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
            IsFileWritableTaskAction clone = new IsFileWritableTaskAction();
            if (!(InputParams is null))
            {
                clone.InputParams = (IsFileWritableTaskActionInputParams)InputParams.Clone();
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
                throw new FileNotFoundException($"El archivo no existe: {fileFullName}");
            }

            try
            {
                using (FileStream stream = InputParams.FilePath.Open(FileMode.Open, FileAccess.Write))
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
