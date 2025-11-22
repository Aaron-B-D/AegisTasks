using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AegisTasks.TasksLibrary.TaskPlan
{
    public class CopyDirectoryPlanInputParams
    {

        #region PUBLIC PROPERTIES

        /// <summary>
        /// Si el directorio de destino no existe, si se puede crear o no
        /// </summary>
        public bool CreateDestinationDirectoryIfNotExists { get; set; }
        /// <summary>
        /// Si alguno de los directorios ya existen en el directorio de destino, si se sobrescriben o no
        /// </summary>
        public bool OverwriteDirectoriesIfExist { get; set; }
        /// <summary>
        /// Si alguno de los archivos ya existen en el directorio de destino, si se sobrescriben o no
        /// </summary>
        public bool OverwriteFilesIfExist { get; set; }
        /// <summary>
        /// El directorio del que se quiere copiar
        /// </summary>
        public DirectoryInfo DirectoryToCopy { get; set; }

        /// <summary>
        /// El directorio de destino
        /// </summary>
        public DirectoryInfo DestinationDirectory { get; set; }

        /// <summary>
        /// La profundidad a la que se desea copiar. Si no se aporta se copia el directorio entero hasta su fin
        /// </summary>
        public int? CopyDepth { get; set; }

        #endregion PUBLIC PROPERTIES


        #region CONSTRUCTOR
        public CopyDirectoryPlanInputParams(bool createDestinationDirectoryIfNotExists, bool overwriteDirectoriesIfExist, bool overwriteFilesIfExist, DirectoryInfo directoryToCopy, DirectoryInfo destinationDirectory, int? copyDepth)
        {
            CreateDestinationDirectoryIfNotExists = createDestinationDirectoryIfNotExists;
            OverwriteDirectoriesIfExist = overwriteDirectoriesIfExist;
            OverwriteFilesIfExist = overwriteFilesIfExist;
            DirectoryToCopy = directoryToCopy;
            DestinationDirectory = destinationDirectory;
            CopyDepth = copyDepth;
        }

        public CopyDirectoryPlanInputParams(bool createDestinationDirectoryIfNotExists, bool overwriteDirectoriesIfExist, bool overwriteFilesIfExist, DirectoryInfo directoryToCopy, DirectoryInfo destinationDirectory) : this(createDestinationDirectoryIfNotExists, overwriteFilesIfExist, overwriteFilesIfExist, directoryToCopy, destinationDirectory, null)
        {}

        #endregion CONSTRUCTOR

        public bool IsValid()
        {
            return this.DirectoryToCopy != null && this.DestinationDirectory != null && this.CopyDepth == null || this.CopyDepth >= 0;
        }

    }
}
