using System;
using System.Runtime.Serialization;

namespace AegisTasks.Core.TaskAction
{
    /// <summary>
    /// Excepción lanzada cuando se intenta obtener el resultado de una acción que aún no ha sido ejecutada.
    /// </summary>
    [Serializable]
    public sealed class NotExecutedTaskActionException : Exception
    {
        /// <summary>
        /// El nombre de la tarea que no ha sido ejecutada.
        /// </summary>
        public string TaskName { get; }

        /// <summary>
        /// Inicializa una nueva instancia de la excepción con el nombre de la tarea especificado.
        /// </summary>
        public NotExecutedTaskActionException(string taskName)
            : base($"La tarea '{taskName}' no ha sido ejecutada y no se pueden obtener resultados.")
        {
            TaskName = taskName;
        }

        /// <summary>
        /// Inicializa una nueva instancia de la excepción con el nombre de la tarea y una excepción interna.
        /// </summary>
        public NotExecutedTaskActionException(string taskName, Exception innerException)
            : base($"La tarea '{taskName}' no ha sido ejecutada y no se pueden obtener resultados.", innerException)
        {
            TaskName = taskName;
        }

        /// <summary>
        /// Constructor protegido usado para la deserialización de la excepción.
        /// </summary>
        private NotExecutedTaskActionException(SerializationInfo info, StreamingContext context)
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
        }
    }
}
