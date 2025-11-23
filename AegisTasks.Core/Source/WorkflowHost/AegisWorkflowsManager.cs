using AegisTasks.Core.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using System.Threading.Tasks;
using AegisTasks.Core.TaskAction;
using AegisTasks.Core.TaskPlan;
using WorkflowCore.Models.LifeCycleEvents;
using AegisTasks.Core.Middleware;
using AegisTasks.Core.Events;

namespace AegisTasks.Core.WorkflowHost
{
    /// <summary>
    /// La clase responsable de recibir y ejecutar los flujos de trabajo/plan que tiene disponibles
    /// </summary>
    public class AegisWorkflowsManager
    {

        #region EVENTS

        /// <summary>
        /// Evento emitido cuando un workflow ha completado correctamente
        /// </summary>
        public event TaskPlanEventHandler TaskPlanCompleted;

        public event TaskPlanEventHandler TaskPlanStarted;

        /// <summary>
        /// Evento emitido cuando un workflow ha terminado abruptamente
        /// </summary>
        public event TaskPlanEventHandler TaskPlanTerminated;

        /// <summary>
        /// Evento emitido cuando un workflow ha concluido (independientemente del estado)
        /// </summary>
        public event TaskPlanEventHandler TaskPlanConcluded;

        /// <summary>
        /// Evento emitido cuando un workflow se ha reanudado
        /// </summary>
        public event TaskPlanEventHandler TaskPlanResumed;

        /// <summary>
        /// Evento emitido cuando un workflow se ha suspendido
        /// </summary>
        public event TaskPlanEventHandler TaskPlanSuspended;

        /// <summary>
        /// Evento emitido cuando un step empieza
        /// </summary>
        public event TaskActionEventHandler TaskActionStarted;

        /// <summary>
        /// Evento emitido cuando un step termina
        /// </summary>
        public event TaskActionEventHandler TaskActionCompleted;

        #endregion EVENTS



        #region PROPERTIES

        #region CONSTANTS

        #region PRIVATE CONSTANTS
        #endregion PRIVATE CONSTANTS
        #region PUBLIC CONSTANTS
        #endregion PUBLIC CONSTANTS

        #endregion CONSTANTS

        #region STATIC PROPERTIES
        #region STATIC PRIVATE PROPERTIES

        #endregion STATIC PRIVATE PROPERTIES
        #region STATIC PUBLIC PROPERTIES
        #endregion STATIC PUBLIC PROPERTIES
        #endregion STATIC PROPERTIES

        #region PRIVATE PROPERTIES

        /// <summary>
        /// La lista de tareas atómicas que se registrarán en el host para ser usadas. No puede haber duplicados
        /// </summary>
        private HashSet<ITaskActionRegistrable> _TaskActions = new HashSet<ITaskActionRegistrable>();

        /// <summary>
        /// La lista de planes de tarea que se registrarán en el host para ser usadas
        /// </summary>
        private HashSet<ITaskPlanRegistrable> _TaskPlans = new HashSet<ITaskPlanRegistrable>();

        /// <summary>
        /// La lista de middlewares que se registrarán en el host para ser usados
        /// </summary>
        private HashSet<AegisTaskPlansMiddlewareBase> _TaskPlansMiddlewares = new HashSet<AegisTaskPlansMiddlewareBase>
        {};

        /// <summary>
        /// La lista de middlewares de acciones que se registrarán en el host para ser usados
        /// </summary>
        private HashSet<AegisTaskActionsMiddlewareBase> _TaskActionsMiddlewares = new HashSet<AegisTaskActionsMiddlewareBase>
        {};

        /// <summary>
        /// Los servicios
        /// </summary>
        private ServiceCollection _Services = null;

        #endregion PRIVATE PROPERTIES

        #region PUBLIC PROPERTIES


        private IWorkflowHost _Host;
        /// <summary>
        /// Obtiene el WorkflowHost de WorkflowCore.
        /// </summary>
        public IWorkflowHost Host
        {
            get
            {
                return _Host;
            }
        }

        #endregion PUBLIC PROPERTIES

