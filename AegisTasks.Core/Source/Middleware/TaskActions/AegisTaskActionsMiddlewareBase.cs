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
    /// La estructura básica que todo middleware de TaskAction de Aegis debe cumplir.
    /// Consiste en un comportamiento que se ejecuta en una fase concreta del proceso de ejecución de una tarea atómica/paso. Idealmente se tratarán de comportamientos genéricos
    /// </summary>
    public abstract class AegisTaskActionsMiddlewareBase: IWorkflowStepMiddleware
    {
        #region PUBLIC PROPERTIES

        #endregion PUBLIC PROPERTIES


        #region CONSTRUCTOR

        #endregion CONSTRUCTOR


        #region PUBLIC METHODS

        /// <summary>
        /// El método llamado por Workflow Core para ejecutar el middleware
        /// </summary>
        /// <param name="workflow"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task<ExecutionResult> HandleAsync(IStepExecutionContext context, IStepBody body, WorkflowStepDelegate next)
        {
            // Ejecutar la lógica interna del middleware
            await internalHandleAsync(context, body, next);

            // Asegurar que siempre se continúa con la cadena de middlewares
            return await next();
        }

        public abstract void RegisterAtServices(IServiceCollection services);

        #endregion PUBLIC METHODS

        protected abstract Task internalHandleAsync(IStepExecutionContext context, IStepBody body, WorkflowStepDelegate next);


        #region PRIVATE METHODS

        #endregion PRIVATE METHODS
    }
}
