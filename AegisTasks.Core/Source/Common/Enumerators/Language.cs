using System;
using System.Globalization;

namespace AegisTasks.Core.Common
{
    /// <summary>
    /// Lenguajes disponibles
    /// </summary>
    public enum Language
    {
        SPANISH,
        GALICIAN,
        ENGLISH
    }

    public static class LanguageExtensions
    {
        /// <summary>
        /// Convierte un Language a CultureInfo
        /// </summary>
        public static CultureInfo ToCulture(this Language language)
        {
            switch (language)
            {
                case Language.SPANISH:
                    return new CultureInfo("es-ES");
                case Language.GALICIAN:
                    return new CultureInfo("gl-ES");
                case Language.ENGLISH:
                    return new CultureInfo("en-US");
                default:
                    return CultureInfo.InvariantCulture;
            }
        }
    }
}