        #endregion PROPERTIES



        #region METHODS

        #region CONSTRUCTOR

        public AegisWorkflowsManager()
        {}

        #endregion CONSTRUCTOR

        #region STATIC METHODS

        #region STATIC PRIVATE METHODS

        /// <summary>
        /// Registra los steps en el host (las tareas atómicas)
        /// </summary>
        /// <param name="services"></param>
        private static void registerTaskActions(
            IServiceCollection services,
            HashSet<ITaskActionRegistrable> taskActionsToRegister
        )
        {
            foreach (ITaskActionRegistrable taskActionToRegister in taskActionsToRegister)
            {
                taskActionToRegister.RegisterAtServices(services);
            }
        }

        /// <summary>
        /// Registra los workflows/plans en el host
        /// </summary>
        private static void registerTaskPlans(IWorkflowHost host, HashSet<ITaskPlanRegistrable> taskPlans)
        {
            // Nota: Este método necesita ser implementado según la estructura de tus TaskPlan
            // Ya que no se puede usar genéricos directamente con Type en runtime
            foreach (ITaskPlanRegistrable taskPlan in taskPlans)
            {
                taskPlan.RegisterAtHost(host);
            }
        }


        /// <summary>
        /// Registra los middleware de planes de acción en el host
        /// </summary>
        private static void registerTaskPlanMiddlewares(
            IServiceCollection services,
            HashSet<AegisTaskPlansMiddlewareBase> taskPlanMiddlewares
        )
        {
            foreach (AegisTaskPlansMiddlewareBase taskPlanMiddleware in taskPlanMiddlewares)
            {
                taskPlanMiddleware.RegisterAtServices(services);
            }
        }

        /// <summary>
        /// Registra los middleware de acciones/steps en el host
        /// </summary>
        private static void registerTaskActionMiddlewares(
            IServiceCollection services,
            HashSet<AegisTaskActionsMiddlewareBase> taskActionMiddlewares
        )
        {
            foreach (AegisTaskActionsMiddlewareBase taskActionMiddleware in taskActionMiddlewares)
            {
                taskActionMiddleware.RegisterAtServices(services);
            }
        }

        #endregion STATIC PRIVATE METHODS
        #region STATIC PUBLIC METHODS
        #endregion STATIC PUBLIC METHODS

        #endregion STATIC METHODS

        #region PUBLIC METHODS

        /// <summary>
        /// Registra una tarea atómica/task action en el gestor
        /// </summary>
        /// <param name="action"></param>
        public void AddTaskAction(ITaskActionRegistrable taskAction)
        {
            this._TaskActions.Add(taskAction);
        }

        /// <summary>
        /// Añadir una serie de tareas atómicas en el gestor
        /// </summary>
        /// <param name="taskActions"></param>
        public void AddTaskActions(HashSet<ITaskActionRegistrable> taskActions)
        {
            foreach (ITaskActionRegistrable taskAction in taskActions)
            {
                AddTaskAction(taskAction);
            }
        }

        public void AddTaskPlanMiddleware(AegisTaskPlansMiddlewareBase taskPlanMiddleware)
        {
            this._TaskPlansMiddlewares.Add(taskPlanMiddleware);
        }

        public void AddTaskPlanMiddlewares(HashSet<AegisTaskPlansMiddlewareBase> taskPlanMiddlewares)
        {
            foreach (AegisTaskPlansMiddlewareBase taskPlanMiddleware in taskPlanMiddlewares)
            {
                AddTaskPlanMiddleware(taskPlanMiddleware);
            }
        }

        public void AddTaskActionMiddleware(AegisTaskActionsMiddlewareBase taskActionMiddleware)
        {
            this._TaskActionsMiddlewares.Add(taskActionMiddleware);
        }

        public void AddTaskActionMiddlewares(HashSet<AegisTaskActionsMiddlewareBase> taskActionMiddlewares)
        {
            foreach (AegisTaskActionsMiddlewareBase taskActionMiddleware in taskActionMiddlewares)
            {
                AddTaskActionMiddleware(taskActionMiddleware);
            }
        }

