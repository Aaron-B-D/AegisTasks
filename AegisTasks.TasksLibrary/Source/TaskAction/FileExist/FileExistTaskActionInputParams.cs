using AegisTasks.Core.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AegisTasks.Core.TaskAction;

namespace AegisTasks.TasksLibrary.TaskAction
{
    /// <summary>
    /// Los parámetros de entrada de una acción de comprobación de existencia de un archivo
    /// </summary>
    public class FileExistTaskActionInputParams: TaskActionInputParamsBase
    {
        /// <summary>
        /// La ruta del archivo a comprobar
        /// </summary>
        public FileInfo FilePath { get; set; }

        public FileExistTaskActionInputParams(FileInfo filePath, int numRetries, int retryIntervalMs)
            : base(numRetries, retryIntervalMs)
        {
            this.FilePath = filePath;
        }

        public FileExistTaskActionInputParams(FileInfo filePath)
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
