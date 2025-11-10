using AegisTasks.Core;
using AegisTasks.Core.Common;
using AegisTasks.Core.TaskAction;
using AegisTasks.TasksLibrary;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AegisTasks.TasksLibrary.TaskAction
{
    /// <summary>
    /// Escanea todo el contenido de un directorio hasta una profundidad específica
    /// </summary>
    public class ScanDirectoryTaskAction : TaskActionBase<ScanDirectoryTaskActionInputParams, ScanDirectoryTaskActionOutputParams>
    {
        #region CONSTANTS
        public readonly static string CALL_NAME = nameof(ScanDirectoryTaskAction);
        public readonly static string NAME = "Scan directory action";
        public readonly static string DESCRIPTION = "Acción de tarea que escanea el contenido de un directorio hasta la profundidad indicada";
        #endregion CONSTANTS

        #region CONSTRUCTOR
        public ScanDirectoryTaskAction()
            : this(null)
        { }

        public ScanDirectoryTaskAction(ScanDirectoryTaskActionInputParams inputParams)
            : base(CALL_NAME, Constants.TASK_CATEGORY_FILES, TaskActionExecution.Sync, NAME, DESCRIPTION)
        {
            this.InputParams = inputParams;
        }
        #endregion CONSTRUCTOR

        #region PUBLIC METHODS
        public override object Clone() => throw new NotImplementedException();

        public override ExecutionResult Compensate(IStepExecutionContext context)
        {
            throw new NoCompensationTaskActionException(this.CallName);
        }

        public override ScanDirectoryTaskActionOutputParams GetResult()
        {
            return this._OutputParams;
        }

        public override void RegisterAtServices(IServiceCollection services)
        {
            services.AddTransient<ScanDirectoryTaskAction>();
        }

        #endregion PUBLIC METHODS

        #region PRIVATE METHODS
        protected override bool areInputParamsValid()
        {
            return InputParams != null && InputParams.DirectoryToScan != null;
        }

        protected override Task<ExecutionResult> internalRun(IStepExecutionContext context)
        {

            List<DirectoryInfo> directories;
            List<FileInfo> files;

            traverseDirectory(InputParams.DirectoryToScan, out directories, out files, InputParams.MaxDepth, 0);

            // Construimos el objeto de salida
            this._OutputParams = new ScanDirectoryTaskActionOutputParams
            {
                Directories = directories,
                Files = files
            };


            return Task.FromResult(ExecutionResult.Next());

        }

        private void traverseDirectory(DirectoryInfo dir, out List<DirectoryInfo> directories, out List<FileInfo> files, int? maxDepth, int currentDepth)
        {
            directories = new List<DirectoryInfo> { dir };
            files = new List<FileInfo>();

            if (!maxDepth.HasValue || currentDepth < maxDepth.Value)
            {
                foreach (FileInfo file in dir.GetFiles())
                {
                    files.Add(file);
                }

                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    traverseDirectory(subDir, out List<DirectoryInfo> subDirs, out List<FileInfo> subFiles, maxDepth, currentDepth + 1);
                    directories.AddRange(subDirs);
                    files.AddRange(subFiles);
                }
            }
        }

        protected override void logError(string message, [CallerMemberName] string methodName = "") =>
            logError(message, this.CallName, methodName: methodName);

        protected override void logException(Exception exception, [CallerMemberName] string methodName = "") =>
            logException(exception, this.CallName, methodName: methodName);

        protected override void logInfo(string message, [CallerMemberName] string methodName = "") =>
            logInfo(message, this.CallName, methodName: methodName);

        protected override void logWarn(string message, [CallerMemberName] string methodName = "") =>
            logWarn(message, this.CallName, methodName: methodName);

        #endregion PRIVATE METHODS
    }
}
