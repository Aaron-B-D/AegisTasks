using AegisTasks.Core.Common;
using AegisTasks.Core.TaskAction;
using AegisTasks.TasksLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AegisTasks.TasksLibrary.TaskAction
{
    /// <summary>
    /// Parámetros de una acción de crear directorio
    /// </summary>
    public class CreateDirectoryTaskActionInputParams: TaskActionInputParamsBase
    {
        #region PROPERTIES

        #region CONSTANTS

        #region PRIVATE CONSTANTS

        #endregion PRIVATE CONSTANTS
        #region PUBLIC CONSTANTS
        #endregion PUBLIC CONSTANTS

        #endregion CONSTANTS

        #region STATIC PROPERTIES
        #region STATIC PRIVATE PROPERTIES

        #endregion STATIC PRIVATE PROPERTIES
        #region STATIC PUBLIC PROPERTIES
        #endregion STATIC PUBLIC PROPERTIES
        #endregion STATIC PROPERTIES

        #region PRIVATE PROPERTIES
        #endregion PRIVATE PROPERTIES

        #region PUBLIC PROPERTIES

        /// <summary>
        /// La ruta del directorio a comprobar
        /// </summary>
        public DirectoryInfo DirectoryPath { get; set; }

        public bool OverwriteIfExists { get; set; } = false;

        #endregion PUBLIC PROPERTIES

        #endregion PROPERTIES



        #region METHODS

        #region CONSTRUCTOR

        public CreateDirectoryTaskActionInputParams(DirectoryInfo directory, bool overwriteIfExists, int numRetries, int retryIntervalMs)
            : base(numRetries, retryIntervalMs)
        {
            this.DirectoryPath = directory;
            this.OverwriteIfExists = overwriteIfExists;
        }

        public CreateDirectoryTaskActionInputParams(DirectoryInfo directory, bool overwriteIfExists)
            : base()
        {
            this.DirectoryPath = directory;
            this.OverwriteIfExists = overwriteIfExists;
        }

        #endregion CONSTRUCTOR

        #region PUBLIC METHODS


        public override object Clone()
        {
            throw new NotImplementedException();
        }

        #endregion PUBLIC METHODS

        #endregion METHODS

    }
}
