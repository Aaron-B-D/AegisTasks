using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AegisTasks.Core.TaskAction
{
    /// <summary>
    /// Excepción que resulta de tratar de ejecutar una tarea sin los parámetros de entrada correctos
    /// </summary>
    public sealed class InvalidTaskActionsInputParamsException : Exception
    {
        /// <summary>
        /// El nombre de la tarea que ha recibido parámetros de entrada inválidos
        /// </summary>
        public string TaskName { get; }

        /// <summary>
        /// Inicializa una nueva instancia de la excepción con el nombre de la tarea especificado.
        /// </summary>
        public InvalidTaskActionsInputParamsException(string taskName)
            : base($"La tarea '{taskName}' no ha recibido los parámetros de entrada necesarios.")
        {
            TaskName = taskName;
        }

        /// <summary>
        /// Inicializa una nueva instancia de la excepción con el nombre de la tarea y una excepción interna.
        /// </summary>
        public InvalidTaskActionsInputParamsException(string taskName, Exception innerException)
            : base($"La tarea '{taskName}' no ha recibido los parámetros de entrada necesarios.", innerException)
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
