using AegisTasks.Core.TaskAction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AegisTasks.TasksLibrary.TaskAction
{
    /// <summary>
    /// Parámetros de una acción de copiar archivo
    /// </summary>
    public class CopyFileTaskActionInputParams : TaskActionInputParamsBase
    {
        #region PUBLIC PROPERTIES

        /// <summary>
        /// El archivo que se desea copiar
        /// </summary>
        public FileInfo FileToCopy { get; set; }

        /// <summary>
        /// El directorio al que se desea copiar
        /// </summary>
        public DirectoryInfo FileDestination { get; set; }

        /// <summary>
        /// Si el archivo ya existe en el directorio destino, sobrescribirlo o no
        /// </summary>
        public bool OverwriteIfExist { get; set; }

        #endregion PUBLIC PROPERTIES


        #region CONSTRUCTOR

        public CopyFileTaskActionInputParams(FileInfo fileToCopy, DirectoryInfo fileDestination, bool overwriteIfExists, int numRetries, int retryIntervalMs)
            : base(numRetries, retryIntervalMs)
        {
            this.FileToCopy = fileToCopy;
            this.FileDestination = fileDestination;
            this.OverwriteIfExist = overwriteIfExists;
        }

        public CopyFileTaskActionInputParams(FileInfo fileToCopy, DirectoryInfo fileDestination, bool overwriteIfExists)
            : base()
        {
            this.FileToCopy = fileToCopy;
            this.FileDestination = fileDestination;
            this.OverwriteIfExist = overwriteIfExists;
        }

        #endregion CONSTRUCTOR


        #region PUBLIC METHODS

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        #endregion PUBLIC METHODS

    }
}
