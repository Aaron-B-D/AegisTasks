using System;
using System.Collections.Generic;
using System.Text;

namespace AegisTasks.Core.Common
{
    public static class Constants
    {
        /// <summary>
        /// Identificador para tipos enteros no definidos
        /// </summary>
        public const int INT_ID_UNDEFINED = -1;

        /// <summary>
        /// Identificador para tipos enteros no definidos
        /// </summary>
        public const int INT_UNDEFINED = 0;

        /// <summary>
        /// Identificador para tipos long no definidos
        /// </summary>
        public const long LONG_ID_UNDEFINED = -1;

        /// <summary>
        /// Valor por defecto para variables de tipo 'bool' no definidas
        /// </summary>
        public const bool BOOL_UNDEFINED = false;

        /// <summary>
        /// Valor por defecto para variables de tipo 'double' no definidas
        /// </summary>
        public const double DOUBLE_UNDEFINED = -1;

        /// <summary>
        /// Valor por defecto para variables de tipo 'float' no definidas
        /// </summary>
        public const float FLOAT_UNDEFINED = -1.0f;

        /// <summary>
        /// Valor por defecto para variables de tipo 'string' no definidas
        /// </summary>
        public const string STRING_UNDEFINED = "";

        /// <summary>
        /// Categoría empleada por tareas atómicas internas que están relacionadas con la gestión de archivos (documentos, directorios, etc)
        /// </summary>
        public const string TASK_CATEGORY_FILES = "Files";
    }
}