        /// <summary>
        /// Registra un plan de tareas en el gestor
        /// </summary>
        /// <param name="taskPlan"></param>
        public void AddTaskPlan(ITaskPlanRegistrable taskPlan)
        {
            this._TaskPlans.Add(taskPlan);
        }

        public void AddTaskPlans(HashSet<ITaskPlanRegistrable> taskPlans)
        {
            foreach(ITaskPlanRegistrable taskPlan in taskPlans)
            {
                AddTaskPlan(taskPlan);
            }
        }

        public HashSet<ITaskPlanRegistrable> GetAvailableTaskPlans()
        {
            return this._TaskPlans;
        }

        /// <summary>
        /// Arrancar el host para ser usado
        /// </summary>
        public void Start()
        {
            if (_Host is null)
            {
                this.buildHost();
                //Se setea al arrancar el manager porque es a partir de este punto cuando se va a empezar a utilizar
                this.setEvents();
            }

            _Host.Start();
        }

        public void Stop()
        {
            if (!(_Host is null))
            {
                _Host.Stop();
            }
        }

        public Task<string> StartWorkflow(string workflowId, object data = null)
        {
            if (!(_Host is null))
            {
                return _Host.StartWorkflow(workflowId, data);

            }
            else
            {
                throw new NullReferenceException(nameof(_Host));
            }
        }

        public Task<bool> StopWorkflow(string workflowId)
        {
            if (!(_Host is null))
            {
                return _Host.SuspendWorkflow(workflowId);

            }
            else
            {
                throw new NullReferenceException(nameof(_Host));
            }
        }


        #endregion PUBLIC METHODS
        #region PRIVATE METHODS


        /// <summary>
        /// Construye el host del manager y lo guarda en la propiedad correspondiente
        /// </summary>
        private void buildHost()
        {
            if (_Services is null)
            {
                this.buildServices();
            }


            this._Host = generateHost(this._Services, this._TaskPlans);
        }

        /// <summary>
        /// Genera los servicios del manager y lo guarda en la propiedad correspondiente
        /// </summary>
        private void buildServices()
        {
            this._Services = generateServices(this._TaskActions, this._TaskPlansMiddlewares, this._TaskActionsMiddlewares);
        }

        /// <summary>
        /// Maneja los eventos del ciclo de vida del workflow y los redirige a los eventos del manager
        /// </summary>
        private async void handleLifeCycleEvent(LifeCycleEvent lifecycleEvent)
        {
            // Obtener el WorkflowInstance completo a partir del ID
            WorkflowInstance workflow = await _Host.PersistenceStore.GetWorkflowInstance(lifecycleEvent.WorkflowInstanceId);

            if (workflow != null)
            {
                if(lifecycleEvent is WorkflowStarted)
                {
                    onTaskPlanStarted(new TaskPlanEventArgs(workflow));
                }
                else if (lifecycleEvent is WorkflowCompleted)
                {
                    onTaskPlanCompleted(new TaskPlanEventArgs(workflow));
                    onTaskPlanConcluded(new TaskPlanEventArgs(workflow));
                }
                else if (lifecycleEvent is WorkflowTerminated)
                {
                    onTaskPlanTerminated(new TaskPlanEventArgs(workflow));
                    onTaskPlanConcluded(new TaskPlanEventArgs(workflow));
                }
                else if (lifecycleEvent is StepCompleted stepCompleted)
                {
                    onTaskActionCompleted(new TaskActionEventArgs(workflow, stepCompleted.StepId.ToString(), workflow.ExecutionPointers.FirstOrDefault(ep => ep.StepId == stepCompleted.StepId).StepName));
                }
                else if (lifecycleEvent is StepStarted stepStarted)
                {
                    onTaskActionStarted(new TaskActionEventArgs(workflow, stepStarted.StepId.ToString(), workflow.ExecutionPointers.FirstOrDefault(ep => ep.StepId == stepStarted.StepId).StepName));
                }
                else if (lifecycleEvent is WorkflowResumed)
                {
                    onTaskPlanResumed(new TaskPlanEventArgs(workflow));
                }
                else if (lifecycleEvent is WorkflowSuspended)
                {
                    onTaskPlanSuspended(new TaskPlanEventArgs(workflow));
                }
            }
            else
            {
                Logger.LogWarn($"No se ha podido recuperar la instancia del Workflow {lifecycleEvent.WorkflowInstanceId}");
            }
        }

