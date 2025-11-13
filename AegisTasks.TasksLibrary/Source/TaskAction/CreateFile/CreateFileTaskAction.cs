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
    /// Acción responsable de crear un archivo en la ruta especificada
    /// </summary>
    public class CreateFileTaskAction : TaskActionBase<CreateFileTaskActionInputParams, object>
    {
        #region PROPERTIES

        #region CONSTANTS

        #region PRIVATE CONSTANTS

        // Nombres en distintos idiomas
        private const string NAME_ES = "Crear archivo";
        private const string NAME_GL = "Crear arquivo";
        private const string NAME_EN = "Create file";

        // Descripciones en distintos idiomas
        private const string DESCRIPTION_ES = "Crea un archivo en la ruta especificada";
        private const string DESCRIPTION_GL = "Crea un arquivo na ruta especificada";
        private const string DESCRIPTION_EN = "Creates a file at the specified path";

        #endregion PRIVATE CONSTANTS

        #region PUBLIC CONSTANTS
        #endregion PUBLIC CONSTANTS

        #endregion CONSTANTS

        #region STATIC PROPERTIES

        #region STATIC PRIVATE PROPERTIES
        #endregion STATIC PRIVATE PROPERTIES

        #region STATIC PUBLIC PROPERTIES
        public readonly static string CALL_NAME = nameof(CreateFileTaskAction);
        public readonly static string NAME = "Create file action";
        public readonly static string DESCRIPTION = "Acción de tarea responsable de crear un archivo en la ruta especificada";
        #endregion STATIC PUBLIC PROPERTIES

        #endregion STATIC PROPERTIES

        #region PRIVATE PROPERTIES
        #endregion PRIVATE PROPERTIES

        #region PUBLIC PROPERTIES
        #endregion PUBLIC PROPERTIES

        #endregion PROPERTIES

        #region METHODS

        #region CONSTRUCTOR
        public CreateFileTaskAction()
            : base(CALL_NAME, Constants.TASK_CATEGORY_FILES, NAME_ES,
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
            services.AddTransient<CreateFileTaskAction>();
        }

        #endregion PUBLIC METHODS

        #region PRIVATE METHODS

        protected override bool areInputParamsValid()
        {
            if (InputParams is null || InputParams.FileExtension is null || InputParams.FileName is null)
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
            string fileName = InputParams.FileName;
            string fullPath = Path.Combine(InputParams.DirectoryPath, fileName + InputParams.FileExtension);

            if (File.Exists(fullPath) && !InputParams.OverwriteIfExists)
            {
                this.logWarn("El fichero ya existe y no puede ser sobrescrito. Se considerará creado exitosamente");
            }

            if (File.Exists(fullPath) && InputParams.OverwriteIfExists)
            {
                logWarn($"El archivo {fileName} ya existía y se va a eliminar");
                File.Delete(fullPath);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                // Solo crea el archivo, nada más
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
