using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AegisTasks.Core.Middleware
{
    /// <summary>
    /// La estructura básica que todo middleware de TaskPlan de Aegis debe cumplir.
    /// Consiste en un comportamiento que se ejecuta en una fase concreta del proceso de ejecución de un flujo de trabajo / plan de acción. Idealmente se tratarán de comportamientos genéricos
    /// </summary>
    public abstract class AegisTaskPlansMiddlewareBase : IWorkflowMiddleware
    {

        #region PRIVATE PROPERTIES

        #endregion PRIVATE PROPERTIES

        #region PUBLIC PROPERTIES

        private readonly WorkflowMiddlewarePhase _Phase;
        /// <summary>
        /// La fase en la que se ejecuta el middleware (antes/después/durante la ejecución de un workflow)
        /// </summary>
        public WorkflowMiddlewarePhase Phase
        {
            get { return _Phase; }
        }

        #endregion PUBLIC PROPERTIES


        #region CONSTRUCTOR

        protected AegisTaskPlansMiddlewareBase(WorkflowMiddlewarePhase phase)
        {
            _Phase = phase;
        }

        #endregion CONSTRUCTOR


        #region PUBLIC METHODS

        /// <summary>
        /// El método llamado por Workflow Core para ejecutar el middleware
        /// </summary>
        /// <param name="workflow"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task HandleAsync(WorkflowInstance workflow, WorkflowDelegate next)
        {
            // Ejecutar la lógica interna del middleware
            await internalHandleAsync(workflow, next);

            // Asegurar que siempre se continúa con la cadena de middlewares
            await next();
        }

        public abstract void RegisterAtServices(IServiceCollection services);

        #endregion PUBLIC METHODS

        /// <summary>
        /// El comportamiento que debe ejecutar el middleware antes de pasar al siguiente
        /// </summary>
        /// <param name="workflow"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        protected abstract Task internalHandleAsync(WorkflowInstance workflow, WorkflowDelegate next);


        #region PRIVATE METHODS

        #endregion PRIVATE METHODS




    }
}
