using AegisTasks.Core.Common;
using AegisTasks.Core.TaskAction;
using System;
using System.IO;

namespace AegisTasks.TasksLibrary.TaskAction
{
    /// <summary>
    /// Parámetros de entrada para la acción de creación de un archivo
    /// </summary>
    public class CreateFileTaskActionInputParams : TaskActionInputParamsBase
    {
        /// <summary>
        /// Nombre del archivo sin extensión
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Extensión del archivo, por ejemplo ".txt"
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// Ruta donde se creará el archivo
        /// </summary>
        public string DirectoryPath { get; set; }

        /// <summary>
        /// Indica si se sobrescribe el archivo si ya existe
        /// </summary>
        public bool OverwriteIfExists { get; set; }

        public CreateFileTaskActionInputParams(string fileName, string fileExtension, string directoryPath, int numRetries, int retryIntervalMs, bool overwriteIfExists = true)
            : base(numRetries, retryIntervalMs)
        {
            FileName = fileName;
            FileExtension = fileExtension;
            DirectoryPath = directoryPath;
            OverwriteIfExists = overwriteIfExists;
        }

        public CreateFileTaskActionInputParams(string fileName, string fileExtension, string directoryPath, bool overwriteIfExists = true)
            : base()
        {
            FileName = fileName;
            FileExtension = fileExtension;
            DirectoryPath = directoryPath;
            OverwriteIfExists = overwriteIfExists;
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
