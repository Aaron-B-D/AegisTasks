using AegisTasks.BLL.Common;
using AegisTasks.TasksLibrary.TaskPlan;
using System;
using System.Text.Json;

namespace AegisTasks.DataAccess.Common.DTO
{
    /// <summary>
    /// DTO que representa una plantilla de TaskPlan almacenada en base de datos.
    /// </summary>
    public class TemplateDTO: ICloneable
    {
        public TemplateDTO(string workflowId, string workflowVersion, string createdBy, string name, string description, string inputParametersJson)
        {
            WorkflowId = workflowId;
            WorkflowVersion = workflowVersion;
            CreatedBy = createdBy;
            Name = name;
            Description = description;
            InputParametersJson = inputParametersJson;
        }

        public TemplateDTO(int id, string workflowId, string workflowVersion, string createdBy, string name, string description, string inputParametersJson, bool active, DateTime createdAt)
        {
            Id = id;
            WorkflowId = workflowId;
            WorkflowVersion = workflowVersion;
            CreatedBy = createdBy;
            Name = name;
            Description = description;
            InputParametersJson = inputParametersJson;
            Active = active;
            CreatedAt = createdAt;
        }

        public TemplateDTO()
        {
        }

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

        public object Clone()
        {
            // Clonación profunda del objeto
            var clone = new TemplateDTO
            {
                Id = this.Id,
                WorkflowId = this.WorkflowId,
                WorkflowVersion = this.WorkflowVersion,
                CreatedBy = this.CreatedBy,
                Name = this.Name,
                Description = this.Description,
                Active = this.Active,
                CreatedAt = this.CreatedAt
            };

            // Clonación profunda del JSON de parámetros
            if (!string.IsNullOrWhiteSpace(this.InputParametersJson))
            {
                // Deserializa según el tipo, lo vuelve a serializar y almacena en el clon
                var inputParams = this.GetInputParameters();
                clone.SetInputParameters(inputParams);
            }

            return clone;
        }

    }
}
