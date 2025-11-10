using System;
using System.Collections.Generic;
using System.Text;

namespace AegisTasks.TasksLibrary.TaskPlan
{
    public class WriteInFilePlanInternalParams
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
        /// Si se ha confirmado la existencia o no existencia del directorio del plan
        /// </summary>
        public bool DirectoryExist { get; set; } = false;

        /// <summary>
        /// Indica si el archivo existe
        /// </summary>
        public bool FileExist { get; set; } = false;

        /// <summary>
        /// Se tienen permisos de escritura en el archivo
        /// </summary>
        public bool FileIsWritable { get; set; } = false;

        #endregion PUBLIC PROPERTIES

        #endregion PROPERTIES



        #region METHODS

        #region CONSTRUCTOR

        public WriteInFilePlanInternalParams()
        {}

        #endregion CONSTRUCTOR

        #region STATIC METHODS

        #region STATIC PRIVATE METHODS
        #endregion STATIC PRIVATE METHODS
        #region STATIC PUBLIC METHODS
        #endregion STATIC PUBLIC METHODS

        #endregion STATIC METHODS

        #region PUBLIC METHODS
        #endregion PUBLIC METHODS

        #region PRIVATE METHODS
        #endregion PRIVATE METHODS

        #endregion METHODS



        public object Clone() {
            return (WriteInFilePlanInternalParams)this.MemberwiseClone();
        }
    }
}
