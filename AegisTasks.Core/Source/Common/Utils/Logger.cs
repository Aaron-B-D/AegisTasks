using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace AegisTasks.Core.Common
{
    /// <summary>
    /// Logger global para toda la aplicación.
    /// Permite registrar mensajes en consola y en archivo, incluyendo nivel de log, clase y método.
    /// </summary>
    public static class Logger
    {

        #region PROPERTIES

        /// <summary>
        /// Niveles de log disponibles.
        /// </summary>
        public enum LogLevel
        {
            Info,
            Warning,
            Error,
            Debug
        }

        #region CONSTANTS

        #region PRIVATE CONSTANTS
        #endregion PRIVATE CONSTANTS
        #region PUBLIC CONSTANTS
        #endregion PUBLIC CONSTANTS

        #endregion CONSTANTS

        #region STATIC PROPERTIES
        #region STATIC PRIVATE PROPERTIES

        /// <summary>
        /// Nombre del directorio de logs.
        /// </summary>
        private static readonly string _LogDirectoryName = "Logs";

        /// <summary>
        /// Nombre del archivo de log.
        /// </summary>
        private static readonly string _LogFileName = "AegisTask_Log.log";

        /// <summary>
        /// Ruta completa del directorio de logs.
        /// </summary>
        private static readonly string _LogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _LogDirectoryName);

        /// <summary>
        /// Ruta completa del archivo de log.
        /// </summary>
        private static readonly string _LogFile = Path.Combine(_LogDirectory, _LogFileName);

        #endregion STATIC PRIVATE PROPERTIES
        #region STATIC PUBLIC PROPERTIES
        #endregion STATIC PUBLIC PROPERTIES
        #endregion STATIC PROPERTIES

        #region PRIVATE PROPERTIES
        #endregion PRIVATE PROPERTIES

        #region PUBLIC PROPERTIES
        #endregion PUBLIC PROPERTIES

        #endregion PROPERTIES

        #region METHODS

        #region CONSTRUCTOR

        /// <summary>
        /// Constructor estático que asegura la existencia de la carpeta y archivo de logs.
        /// </summary>
        static Logger()
        {
            if (!Directory.Exists(_LogDirectory))
            {
                Directory.CreateDirectory(_LogDirectory);
            }

            if (!File.Exists(_LogFile))
            {
                File.Create(_LogFile).Dispose();
            }
        }

        #endregion CONSTRUCTOR

        #region STATIC METHODS

        #region STATIC PRIVATE METHODS

        /// <summary>
        /// Convierte un nivel de log en su etiqueta correspondiente (INFO, WARN, ERROR).
        /// </summary>
        /// <param name="level">Nivel de log</param>
        /// <returns>Etiqueta como string</returns>
        private static string getLevelLabel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Info:
                    return "INFO";
                case LogLevel.Warning:
                    return "WARNING";
                case LogLevel.Error:
                    return "ERROR";
                default:                    
                    LogWarn($"El nivel {level} no está soportado. Se devolverá un string vacío");
                    return "";
            }
        }

        #endregion STATIC PRIVATE METHODS

        #region STATIC PUBLIC METHODS
        #endregion STATIC PUBLIC METHODS

        #endregion STATIC METHODS

        #region PUBLIC METHODS
        #endregion PUBLIC METHODS

        #region PRIVATE METHODS
        #endregion PRIVATE METHODS

        #endregion METHODS

        /// <summary>
        /// Registra un mensaje en consola y archivo, indicando clase, método y nivel.
        /// </summary>
        /// <param name="message">Mensaje a registrar</param>
        /// <param name="className">Nombre de la clase que genera el log</param>
        /// <param name="methodName">Nombre del método que genera el log</param>
        /// <param name="level">Nivel de log</param>
        public static void Log(string message, string className, string methodName, LogLevel level)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string levelLabel = getLevelLabel(level);

            // Solo agrega el punto si methodName tiene valor
            string classAndMethod = string.IsNullOrEmpty(methodName) ? className : $"{className}.{methodName}";

            string logMessage = $"[{timestamp}] [{levelLabel}] [{classAndMethod}] {message}";

            // Consola con color
            ConsoleColor originalColor = Console.ForegroundColor;
            switch (level)
            {
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }

            Console.WriteLine(logMessage);
            Debug.WriteLine(logMessage);
            Console.ForegroundColor = originalColor;

            // Escritura en archivo
            try
            {
                File.AppendAllText(_LogFile, logMessage + Environment.NewLine + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{getLevelLabel(LogLevel.Error)}] {nameof(Logger)}.{nameof(Log)} No se pudo escribir en el archivo de log: " + ex.Message);
                Console.ForegroundColor = originalColor;
            }
        }

        /// <summary>
        /// Registra un log de información capturando automáticamente clase y método.
        /// </summary>
        public static void LogDebug(string message,
                    [CallerFilePath] string filePath = "",
                    [CallerMemberName] string methodName = ""
        )
        {
            string className = Path.GetFileNameWithoutExtension(filePath);
            Log(message, className, methodName, LogLevel.Debug);
        }

        /// <summary>
        /// Registra un log de información capturando automáticamente clase y método.
        /// </summary>
        public static void LogInfo(string message,
                    [CallerFilePath] string filePath = "",
                    [CallerMemberName] string methodName = "")
        {
            string className = Path.GetFileNameWithoutExtension(filePath);
            Log(message, className, methodName, LogLevel.Info);
        }

        /// <summary>
        /// Registra un log de advertencia capturando automáticamente clase y método.
        /// </summary>
        public static void LogWarn(string message,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string methodName = "")
        {
            string className = Path.GetFileNameWithoutExtension(filePath);
            Log(message, className, methodName, LogLevel.Warning);
        }

        /// <summary>
        /// Registra un log de error capturando automáticamente clase y método.
        /// </summary>
        public static void LogError(string message,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string methodName = "")
        {
            string className = Path.GetFileNameWithoutExtension(filePath);
            Log(message, className, methodName, LogLevel.Error);
        }

        /// <summary>
        /// Registra una excepción capturando automáticamente clase y método.
        /// Incluye mensaje y stack trace completo.
        /// </summary>
        public static void LogException(Exception ex,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string methodName = "")
        {
            string className = Path.GetFileNameWithoutExtension(filePath);
            string message = $"Excepción: {ex.GetType().Name}. Mensaje: {ex.Message}. StackTrace: {ex.StackTrace}";
            Log(message, className, methodName, LogLevel.Error);
        }
    }
}
