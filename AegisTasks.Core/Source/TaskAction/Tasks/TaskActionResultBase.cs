using AegisTasks.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AegisTasks.Core.TaskAction
{
    /// <summary>
    /// El resultado de una tarea cualquiera
    /// </summary>
    public abstract class TaskActionResultBase: ExecutionResult
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
        /// La id única del resultado de tarea
        /// </summary>
        public long Id { get; set; } = Constants.LONG_ID_UNDEFINED;

        /// <summary>
        /// La id de la tarea propietaria del resultado
        /// </summary>
        public long OwnerId { get; set; } = Constants.LONG_ID_UNDEFINED;

        #endregion PUBLIC PROPERTIES

        #endregion PROPERTIES



        #region METHODS

        #region CONSTRUCTOR
        #endregion CONSTRUCTOR

        #region STATIC METHODS

        #region STATIC PRIVATE METHODS
        #endregion STATIC PRIVATE METHODS
        #region STATIC PUBLIC METHODS
        #endregion STATIC PUBLIC METHODS

        #endregion STATIC METHODS

        #region PUBLIC METHODS
        #endregion PUBLIC METHODS

        #region PRIVATE METHODS

        #endregion PRIVATE METHODS

        #endregion METHODS

    }
}
