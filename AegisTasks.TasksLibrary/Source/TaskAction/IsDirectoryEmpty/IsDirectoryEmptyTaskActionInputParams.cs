using AegisTasks.Core.TaskAction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AegisTasks.TasksLibrary.Source.TaskAction.IsDirectoryEmpty
{
    /// <summary>
    /// Parámetros de entrada para la acción de comprobación de que un directorio está vacío
    /// </summary>
    public class IsDirectoryEmptyTaskActionInputParams : TaskActionInputParamsBase
    {
        /// <summary>
        /// El directorio a comprobar
        /// </summary>
        public DirectoryInfo DirectoryToCheck { get; set; }

        public IsDirectoryEmptyTaskActionInputParams(DirectoryInfo directoryToCheck, int numRetries, int retryIntervalMs)
            : base(numRetries, retryIntervalMs)
        {
            this.DirectoryToCheck = directoryToCheck;
        }

        public IsDirectoryEmptyTaskActionInputParams(DirectoryInfo directoryToCheck)
            : base()
        {
            this.DirectoryToCheck = directoryToCheck;
        }
        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
