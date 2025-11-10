using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;

namespace AegisTasks.Core.Common
{
    /// <summary>
    /// Interfaz que permite registrar TaskPlans sin especificar tipos genéricos concretos.
    /// Esto facilita el almacenamiento de diferentes TaskPlans con distintos tipos genéricos en una misma colección.
    /// </summary>
    public interface ITaskPlanRegistrable
    {
        /// <summary>
        /// Registra el TaskPlan (workflow) en el WorkflowHost de WorkflowCore.
        /// </summary>
        /// <param name="host">El WorkflowHost donde se registrará el TaskPlan</param>
        void RegisterAtHost(IWorkflowHost host);
    }
}
