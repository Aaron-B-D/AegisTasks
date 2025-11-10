using AegisTasks.Core.TaskPlan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AegisTasks.TasksLibrary.TaskPlan
{
	public class CopyDirectoryPlanParams : TaskPlanParamsBase<CopyDirectoryPlanInputParams, CopyDirectoryPlanInternalParams>
	{

		#region PUBLIC PROPERTIES


		#region INPUT PARAMS

		/// <summary>
		/// Si el directorio de destino no existe, si se puede crear o no
		/// </summary>
		public bool CreateDestinationDirectoryIfNotExists
		{
			get
			{
				return InputParams.CreateDestinationDirectoryIfNotExists;
			}
			set
			{
				InputParams.CreateDestinationDirectoryIfNotExists = value;
			}
		}

		/// <summary>
		/// Si alguno de los directorios ya existen en el directorio de destino, si se sobrescriben o no
		/// </summary>
		public bool OverwriteDirectoriesIfExist
		{
			get
			{
				return InputParams.OverwriteDirectoriesIfExist;
			}
			set
			{
				InputParams.OverwriteDirectoriesIfExist = value;
			}
		}

		/// <summary>
		/// Si alguno de los archivos ya existen en el directorio de destino, si se sobrescriben o no
		/// </summary>
		public bool OverwriteFilesIfExists
		{
			get
			{
				return InputParams.OverwriteFilesIfExist;
			}
			set
			{
				InputParams.OverwriteFilesIfExist = value;
			}
		}

		/// <summary>
		/// El directorio del que se quiere copiar
		/// </summary>
		public DirectoryInfo DirectoryToCopy
		{
			get
			{
				return InputParams.DirectoryToCopy;
			}
			set
			{
				InputParams.DirectoryToCopy = value;
			}
		}


		/// <summary>
		/// El directorio de destino
		/// </summary>
		public DirectoryInfo DestinationDirectory
		{
			get
			{
				return InputParams.DestinationDirectory;
			}
			set
			{
				InputParams.DestinationDirectory = value;
			}
		}


		public int? CopyDepth
		{
			get
			{
				return InputParams.CopyDepth;
			}
			set
			{
				InputParams.CopyDepth = value;
			}
		}


		#endregion INPUT PARAMS

		#region INTERNAL PARAMS


		/// <summary>
		/// Existe o no el directorio de destino
		/// </summary>
		public bool DestinationDirectoryExist
        {
            get
            {
                return InternalParams.DestinationDirectoryExist;
            }
            set
            {
                InternalParams.DestinationDirectoryExist = value;
            }
        }

        /// <summary>
        /// Existe o no el directorio a copiar
        /// </summary>
        public bool DirectoryToCopyExist
        {
            get
            {
                return InternalParams.DirectoryToCopyExist;
            }
            set
            {
                InternalParams.DirectoryToCopyExist = value;
            }
        }

        /// <summary>
        /// Si el directorio a copiar está vacío o no
        /// </summary>
        public bool DirectoryToCopyIsEmpty
        {
            get
            {
                return InternalParams.DirectoryToCopyIsEmpty;
            }
            set
            {
                InternalParams.DirectoryToCopyIsEmpty = value;
            }
        }

        /// <summary>
        /// Lista de directorios encontrados
        /// </summary>
        public List<DirectoryInfo> Directories
        {
            get
            {
                return InternalParams.Directories;
            }
            set
            {
                InternalParams.Directories = value;
            }
        }

        /// <summary>
        /// Lista de archivos encontrados
        /// </summary>
        public List<FileInfo> Files
        {
            get
            {
                return InternalParams.Files;
            }
            set
            {
                InternalParams.Files = value;
            }
        }

        #endregion INTERNAL PARAMS

        #endregion PUBLIC PROPERTIES


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public CopyDirectoryPlanParams()
			: this(null)
		{}

		/// <summary>
		/// Constructor con parámetros de configuración e internos
		/// </summary>
		public CopyDirectoryPlanParams(CopyDirectoryPlanInputParams InputParams)
			: this(InputParams, new CopyDirectoryPlanInternalParams())
		{}

		protected CopyDirectoryPlanParams(
			CopyDirectoryPlanInputParams InputParams, CopyDirectoryPlanInternalParams InternalParams)
			: base(InputParams, InternalParams)
		{}

		#endregion CONSTRUCTOR
	}
}
