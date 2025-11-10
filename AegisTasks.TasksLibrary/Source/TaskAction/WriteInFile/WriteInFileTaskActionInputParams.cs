using AegisTasks.Core.Common;
using AegisTasks.Core.TaskAction;
using System;
using System.IO;


namespace AegisTasks.TasksLibrary.TaskAction
{
    /// <summary>
    /// Parámetros de entrada para la acción de escritura en un archivo (contenido genérico)
    /// </summary>
    public class WriteInFileTaskActionInputParams : TaskActionInputParamsBase
    {
        /// <summary>
        /// Archivo destino
        /// </summary>
        public FileInfo FilePath { get; set; }

        /// <summary>
        /// Contenido a escribir, puede ser string, byte[], Stream u otros tipos
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// True si se quiere añadir sobre lo existente, False para sobrescribir
        /// </summary>
        public bool Append { get; set; }

        public WriteInFileTaskActionInputParams(FileInfo filePath, object content, int numRetries, int retryIntervalMs, bool append = true)
            : base(numRetries, retryIntervalMs)
        {
            FilePath = filePath;
            Content = content;
            Append = append;
        }

        public WriteInFileTaskActionInputParams(FileInfo filePath, object content, bool append = true)
            : base()
        {
            FilePath = filePath;
            Content = content;
            Append = append;
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
