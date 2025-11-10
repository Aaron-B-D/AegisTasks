using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AegisTasks.Core.TaskAction
{
    /// <summary>
    /// Excepción que resulta de tratar de ejecutar la compensación de una tarea atómica que no tiene definida una compensación
    /// </summary>
    public sealed class NoCompensationTaskActionException: Exception
    {
        /// <summary>
        /// El nombre de la tarea que no está diseñada para compensar
        /// </summary>
        public string TaskName { get; }

        /// <summary>
        /// Inicializa una nueva instancia de la excepción con el nombre de la tarea especificado.
        /// </summary>
        public NoCompensationTaskActionException(string taskName)
            : base($"La tarea '{taskName}' no está diseñada para compensar su comportamiento.")
        {
            TaskName = taskName;
        }

        /// <summary>
        /// Inicializa una nueva instancia de la excepción con el nombre de la tarea y una excepción interna.
        /// </summary>
        public NoCompensationTaskActionException(string taskName, Exception innerException)
            : base($"La tarea '{taskName}' no está diseñada para compensar su comportamiento.", innerException)
        {
            TaskName = taskName;
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
