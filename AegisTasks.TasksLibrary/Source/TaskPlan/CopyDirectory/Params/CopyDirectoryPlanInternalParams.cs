using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AegisTasks.TasksLibrary.TaskPlan
{
    public class CopyDirectoryPlanInternalParams
    {

        #region PUBLIC PROPERTIES

        /// <summary>
        /// Existe o no el directorio de destino
        /// </summary>
        public bool DestinationDirectoryExist { get; set; }

        /// <summary>
        /// Existe o no el directorio a copiar
        /// </summary>
        public bool DirectoryToCopyExist { get; set; }

        /// <summary>
        /// Si el directorio a copiar está vacío o no
        /// </summary>
        public bool DirectoryToCopyIsEmpty { get; set; }

        /// <summary>
        /// Lista de directorios encontrados en el directorio a copiar
        /// </summary>
        public List<DirectoryInfo> Directories { get; set; } = new List<DirectoryInfo>();

        /// <summary>
        /// Lista de archivos encontrados en el directorio a copiar
        /// </summary>
        public List<FileInfo> Files { get; set; } = new List<FileInfo>();

        #endregion PUBLIC PROPERTIES

    }
}
