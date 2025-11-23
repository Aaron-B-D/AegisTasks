using AegisTasks.BLL.Common;
using AegisTasks.TasksLibrary.TaskPlan;
using System;
using System.Text.Json;

namespace AegisTasks.DataAccess.Common.DTO
{
    /// <summary>
    /// DTO que representa una plantilla de TaskPlan almacenada en base de datos.
    /// </summary>
    public class TemplateDTO
    {
        public int Id { get; set; }
        public string WorkflowId { get; set; }
        public string WorkflowVersion { get; set; }
        public string CreatedBy { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// JSON con los parámetros de entrada del TaskPlan.
        /// </summary>
        public string InputParametersJson { get; set; }

        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Devuelve el tipo de TaskPlan según el WorkflowId
        /// </summary>
        public TaskPlanType GetTaskPlanType()
        {
            if (WorkflowId == WriteInFilePlan.CALL_NAME)
                return TaskPlanType.WRITE_IN_FILE;
            else if (WorkflowId == CopyDirectoryPlan.CALL_NAME)
                return TaskPlanType.COPY_DIRECTORY;
            else
                throw new NotSupportedException(string.Format("WorkflowId '{0}' no soportado.", WorkflowId));
        }

        /// <summary>
        /// Deserializa los parámetros de entrada según el tipo de TaskPlan.
        /// </summary>
        public object GetInputParameters()
        {
            if (WorkflowId == WriteInFilePlan.CALL_NAME)
            {
                return WriteInFilePlanInputParams.FromJson(InputParametersJson);
            }
            else if (WorkflowId == CopyDirectoryPlan.CALL_NAME)
            {
                return CopyDirectoryPlanInputParams.FromJson(InputParametersJson);
            }
            else
            {
                throw new NotSupportedException(string.Format("WorkflowId '{0}' no soportado para InputParameters.", WorkflowId));
            }
        }

        /// <summary>
        /// Convierte los parámetros de entrada a JSON para almacenarlos.
        /// </summary>
        public void SetInputParameters(object inputParams)
        {
            if (inputParams == null) throw new ArgumentNullException(nameof(inputParams));

            if (inputParams is WriteInFilePlanInputParams writeInFile)
            {
                InputParametersJson = writeInFile.ToJson();
            }
            else if (inputParams is CopyDirectoryPlanInputParams copyDirectory)
            {
                InputParametersJson = copyDirectory.ToJson();
            }
            else
            {
                throw new NotSupportedException(string.Format("Tipo de InputParams '{0}' no soportado.", inputParams.GetType().Name));
            }
        }
    }
}
