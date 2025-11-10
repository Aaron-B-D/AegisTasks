using AegisTasks.Core.Common;
using System.IO;
using AegisTasks.Core.TaskAction;
using System;

namespace AegisTasks.TasksLibrary.TaskAction
{
    /// <summary>
    /// Parámetros de entrada de la acción de comprobación de existencia de un directorio
    /// </summary>
    public class DirectoryExistTaskActionInputParams : TaskActionInputParamsBase
    {
        /// <summary>
        /// La ruta del directorio a comprobar
        /// </summary>
        public DirectoryInfo DirectoryPath { get; set; }

        public DirectoryExistTaskActionInputParams(DirectoryInfo directoryPath, int numRetries, int retryIntervalMs)
            : base(numRetries, retryIntervalMs)
        {
            this.DirectoryPath = directoryPath;
        }

        public DirectoryExistTaskActionInputParams(DirectoryInfo directoryPath)
            : base()
        {
            this.DirectoryPath = directoryPath;
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
