using System;

namespace AegisTasks.Core.DTO
{
    public class ExecutionHistoryDTO
    {
        /// <summary>
        /// Identificador único del registro.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador del flujo de trabajo asociado.
        /// </summary>
        public string WorkflowId { get; set; }

        /// <summary>
        /// Nombre del flujo de trabajo.
        /// </summary>
        public string WorkflowName { get; set; }

        /// <summary>
        /// Fecha y hora de inicio de la ejecución.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Fecha y hora de finalización de la ejecución.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Indica si la ejecución fue exitosa.
        /// </summary>
        public bool Success { get; set; }
    }
}