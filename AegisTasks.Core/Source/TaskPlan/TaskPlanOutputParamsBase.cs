using AegisTasks.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AegisTasks.Core.TaskPlan
{
	/// <summary>
	/// Las base de cualquier salida de un plan/workflow
	/// </summary>
	public abstract class TaskPlanOutputParamsBase<ResultType>
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
        /// En caso de fracaso en la ejecución del workflow, aquí se almacena la excepción que ha provocado el problema
        /// </summary>
        public Exception FailException { get; set; }

        /// <summary>
        /// Si se ha conseguido ejecutar el workflow exitosamente o no
        /// </summary>
        public bool Success { get; set; } = false;

        #endregion PUBLIC PROPERTIES

        #endregion PROPERTIES



        #region METHODS

        #region CONSTRUCTOR

        protected TaskPlanOutputParamsBase()
        {}

		#endregion CONSTRUCTOR

		#region STATIC METHODS

		#region STATIC PRIVATE METHODS
		#endregion STATIC PRIVATE METHODS
		#region STATIC PUBLIC METHODS
		#endregion STATIC PUBLIC METHODS

		#endregion STATIC METHODS

		#region PUBLIC METHODS

		public abstract object Clone();

        public abstract ResultType GetResult();

        #endregion PUBLIC METHODS

        #region PRIVATE METHODS
        #endregion PRIVATE METHODS

        #endregion METHODS

    }
}
