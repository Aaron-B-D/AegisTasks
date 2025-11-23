using AegisTasks.Core;
using AegisTasks.Core.Common;
using AegisTasks.Core.TaskAction;
using AegisTasks.TasksLibrary;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Linq;
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

        // Nombres en distintos idiomas
        private const string NAME_ES = "Directorio existe";
        private const string NAME_GL = "Directorio existe";
        private const string NAME_EN = "Directory exists";

        // Descripciones en distintos idiomas
        private const string DESCRIPTION_ES = "Comprueba si un directorio existe en la ruta especificada";
        private const string DESCRIPTION_GL = "Comproba se un directorio existe na ruta especificada";
        private const string DESCRIPTION_EN = "Checks if a directory exists at the specified path";

        #region STATIC PROPERTIES

        #region STATIC PUBLIC PROPERTIES
        public readonly static string CALL_NAME = nameof(DirectoryExistTaskAction);

        #endregion STATIC PUBLIC PROPERTIES
        #endregion STATIC PROPERTIES

        #region PRIVATE PROPERTIES
        #endregion PRIVATE PROPERTIES

        #region PUBLIC PROPERTIES
        #endregion PUBLIC PROPERTIES

        #endregion PROPERTIES


        #region CONSTRUCTOR

        public DirectoryExistTaskAction()
            : base(CALL_NAME, Constants.TASK_CATEGORY_FILES, NAME_ES, DESCRIPTION_ES, NAME_GL, DESCRIPTION_GL, NAME_EN, DESCRIPTION_EN)
        { }

        #endregion CONSTRUCTOR

        #region METHODS


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
