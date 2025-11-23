using AegisTasks.Core.Common;
using AegisTasks.Core.Events;
using AegisTasks.Core.TaskPlan;
using AegisTasks.TasksLibrary.TaskPlan;
using AegisTasks.TasksLibrary.WorkflowHost;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AegisTasks.BLL.Aegis
{
    public static class AegisManagerBLL
    {
        // Acceso al manager preconfigurado

        private static readonly PreconfiguredAegisWorkflowsManager _Manager;

        // Constructor estático: inicializa y arranca el manager
        static AegisManagerBLL()
        {
            _Manager = new PreconfiguredAegisWorkflowsManager();
            _Manager.Start();

        }

        private static async void startWorkflowAndWait<TInput>(string workflowName, TInput inputParams, TaskPlanEventHandler taskPlanStarted, TaskPlanEventHandler taskPlanCompleted, TaskPlanEventHandler taskPlanTerminated, TaskActionEventHandler taskActionStarted, TaskActionEventHandler taskActionEnded)
        {

            string workflowId = String.Empty;
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

            _Manager.TaskPlanStarted += taskPlanStarted;
            _Manager.TaskPlanCompleted += taskPlanCompleted;
            _Manager.TaskPlanTerminated += taskPlanTerminated;
            _Manager.TaskActionStarted += taskActionStarted;
            _Manager.TaskActionCompleted += taskActionEnded;

            await _Manager.StartWorkflow(workflowName, inputParams);
        }

        public static void StopWorkflow(string workflowId)
        {
            _Manager.StopWorkflow(workflowId);
        }


        public static void ExecuteWriteInPlan(WriteInFilePlanInputParams inputParams, TaskPlanEventHandler taskPlanStarted, TaskPlanEventHandler taskPlanCompleted, TaskPlanEventHandler taskPlanTerminated, TaskActionEventHandler taskActionStarted, TaskActionEventHandler taskActionEnded)
        {
            startWorkflowAndWait(WriteInFilePlan.CALL_NAME, new WriteInFilePlanParams(inputParams), taskPlanStarted, taskPlanCompleted, taskPlanTerminated, taskActionStarted, taskActionEnded);
        }

        public static void ExecuteCopyDirectoryPlan(CopyDirectoryPlanInputParams inputParams, TaskPlanEventHandler taskPlanStarted, TaskPlanEventHandler taskPlanCompleted, TaskPlanEventHandler taskPlanTerminated, TaskActionEventHandler taskActionStarted, TaskActionEventHandler taskActionEnded)
        {
            startWorkflowAndWait(CopyDirectoryPlan.CALL_NAME, new CopyDirectoryPlanParams(inputParams), taskPlanStarted, taskPlanCompleted, taskPlanTerminated, taskActionStarted, taskActionEnded);
        }

        public static HashSet<ITaskPlanRegistrable> GetAvailableTaskPlans()
        {
            return _Manager.GetAvailableTaskPlans();
        }
    }
}
