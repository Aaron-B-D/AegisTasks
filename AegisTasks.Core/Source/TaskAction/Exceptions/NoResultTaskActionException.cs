using System;
using System.Runtime.Serialization;

namespace AegisTasks.Core.TaskAction
{
    /// <summary>
    /// Excepción que resulta de intentar obtener el resultado de una tarea atómica que no está diseñada para devolver resultados.
    /// </summary>
    [Serializable]
    public sealed class NoResultTaskActionException : Exception
    {
        /// <summary>
        /// El nombre de la tarea que no está diseñada para devolver resultados.
        /// </summary>
        public string TaskName { get; }

        /// <summary>
        /// Inicializa una nueva instancia de la excepción con el nombre de la tarea especificado.
        /// </summary>
        public NoResultTaskActionException(string taskName)
            : base($"La tarea '{taskName}' no está diseñada para devolver resultados.")
        {
            TaskName = taskName;
        }

        /// <summary>
        /// Inicializa una nueva instancia de la excepción con el nombre de la tarea y una excepción interna.
        /// </summary>
        public NoResultTaskActionException(string taskName, Exception innerException)
            : base($"La tarea '{taskName}' no está diseñada para devolver resultados.", innerException)
        {
            TaskName = taskName;
        }

        /// <summary>
        /// Constructor protegido usado para la deserialización de la excepción.
        /// </summary>
        private NoResultTaskActionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            TaskName = info.GetString(nameof(TaskName));
        }

        /// <summary>
        /// Serializa la información personalizada de la excepción.
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(TaskName), TaskName);
        }
    }
}
