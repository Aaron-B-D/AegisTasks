using AegisTasks.Core.Common;
using AegisTasks.Core.TaskAction;
using System;
using System.IO;

namespace AegisTasks.TasksLibrary.TaskAction
{
    /// <summary>
    /// Parámetros de entrada para la acción de comprobación de lectura de un archivo
    /// </summary>
    public class IsFileReadableTaskActionInputParams : TaskActionInputParamsBase
    {
        /// <summary>
        /// La ruta del archivo a comprobar
        /// </summary>
        public FileInfo FilePath { get; set; }

        public IsFileReadableTaskActionInputParams(FileInfo filePath, int numRetries, int retryIntervalMs)
            : base(numRetries, retryIntervalMs)
        {
            this.FilePath = filePath;
        }

        public IsFileReadableTaskActionInputParams(FileInfo filePath)
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
