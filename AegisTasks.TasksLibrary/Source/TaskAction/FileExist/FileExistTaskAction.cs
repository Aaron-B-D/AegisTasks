using AegisTasks.Core;
using AegisTasks.Core.Common;
using AegisTasks.Core.TaskAction;
using AegisTasks.TasksLibrary;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AegisTasks.TasksLibrary.TaskAction
{
    /// <summary>
    /// Acción responsable de comprobar si un archivo exista en la ruta especificada
    /// </summary>
    public class FileExistTaskAction : TaskActionBase<FileExistTaskActionInputParams, bool>
    {
        #region PROPERTIES

        #region CONSTANTS

        #region PRIVATE CONSTANTS

        // Nombres en distintos idiomas
        private const string NAME_ES = "Archivo existe";
        private const string NAME_GL = "Arquivo existe";
        private const string NAME_EN = "File exists";

        // Descripciones en distintos idiomas
        private const string DESCRIPTION_ES = "Comprueba si un archivo existe en la ruta especificada";
        private const string DESCRIPTION_GL = "Comprueba se un arquivo existe na ruta especificada";
        private const string DESCRIPTION_EN = "Checks if a file exists at the specified path";

        #endregion PRIVATE CONSTANTS
        #region PUBLIC CONSTANTS
        #endregion PUBLIC CONSTANTS

        #endregion CONSTANTS

        #region STATIC PROPERTIES
        #region STATIC PRIVATE PROPERTIES
        #endregion STATIC PRIVATE PROPERTIES
        #region STATIC PUBLIC PROPERTIES
        public readonly static string CALL_NAME = nameof(FileExistTaskAction);

        //#region INPUT PARAMS

        //#endregion INPUT PARAMS

        //#region OUTPUT PARAMS

        //#endregion INPUT PARAMS


        #endregion STATIC PUBLIC PROPERTIES
        #endregion STATIC PROPERTIES

        #region PRIVATE PROPERTIES
        #endregion PRIVATE PROPERTIES

        #region PUBLIC PROPERTIES



        #endregion PUBLIC PROPERTIES

        #endregion PROPERTIES



        #region METHODS

        #region CONSTRUCTOR

        public FileExistTaskAction()
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
            FileExistTaskAction clone = new FileExistTaskAction();

            if (!(InputParams is null))
            {
                clone.InputParams = (FileExistTaskActionInputParams)this.InputParams.Clone();
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
            services.AddTransient<FileExistTaskAction>();
        }

        public override ExecutionResult Compensate(IStepExecutionContext context)
        {
            throw new NoCompensationTaskActionException(this.CallName);
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
            _OutputParams = File.Exists(InputParams.FilePath.FullName);

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
