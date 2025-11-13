using AegisTasks.Core.Common;
using AegisTasks.Core.TaskAction;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AegisTasks.TasksLibrary.TaskAction
{
	/// <summary>
	/// Copia un archivo o falla en el intento
	/// </summary>
	public class CopyFileTaskAction : TaskActionBase<CopyFileTaskActionInputParams, object>
	{

        #region STATIC PUBLIC PROPERTIES

        public readonly static string CALL_NAME = nameof(CopyFileTaskAction);

        #endregion STATIC PUBLIC PROPERTIES


        #region PRIVATE PROPERTIES

        // Nombres en distintos idiomas
        private const string NAME_ES = "Copiar archivo";
        private const string NAME_GL = "Copiar arquivo";
        private const string NAME_EN = "Copy file";

        // Descripciones en distintos idiomas
        private const string DESCRIPTION_ES = "Copia un archivo al destino especificado o falla si no es posible";
        private const string DESCRIPTION_GL = "Copia un arquivo ao destino especificado ou falla se non é posible";
        private const string DESCRIPTION_EN = "Copies a file to the specified destination or fails if not possible";

        #endregion PRIVATE PROPERTIES


        #region CONSTRUCTOR

        public CopyFileTaskAction()
            : this(null)
        { }

        public CopyFileTaskAction(CopyFileTaskActionInputParams inputParams)
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


        #region PUBLIC METHODS

        public override object Clone()
		{
			throw new NotImplementedException();
		}

		public override ExecutionResult Compensate(IStepExecutionContext context)
		{
            try
            {
                if (!this.areInputParamsValid())
                {
                    logWarn("No se pudo compensar: InputParams incompletos o nulos.");
                }
                else
                {
                    // Reconstruir la ruta del archivo copiado
                    string copiedFilePath = Path.Combine(
                        InputParams.FileDestination.FullName,
                        InputParams.FileToCopy.Name
                    );

                    if (File.Exists(copiedFilePath))
                    {
                        File.Delete(copiedFilePath);
                        logInfo($"Archivo copiado eliminado durante compensación: {copiedFilePath}");
                    }
                    else
                    {
                        logWarn($"No se encontró archivo para compensar: {copiedFilePath}");
                    }
                }

                return ExecutionResult.Next();
            }
            catch (Exception ex)
            {
                logException(ex);
                throw new InvalidOperationException(
                    $"Error durante la compensación al intentar eliminar el archivo: {InputParams?.FileToCopy?.Name}",
                    ex
                );
            }
        }

		#endregion PUBLIC METHODS


		#region PRIVATE METHODS

		protected override bool areInputParamsValid()
		{
			if(this.InputParams is null || this.InputParams.FileToCopy is null || this.InputParams.FileDestination is null)
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
            FileInfo fileToCopy = InputParams.FileToCopy;
            if (!fileToCopy.Exists)
            {
                throw new FileNotFoundException("No se ha encontrado el archivo a copiar", fileToCopy.FullName);
            }

            DirectoryInfo destinationDirectory = InputParams.FileDestination;
            if (!destinationDirectory.Exists)
            {
                throw new DirectoryNotFoundException($"Directorio de destino '{destinationDirectory.FullName}' no encontrado");
            }

            string destinationPath = Path.Combine(destinationDirectory.FullName, fileToCopy.Name);
            FileInfo destinationFile = new FileInfo(destinationPath);

            if (destinationFile.Exists)
            {
                if (InputParams.OverwriteIfExist)
                {
                    destinationFile.Delete();
                }
                else
                {
                    throw new InvalidOperationException($"El archivo '{destinationFile.FullName}' ya existe en el directorio de destino y no se permite sobrescribirlo");
                }
            }

            fileToCopy.CopyTo(destinationPath);

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



		public override object GetResult()
		{
			throw new NoResultTaskActionException(this.CallName);
		}

		public override void RegisterAtServices(IServiceCollection services)
		{
			services.AddTransient<CopyFileTaskAction>();
		}

	}
}
