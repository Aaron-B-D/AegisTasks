using AegisTasks.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;

namespace AegisTasks.Core.TaskPlan
{
    /// <summary>
    /// Define una macro tarea compuesta por una serie de acciones predefinidas de antemano. Un plan de acción. Equivalente al workflow de Workflow Core
    /// </summary>
    /// <typeparam name="TData">Tipo de datos que fluye por el workflow</typeparam>
    public abstract class TaskPlanBase<TaskPlanBaseInputParams> : ITaskPlanRegistrable, IWorkflow<TaskPlanBaseInputParams> where TaskPlanBaseInputParams : class, new()
    {
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

        #endregion PRIVATE PROPERTIES

        #region PUBLIC PROPERTIES

        /// <summary>
        /// El nombre con el que WorkFlowCore identifica la tarea para su llamada
        /// </summary>
        //Coincide, en esencia, con la Id. Solo damos propiedad readonly para comodidad
        public string CallName
        {
            get { return this.Id; }
        }

        /// <summary>
        /// La categoría de la tarea, empleada para agrupar tareas
        /// </summary>
        public readonly string Category;

        /// <summary>
        /// La id única de la tarea
        /// </summary>
        public string Id { get; }


        /// <summary>
        /// La versión en la que se encuentra el plan. Exigido por WorflowCore y útil para cambios futuros
        /// </summary>
        public int Version { get; }

        /// <summary>
        /// Nombre de la tarea (que se mostrará en la interfaz gráfica)
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// La descripción de la tarea (que se mostrará en la interfaz gráfica)
        /// </summary>
        public string Description { get; set; }

        #endregion PUBLIC PROPERTIES

        #endregion PROPERTIES

        #region METHODS

        #region CONSTRUCTOR

        protected TaskPlanBase(string category, int version, string callName, string name, string description) {
            this.Category = category;
            this.Id = callName;
            this.Version = version;
            this.Name = name;
            this.Description = description;
        }

        protected TaskPlanBase(int version, string callName, string name, string description) : this("", version, callName, name, description)
        {}

        #endregion CONSTRUCTOR

        #region STATIC METHODS

        #region STATIC PRIVATE METHODS
        #endregion STATIC PRIVATE METHODS
        #region STATIC PUBLIC METHODS

        #endregion STATIC PUBLIC METHODS

        #endregion STATIC METHODS

        #region PUBLIC METHODS

        /// <summary>
        /// Método llamado por WorkflowCore para definir los pasos que se ejecutan en un WorkFlow
        /// </summary>
        /// <param name="builder"></param>
        public abstract void Build(IWorkflowBuilder<TaskPlanBaseInputParams> builder);

        public abstract object Clone();

        /// <summary>
        /// Permite registrar el plan de acción en un host de workflow core. Debe establecerse en cada uno para que el tipo sea el adecuado
        /// </summary>
        /// <param name="host"></param>
        public abstract void RegisterAtHost(IWorkflowHost host);

        #endregion PUBLIC METHODS

        #region PRIVATE METHODS

        /// <summary>
        /// Genera una Id única basada en la marca temporal actual.
        /// Combina el nombre del plan, el nombre del paso y los ticks del reloj del sistema.
        /// </summary>
        /// <param name="name">Nombre a usar, normalmente el nombre del step (por ejemplo, "CreateFile" o "WriteInFile")</param>
        /// <returns>Id única legible y cronológicamente ordenable</returns>
        protected string generateUniqueId(string name)
        {
            long ticks = DateTime.UtcNow.Ticks;
            return $"{this.CallName}_{name}_{ticks}";
        }

        #endregion PRIVATE METHODS

        #endregion METHODS

    }
}
