using System;
using System.Globalization;

namespace AegisTasks.Core.Common
{
    /// <summary>
    /// Lenguajes disponibles
    /// </summary>
    public enum SupportedLanguage
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
        public static CultureInfo ToCulture(this SupportedLanguage language)
        {
            switch (language)
            {
                case SupportedLanguage.SPANISH:
                    return new CultureInfo("es-ES");
                case SupportedLanguage.GALICIAN:
                    return new CultureInfo("gl-ES");
                case SupportedLanguage.ENGLISH:
                    return new CultureInfo("en-US");
                default:
                    return CultureInfo.InvariantCulture;
            }
        }
    }
}
