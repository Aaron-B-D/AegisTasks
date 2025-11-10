using AegisTasks.Core;
using AegisTasks.Core.Common;
using AegisTasks.Core.TaskAction;
using AegisTasks.Core.TaskPlan;
using AegisTasks.Core.WorkflowHost;
using AegisTasks.TasksLibrary.TaskAction;
using AegisTasks.TasksLibrary.TaskPlan;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AegisTasks.TasksLibrary.WorkflowHost
{
    /// <summary>
    /// Clase que encapsula un WorkflowHost de WorkflowCore preconfigurado.
    /// Registra automáticamente los Steps y Workflows definidos en la librería por defecto, permitiendo iniciar y ejecutar workflows sin necesidad de configuración adicional.
    /// </summary>
    public class PreconfiguredAegisWorkflowsManager : AegisWorkflowsManager
    {
        #region CONSTANTS

        #region PRIVATE CONSTANTS

        #endregion PRIVATE CONSTANTS

        #region PUBLIC CONSTANTS

        #endregion PUBLIC CONSTANTS

        #endregion CONSTANTS


        #region PROPERTIES

        #region STATIC PROPERTIES

        #region PRIVATE STATIC PROPERTIES

        #endregion PRIVATE STATIC PROPERTIES

        #region PUBLIC STATIC PROPERTIES

        #endregion PUBLIC STATIC PROPERTIES

        #endregion STATIC PROPERTIES

        #region PRIVATE PROPERTIES

        #endregion PRIVATE PROPERTIES

        #region PUBLIC PROPERTIES

        #endregion PUBLIC PROPERTIES

        #endregion PROPERTIES


        #region CONSTRUCTOR

        public PreconfiguredAegisWorkflowsManager () {
            this.AddTaskActions(
                new HashSet<ITaskActionRegistrable>
                {
                    new CreateDirectoryTaskAction(),
                    new CreateFileTaskAction(),
                    new DirectoryExistTaskAction(),
                    new FileExistTaskAction(),
                    new IsFileReadableTaskAction(),
                    new IsFileWritableTaskAction(),
                    new WriteInFileTaskAction(),
                    new CopyFileTaskAction(),
                    new ScanDirectoryTaskAction()
                }
            );

            // Registrar las TaskPlans
            this.AddTaskPlans(
                new HashSet<ITaskPlanRegistrable>
                {
                    new WriteInFilePlan(),
                    new CopyDirectoryPlan()
                }
            );
        }

        #endregion CONSTRUCTOR


        #region METHODS

        #region STATIC METHODS

        #region PRIVATE STATIC METHODS

        #endregion PRIVATE STATIC METHODS

        #region PUBLIC STATIC METHODS

        #endregion PUBLIC STATIC METHODS

        #endregion STATIC METHODS

        #region PRIVATE METHODS

        #endregion PRIVATE METHODS

        #region PUBLIC METHODS

        #endregion PUBLIC METHODS

        #endregion METHODS
    }
}