        /// <summary>
        /// Dispara el evento TaskPlanCompleted
        /// </summary>
        protected virtual void onTaskPlanCompleted(TaskPlanEventArgs e)
        {
            TaskPlanCompleted?.Invoke(this, e);
        }

        /// <summary>
        /// Dispara el evento TaskPlanStarted
        /// </summary>
        protected virtual void onTaskPlanStarted(TaskPlanEventArgs e)
        {
            TaskPlanStarted?.Invoke(this, e);
        }


        /// <summary>
        /// Dispara el evento TaskPlanTerminated
        /// </summary>
        protected virtual void onTaskPlanTerminated(TaskPlanEventArgs e)
        {
            TaskPlanTerminated?.Invoke(this, e);
        }

        /// <summary>
        /// Dispara el evento TaskPlanConcluded
        /// </summary>
        protected virtual void onTaskPlanConcluded(TaskPlanEventArgs e)
        {
            TaskPlanConcluded?.Invoke(this, e);
        }

        /// <summary>
        /// Dispara el evento TaskPlanResumed
        /// </summary>
        protected virtual void onTaskPlanResumed(TaskPlanEventArgs e)
        {
            TaskPlanResumed?.Invoke(this, e);
        }

        /// <summary>
        /// Dispara el evento TaskPlanSuspended
        /// </summary>
        protected virtual void onTaskPlanSuspended(TaskPlanEventArgs e)
        {
            TaskPlanSuspended?.Invoke(this, e);
        }

        /// <summary>
        /// Dispara el evento TaskActionStarted
        /// </summary>
        protected virtual void onTaskActionStarted(TaskActionEventArgs e)
        {
            TaskActionStarted?.Invoke(this, e);
        }

        /// <summary>
        /// Dispara el evento TaskActionCompleted
        /// </summary>
        protected virtual void onTaskActionCompleted(TaskActionEventArgs e)
        {
            TaskActionCompleted?.Invoke(this, e);
        }

        /// <summary>
        /// Establece los eventos que expone por defecto el manager
        /// </summary>
        private void setEvents()
        {
            this._Host.OnLifeCycleEvent += handleLifeCycleEvent;
        }

        //#region PRIVATE STATIC METHODS

        /// <summary>
        /// Genera un host con una configuración dada
        /// </summary>
        /// <returns></returns>
        private static IWorkflowHost generateHost(ServiceCollection services, HashSet<ITaskPlanRegistrable> taskPlans)
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            IWorkflowHost host = serviceProvider.GetService<IWorkflowHost>();

            if (host is null)
            {
                throw new InvalidOperationException("No se ha podido generar un host de WorkflowCore");
            }
            else
            {
                registerTaskPlans(host, taskPlans);
            }

            return host;
        }

        /// <summary>
        /// Genera los servicio registrando las tareas atómicas dadas
        /// </summary>
        /// <param name="taskActions">Los tipos de las tareas atómicas a registrar</param>
        /// <returns>Los servicios preparados con la configuración pasada</returns>
        private static ServiceCollection generateServices(HashSet<ITaskActionRegistrable> taskActions, HashSet<AegisTaskPlansMiddlewareBase> taskPlanMiddlewares, HashSet<AegisTaskActionsMiddlewareBase> taskActionMiddlewares)
        {
            ServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddWorkflow();

            registerTaskActions(services, taskActions);
            registerTaskPlanMiddlewares(services, taskPlanMiddlewares);
            registerTaskActionMiddlewares(services, taskActionMiddlewares);

            return services;
        }

        //#endregion PRIVATE STATIC METHODS

        #endregion PRIVATE METHODS
        #endregion METHODS

    }
}
