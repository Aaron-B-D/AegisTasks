using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Models;

namespace AegisTasks.Core.Events
{
    /// <summary>
    /// Delegado para eventos relacionados con acciones de tareas (steps)
    /// </summary>
    public delegate void TaskActionEventHandler(object sender, TaskActionEventArgs e);

    /// <summary>
    /// Argumentos de evento para acciones de tareas (steps)
    /// </summary>
    public class TaskActionEventArgs : EventArgs
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
        /// ID del step actual
        /// </summary>
        public string StepId { get; set; }

        public string StepName { get; set; }


        public TaskActionEventArgs()
        {
        }

        public TaskActionEventArgs(WorkflowInstance workflowInstance)
        {
            WorkflowInstance = workflowInstance;
            WorkflowInstanceId = workflowInstance?.Id;
        }

        public TaskActionEventArgs(WorkflowInstance workflowInstance, string stepId, string stepName)
        {
            WorkflowInstance = workflowInstance;
            WorkflowInstanceId = workflowInstance?.Id;
            StepId = stepId;
            StepName = stepName;
        }
    }
}
