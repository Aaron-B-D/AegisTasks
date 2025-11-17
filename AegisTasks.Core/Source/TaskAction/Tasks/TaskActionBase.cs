using AegisTasks.Core.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AegisTasks.Core.TaskAction
{
    /// <summary>
    /// Clase base de todas las tareas atómicas de Aegis. Representa un paso o acción concreta y deberían ser lo más sencilla posible. Equivalente a un StepBodyAsync de Workflow Core
    /// </summary>
    public abstract class TaskActionBase<TaskActionInputParamsType, TaskActionOutputParamsType>
        : StepBodyAsync, ITaskActionRegistrable, ICloneable
    {
        #region PROPERTIES

        #region PRIVATE PROPERTIES

        protected readonly string _ES_Name = String.Empty;
        protected readonly string _ES_Description = String.Empty;
        protected readonly string _GL_Name = String.Empty;
        protected readonly string _GL_Description = String.Empty;
        protected readonly string _EN_Name = String.Empty;
        protected readonly string _EN_Description = String.Empty;

        protected int _NumRetries
        {
            get { return (InputParams as ITaskActionInputParams).NumRetries; }
        }

        protected int _RetryIntervalMs
        {
            get { return (InputParams as ITaskActionInputParams).RetryIntervalMs; }
        }

        #endregion PRIVATE PROPERTIES

        #region PUBLIC PROPERTIES

        /// <summary>
        /// La id única de la tarea, con la que WorkflowCore puede identificarla frente a otros steps
        /// </summary>
        public string Id { get; private set; }

        private int _NumExecutions = 0;
        /// <summary>
        /// La cantidad de veces que se ha ejecutado
        /// </summary>
        public int NumExecutions
        {
            get
            {
                return this._NumExecutions;
            }
            private set { this._NumExecutions = value; }
        }

        /// <summary>
        /// El nombre con el que se llama la tarea
        /// </summary>
        //En esencia la Id, pero le damos este nombre para legibilidad
        public string CallName
        {
            get
            {
                return this.Id;
            }
        }

        /// <summary>
        /// La categoría en la que está la tarea atómica. Empleado para agrupar
        /// </summary>
        public readonly string Category;

        /// <summary>
        /// Los parámetros de entrada para la tarea
        /// </summary>
        public TaskActionInputParamsType InputParams { get; set; }

        /// <summary>
        /// Los parámetros de salida para la tarea
        /// </summary>
        protected TaskActionOutputParamsType _OutputParams { get; set; }

        #endregion PUBLIC PROPERTIES

        #endregion PROPERTIES



        #region METHODS

        #region CONSTRUCTOR

        /// <summary>
        /// Constructor base de la tarea atómica.
        /// Todos los nombres y descripciones de los idiomas son obligatorios.
        /// </summary>
        /// <param name="callName">Nombre de la tarea.</param>
        /// <param name="category">Categoría de la tarea.</param>
        /// <param name="esName">Nombre en español.</param>
        /// <param name="esDescription">Descripción en español.</param>
        /// <param name="glName">Nombre en gallego.</param>
        /// <param name="glDescription">Descripción en gallego.</param>
        /// <param name="enName">Nombre en inglés.</param>
        /// <param name="enDescription">Descripción en inglés.</param>
        protected TaskActionBase(
            string callName,
            string category,
            string esName,
            string esDescription,
            string glName,
            string glDescription,
            string enName,
            string enDescription)
        {
            Id = callName;
            Category = category;

            _ES_Name = esName;
            _ES_Description = esDescription;
            _GL_Name = glName;
            _GL_Description = glDescription;
            _EN_Name = enName;
            _EN_Description = enDescription;
        }

        protected TaskActionBase(string callName)
            : this(callName, "", "", "", "", "", "", "")
        { }

        #endregion CONSTRUCTOR

        #region STATIC METHODS

        #region STATIC PRIVATE METHODS
        #endregion STATIC PRIVATE METHODS
        #region STATIC PUBLIC METHODS

        #endregion STATIC PUBLIC METHODS

        #endregion STATIC METHODS

        #region PUBLIC METHODS

        public abstract object Clone();


        /// <summary>
        /// La compensación que la tarea atómica realiza cuando ha fracasado al ejecutarse
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract ExecutionResult Compensate(IStepExecutionContext context);

        public string GetName(SupportedLanguage language)
        {
            switch (language)
            {
                case SupportedLanguage.SPANISH:
                    return _ES_Name;
                case SupportedLanguage.GALICIAN:
                    return _GL_Name;
                case SupportedLanguage.ENGLISH:
                    return _EN_Name;
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), $"Idioma no soportado: {language}");
            }
        }

        public string GetDescription(SupportedLanguage language)
        {
            switch (language)
            {
                case SupportedLanguage.SPANISH:
                    return _ES_Description;
                case SupportedLanguage.GALICIAN:
                    return _GL_Description;
                case SupportedLanguage.ENGLISH:
                    return _EN_Description;
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), $"Idioma no soportado: {language}");
            }
        }

        /// <summary>
        /// Obtener los resultados de la acción una vez se ha ejecutado
        /// </summary>
        public abstract TaskActionOutputParamsType GetResult();

        /// <summary>
        /// Registra la tarea atómica en unos servicios utilizables por Workflow Core
        /// </summary>
        /// <param name="services"></param>
        public abstract void RegisterAtServices(IServiceCollection services);

        /// <summary>
        /// Ejecutar la tarea atómica
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        //No permitimos que se sobrescriba para controlar adecuadametne el número de ejecuciones
        public sealed override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            this.NumExecutions++;

            if (!this.areInputParamsValid())
            {
                throw new InvalidTaskActionsInputParamsException(this.CallName);
            }

            ExecutionResult result = null;
            //El primer intento + reintentos
            int numRetries = this._NumRetries;
            int intervalRetryMs = this._RetryIntervalMs;
            int totalAttempts = this._NumRetries + 1;

            for (int numTry = 1; numTry <= totalAttempts; numTry++)
            {
                try
                {
                    if (numTry > 1)
                    {
                        this.logInfo($"Reintento {numTry - 1} de {numRetries}");
                    }

                    result = await this.internalRun(context);

                    if (numTry > 1)
                    {
                        this.logInfo($"Tarea exitosa tras {numTry - 1} reintentos");
                    }

                    break;
                }
                catch (Exception ex)
                {
                    this.logException(ex);

                    // Si no es el último intento, esperar y continuar
                    if (numTry < totalAttempts)
                    {
                        this.logWarn($"Esperando {intervalRetryMs}ms antes de reintentar...");
                        await Task.Delay(intervalRetryMs);
                    }
                    else
                    {
                        // Último intento: relanzar excepción para que StepError lo capture
                        this.logError($"Tarea fallida tras {totalAttempts} intentos");
                        throw;
                    }
                }
            }

            return result;
        }

        #endregion PUBLIC METHODS

        #region PRIVATE METHODS

        /// <summary>
        /// El comportamiento de ejecución que toda acción debe definir
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract Task<ExecutionResult> internalRun(IStepExecutionContext context);

        /// <summary>
        /// Comprueba que los parámetros de input son válidos
        /// </summary>
        /// <returns>True si son válidos. False en caso contrario</returns>
        protected abstract bool areInputParamsValid();

        /// <summary>
        /// Mensaje de info en en el logger específico de la tarea atómica
        /// </summary>
        /// <param name="message"></param>
        /// <param name="methodName"></param>
        protected abstract void logInfo(string message, [CallerMemberName] string methodName = "");

        /// <summary>
        /// Publica de la clase un mensaje de información
        /// </summary>
        /// <param name="message">El mensaje</param>
        protected static void logInfo(string message, string taskActionClass, string methodName)
        {
#pragma warning disable S3236 // Caller information arguments should not be provided explicitly
            //De esta manera evitamos que el nombre sea siempre TaskActionBase y se use el de la especialización
            Logger.LogInfo(message, taskActionClass, methodName);
#pragma warning restore S3236 // Caller information arguments should not be provided explicitly
        }

        /// <summary>
        /// Mensaje de warn en en el logger específico de la tarea atómica
        /// </summary>
        /// <param name="message"></param>
        /// <param name="methodName"></param>
        protected abstract void logWarn(string message, [CallerMemberName] string methodName = "");

        /// <summary>
        /// Publica de la clase un mensaje de advertencia
        /// </summary>
        /// <param name="message">El mensaje</param>
        protected static void logWarn(string message, string taskActionClass, string methodName)
        {
#pragma warning disable S3236 // Caller information arguments should not be provided explicitly
            //De esta manera evitamos que el nombre sea siempre TaskActionBase y se use el de la especialización
            Logger.LogWarn(message, taskActionClass, methodName);
#pragma warning restore S3236 // Caller information arguments should not be provided explicitly
        }

        /// <summary>
        /// Mensaje de error en en el logger específico de la tarea atómica
        /// </summary>
        /// <param name="message"></param>
        /// <param name="methodName"></param>
        protected abstract void logError(string message, [CallerMemberName] string methodName = "");

        /// <summary>
        /// Publica de la clase un mensaje de error
        /// </summary>
        /// <param name="message">El mensaje</param>
        protected static void logError(string message, string taskActionClass, string methodName)
        {
#pragma warning disable S3236 // Caller information arguments should not be provided explicitly
            //De esta manera evitamos que el nombre sea siempre TaskActionBase y se use el de la especialización
            Logger.LogError(message, taskActionClass, methodName);
#pragma warning restore S3236 // Caller information arguments should not be provided explicitly
        }

        /// <summary>
        /// Mensaje de error en en el logger específico de la tarea atómica
        /// </summary>
        /// <param name="message"></param>
        /// <param name="methodName"></param>
        protected abstract void logException(Exception exception, [CallerMemberName] string methodName = "");

        /// <summary>
        /// Publica de la clase un mensaje de excepción
        /// </summary>
        /// <param name="message">El mensaje</param>
        protected static void logException(Exception exception, string taskActionClass, string methodName)
        {
#pragma warning disable S3236 // Caller information arguments should not be provided explicitly
            //De esta manera evitamos que el nombre sea siempre TaskActionBase y se use el de la especialización
            Logger.LogException(exception, taskActionClass, methodName);
#pragma warning restore S3236 // Caller information arguments should not be provided explicitly
        }

        #endregion PRIVATE METHODS

        #endregion METHODS
    }
}
