using System.Collections.Generic;
using System.IO;

namespace AegisTasks.TasksLibrary.TaskAction
{
    /// <summary>
    /// Salida de la acción ScanDirectoryTaskAction
    /// </summary>
    public class ScanDirectoryTaskActionOutputParams
    {
        /// <summary>
        /// Lista de directorios encontrados
        /// </summary>
        public List<DirectoryInfo> Directories { get; set; } = new List<DirectoryInfo>();

        /// <summary>
        /// Lista de archivos encontrados
        /// </summary>
        public List<FileInfo> Files { get; set; } = new List<FileInfo>();
    }
}
