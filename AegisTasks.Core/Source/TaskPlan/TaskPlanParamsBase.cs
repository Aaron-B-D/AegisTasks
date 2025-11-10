using AegisTasks.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AegisTasks.Core.TaskPlan
{
    public class TaskPlanParamsBase<TaskPlanInputParamsType, TaskPlanInternalParamsType> 
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
        /// Los parámetros de configuración para el plan y que el usuario debería poder manipular
        /// </summary>
        public TaskPlanInputParamsType InputParams {  get; set; }

        /// <summary>
        /// Los parámetros de configuración que se usarán internamente para comunicar entre tareas atómicas que forman parte del plan
        /// </summary>
        /// <param name="type"></param>
        public TaskPlanInternalParamsType InternalParams {  get; set; }


        #endregion PUBLIC PROPERTIES

        #endregion PROPERTIES

        #region METHODS

        #region CONSTRUCTOR

        public TaskPlanParamsBase(TaskPlanInputParamsType inputParams, TaskPlanInternalParamsType internalParams)
        {
            this.InputParams = inputParams;
            this.InternalParams = internalParams;
        }

        #endregion CONSTRUCTOR

        #region STATIC METHODS

        #region STATIC PRIVATE METHODS
        #endregion STATIC PRIVATE METHODS
        #region STATIC PUBLIC METHODS
        #endregion STATIC PUBLIC METHODS

        #endregion STATIC METHODS

        #region PUBLIC METHODS

        public virtual object Clone()
        {
            throw new NotImplementedException();
        }

        #endregion PUBLIC METHODS

        #region PRIVATE METHODS
        #endregion PRIVATE METHODS

        #endregion METHODS

    }
}
