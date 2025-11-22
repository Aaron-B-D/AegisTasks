using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AegisTasks.TasksLibrary.TaskPlan
{
    public class WriteInFilePlanInputParams
    {
        #region PROPERTIES

        #region PUBLIC PROPERTIES

        /// <summary>
        /// La ruta del archivo donde escribir
        /// </summary>
        public FileInfo FilePath { get; set; }

        /// <summary>
        /// El contenido a escribir en el archivo
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// Indica si se debe crear el archivo si no existe
        /// </summary>
        public bool CreateFileIfNotExists { get; set; }

        /// <summary>
        /// Indica si se debe crear el directorio si no existe
        /// </summary>
        public bool CreateDirectoryIfNotExists { get; set; }

        /// <summary>
        /// Si el contenido aportado se añade a lo existente (true) o si se sobrescribe el contenido (false)
        /// </summary>
        public bool AppendContent { get; set; }

        #endregion PUBLIC PROPERTIES

        #endregion PROPERTIES

        #region METHODS

        #region CONSTRUCTOR

        public WriteInFilePlanInputParams(FileInfo filePath, object content, bool createFileIfNotExists, bool createDirectoryIfNotExists, bool appendContent)
        {
            this.FilePath = filePath;
            this.Content = content;
            this.CreateFileIfNotExists = createFileIfNotExists;
            this.CreateDirectoryIfNotExists = createDirectoryIfNotExists;
            this.AppendContent = appendContent;
        }

        #endregion CONSTRUCTOR

        #region PUBLIC METHODS

        public object Clone()
        {
            WriteInFilePlanInputParams clone = (WriteInFilePlanInputParams)this.MemberwiseClone();

            // Clonar FileInfo para no compartir la misma referencia
            if (this.FilePath != null)
                clone.FilePath = new FileInfo(this.FilePath.FullName);

            // Intentar clonar Content si implementa ICloneable
            if (this.Content is ICloneable cloneable)
                clone.Content = cloneable.Clone();
            else
                clone.Content = this.Content;

            return clone;
        }

        public bool IsValid()
        {
            return this.FilePath != null && this.Content != null;
        }

        #endregion PUBLIC METHODS

        #endregion METHODS
    }
}
