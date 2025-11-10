using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Models;

namespace AegisTasks.Core.Events
{
    /// <summary>
    /// Delegado para eventos relacionados con planes de tareas (workflows)
    /// </summary>
    public delegate void TaskPlanEventHandler(object sender, TaskPlanEventArgs e);

    /// <summary>
    /// Argumentos de evento para planes de tareas (workflows)
    /// </summary>
    public class TaskPlanEventArgs : EventArgs
    {
        /// <summary>
        /// Instancia del workflow
        /// </summary>
        public WorkflowInstance WorkflowInstance { get; set; }

        /// <summary>
        /// ID del workflow
        /// </summary>
        public string WorkflowInstanceId { get; set; }

        /// <summary>
        /// Estado del workflow
        /// </summary>
        public WorkflowStatus Status { get; set; }

        public TaskPlanEventArgs()
        {
        }

        public TaskPlanEventArgs(WorkflowInstance workflowInstance)
        {
            WorkflowInstance = workflowInstance;
            WorkflowInstanceId = workflowInstance?.Id;
            Status = workflowInstance?.Status ?? WorkflowStatus.Runnable;
        }
    }
}
