using AegisTasks.Core;
using AegisTasks.Core.Common;
using AegisTasks.Core.TaskAction;
using AegisTasks.TasksLibrary;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AegisTasks.TasksLibrary.TaskAction
{
    /// <summary>
    /// Crear un directorio o falla en el intento (ya existe o no ha podido hacerlo)
    /// </summary>
    public class CreateDirectoryTaskAction : TaskActionBase<CreateDirectoryTaskActionInputParams, object>
    {
        #region PROPERTIES

        #region CONSTANTS

        #region PRIVATE CONSTANTS

        // Nombres en varios idiomas
        private const string NAME_ES = "Comprobar si el archivo es escribible";
        private const string NAME_GL = "Comprobar se o arquivo é escribible";
        private const string NAME_EN = "Is file writable action";

        // Descripciones en varios idiomas
        private const string DESCRIPTION_ES = "Comprueba si un archivo se puede escribir";
        private const string DESCRIPTION_GL = "Comprueba se un arquivo se pode escribir";
        private const string DESCRIPTION_EN = "Checks if a file is writable";

        #endregion PRIVATE CONSTANTS
        #region PUBLIC CONSTANTS
        public readonly static string CALL_NAME = nameof(CreateDirectoryTaskAction);
        public readonly static string NAME = "Create directory action";
        public readonly static string DESCRIPTION = "Acción de tarea responsable de crear un directorio en la ruta especificada";

        #endregion PUBLIC CONSTANTS

        #endregion CONSTANTS

        #region STATIC PROPERTIES
        #region STATIC PRIVATE PROPERTIES
        #endregion STATIC PRIVATE PROPERTIES
        #region STATIC PUBLIC PROPERTIES
        #endregion STATIC PUBLIC PROPERTIES
        #endregion STATIC PROPERTIES

        #region PRIVATE PROPERTIES
        #endregion PRIVATE PROPERTIES

        #region PUBLIC PROPERTIES
        #endregion PUBLIC PROPERTIES

        #endregion PROPERTIES



        #region METHODS

        #region CONSTRUCTOR

        public CreateDirectoryTaskAction()
            : this(null)
        { }

        public CreateDirectoryTaskAction(CreateDirectoryTaskActionInputParams inputParams)
            : base(CALL_NAME, Constants.TASK_CATEGORY_FILES, NAME_ES,
        DESCRIPTION_ES,
        NAME_GL,
        DESCRIPTION_GL,
        NAME_EN,
        DESCRIPTION_EN)
        {
            this.InputParams = inputParams;
        }

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
            throw new NotImplementedException();
        }

        public override ExecutionResult Compensate(IStepExecutionContext context)
        {
            throw new NoCompensationTaskActionException(this.CallName);
        }

        public override object GetResult()
        {
            throw new NoResultTaskActionException(this.CallName);
        }
        public override void RegisterAtServices(IServiceCollection services)
        {
            services.AddTransient<CreateDirectoryTaskAction>();
        }

        #endregion PUBLIC METHODS

        #region PRIVATE METHODS

        protected override bool areInputParamsValid()
        {
            if (InputParams is null || InputParams.DirectoryPath is null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected override Task<ExecutionResult> internalRun(IStepExecutionContext context)
        {

            DirectoryInfo directoryToCreate = InputParams.DirectoryPath;
            bool directoryAlreadyExist = directoryToCreate.Exists;

            if (directoryAlreadyExist && !InputParams.OverwriteIfExists)
            {
                this.logWarn("El directorio ya existe y no puede ser sobrescrito. Se considerará creado exitosamente");
            } else
            {
                // Si existe y se permite sobrescribir borramos el contenido que haya
                if (directoryAlreadyExist && InputParams.OverwriteIfExists)
                {
                    logWarn($"El directorio {directoryToCreate} ya existía y se va a eliminar");
                    directoryToCreate.Delete(true);
                }

                directoryToCreate.Create();
            }

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

        #endregion METHODS
    }
}
