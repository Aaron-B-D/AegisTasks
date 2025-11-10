using AegisTasks.Core.Common;
using AegisTasks.Core.TaskPlan;
using AegisTasks.TasksLibrary.TaskPlan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AegisTasks.TasksLibrary.TaskPlan
{
    /// <summary>
    /// Parámetros de entrada para el plan de escritura en archivo
    /// </summary>
    public class WriteInFilePlanParams: TaskPlanParamsBase<WriteInFilePlanInputParams, WriteInFilePlanInternalParams>
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

        #region INTERNAL PROPERTIES

        /// <summary>
        /// Indica si el directorio existe
        /// </summary>
        public bool DirectoryExist
        {
            get => this.InternalParams.DirectoryExist;
            set => this.InternalParams.DirectoryExist = value;
        }

        /// <summary>
        /// Indica si el archivo existe
        /// </summary>
        public bool FileExist
        {
            get => this.InternalParams.FileExist;
            set => this.InternalParams.FileExist = value;
        }

        /// <summary>
        /// Si el archivo se puede escribir
        /// </summary>
        public bool FileIsWritable
        {
            get => this.InternalParams.FileIsWritable;
            set => this.InternalParams.FileIsWritable = value;
        }

        #endregion INTERNAL PROPERTIES

        #region CONFIG PROPERTIES

        /// <summary>
        /// La ruta del archivo donde escribir
        /// </summary>
        public FileInfo FilePath
        {
            get => this.InputParams.FilePath;
            set => this.InputParams.FilePath = value;
        }

        /// <summary>
        /// El contenido a escribir en el archivo
        /// </summary>
        public object Content
        {
            get => this.InputParams.Content;
            set => this.InputParams.Content = value;
        }

        /// <summary>
        /// Indica si se debe crear el archivo si no existe
        /// </summary>
        public bool CreateFileIfNotExists
        {
            get => this.InputParams.CreateFileIfNotExists;
            set => this.InputParams.CreateFileIfNotExists = value;
        }

        /// <summary>
        /// Indica si se debe crear el archivo si no existe
        /// </summary>
        public bool CreateDirectoryIfNotExists
        {
            get => this.InputParams.CreateDirectoryIfNotExists;
            set => this.InputParams.CreateDirectoryIfNotExists = value;
        }

        /// <summary>
        /// Si el contenido aportado se añade a lo existente (true) o si se sobrescribe el contenido (false)
        /// </summary>
        public bool AppendContent
        {
            get => this.InputParams.AppendContent;
            set => this.InputParams.AppendContent = value;
        }

        #endregion CONFIG PROPERTIES

        #endregion PUBLIC PROPERTIES

        #endregion PROPERTIES


        #region METHODS

        #region CONSTRUCTOR

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public WriteInFilePlanParams()
            : this(null)
        {
        }

        /// <summary>
        /// Constructor con parámetros de configuración e internos
        /// </summary>
        public WriteInFilePlanParams(
            WriteInFilePlanInputParams InputParams)
            : this(InputParams, new WriteInFilePlanInternalParams())
        {
        }

        protected WriteInFilePlanParams(
            WriteInFilePlanInputParams InputParams, WriteInFilePlanInternalParams InternalParams)
            : base(InputParams, InternalParams)
        {
        }


        #endregion CONSTRUCTOR

        #region STATIC METHODS

        #region STATIC PRIVATE METHODS
        #endregion STATIC PRIVATE METHODS
        #region STATIC PUBLIC METHODS
        #endregion STATIC PUBLIC METHODS

        #endregion STATIC METHODS

        #region PUBLIC METHODS

        public override object Clone()
        {
            WriteInFilePlanInputParams InputParams = null;
            if(this.InputParams != null)
            {
                InputParams = (WriteInFilePlanInputParams)this.InputParams.Clone();
            }


            WriteInFilePlanInternalParams InternalParams = null;
            if(this.InternalParams != null)
            {
                InternalParams = (WriteInFilePlanInternalParams)this.InternalParams.Clone();
            }

            return new WriteInFilePlanParams(InputParams, InternalParams);
        }

        #endregion PUBLIC METHODS

        #region PRIVATE METHODS
        #endregion PRIVATE METHODS

        #endregion METHODS
    }
}
