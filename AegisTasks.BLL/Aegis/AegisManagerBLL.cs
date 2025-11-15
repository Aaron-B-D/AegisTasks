using AegisTasks.Core.Common;
using AegisTasks.Core.Events;
using AegisTasks.Core.TaskPlan;
using AegisTasks.TasksLibrary.TaskPlan;
using AegisTasks.TasksLibrary.WorkflowHost;
using System;
using System.Collections.Generic;
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

            // Arranca el manager
            _Manager.Start();
        }

        private static async Task<string> startWorkflowAndWait<TInput>(string workflowName, TInput inputParams)
        {
            // Inicia el workflow y obtiene su ID inmediatamente
            string workflowId = await _Manager.StartWorkflow(workflowName, inputParams);

            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

            // Handler para cuando el workflow concluya
            void Handler(object sender, TaskPlanEventArgs e)
            {
                if (e.WorkflowInstanceId == workflowId)
                {
                    tcs.TrySetResult(e.WorkflowInstanceId);
                    _Manager.TaskPlanConcluded -= Handler;
                }
            }

            _Manager.TaskPlanConcluded += Handler;

            return await tcs.Task;
        }


        public static Task<string> ExecuteWriteInPlan(WriteInFilePlanInputParams inputParams)
        {
            return startWorkflowAndWait(WriteInFilePlan.CALL_NAME, inputParams);
        }

        public static Task<string> ExecuteCopyDirectoryPlan(CopyDirectoryPlanInputParams inputParams)
        {
            return startWorkflowAndWait(CopyDirectoryPlan.CALL_NAME, inputParams);
        }

        public static HashSet<ITaskPlanRegistrable> GetAvailableTaskPlans()
        {
            return _Manager.GetAvailableTaskPlans();
        }
    }
}
