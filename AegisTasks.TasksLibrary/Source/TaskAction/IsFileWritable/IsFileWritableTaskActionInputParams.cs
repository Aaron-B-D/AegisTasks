using AegisTasks.Core.Common;
using AegisTasks.Core.TaskAction;
using System;
using System.IO;


namespace AegisTasks.TasksLibrary.TaskAction
{
    /// <summary>
    /// Parámetros de entrada para la acción de comprobación de escritura de un archivo
    /// </summary>
    public class IsFileWritableTaskActionInputParams : TaskActionInputParamsBase
    {
        /// <summary>
        /// La ruta del archivo a comprobar
        /// </summary>
        public FileInfo FilePath { get; set; }

        public IsFileWritableTaskActionInputParams(FileInfo filePath, int numRetries, int retryIntervalMs)
            : base(numRetries, retryIntervalMs)
        {
            this.FilePath = filePath;
        }

        public IsFileWritableTaskActionInputParams(FileInfo filePath)
            : base()
        {
            this.FilePath = filePath;
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
