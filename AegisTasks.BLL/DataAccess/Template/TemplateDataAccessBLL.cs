using AegisTasks.BLL.Common;
using AegisTasks.DataAccess.Common.DTO;
using AegisTasks.DataAccess.DataAccesses;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace AegisTasks.BLL.DataAccess
{
    /// <summary>
    /// Capa BLL para Templates. Convierte los datos crudos del DataAccess en TemplateDTO
    /// y gestiona la serialización de los parámetros de entrada.
    /// </summary>
    public class TemplateDataAccessBLL
    {
        private readonly TemplatesAccess _templatesAccess;

        public TemplateDataAccessBLL()
        {
            _templatesAccess = new TemplatesAccess();
        }

        /// <summary>
        /// Inserta un nuevo template y devuelve su Id.
        /// </summary>
        public int InsertTemplate(SqlConnection conn, TemplateDTO template)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            // Convertir los InputParams a JSON si no lo están
            string inputJson = template.InputParametersJson;
            if (template.InputParametersJson == null && template.GetInputParameters() != null)
            {
                template.SetInputParameters(template.GetInputParameters());
                inputJson = template.InputParametersJson;
            }

            return _templatesAccess.InsertTemplate(
                conn,
                template.WorkflowId,
                template.WorkflowVersion,
                template.CreatedBy.ToLowerInvariant(),
                template.Name,
                template.Description,
                inputJson
            );
        }

        /// <summary>
        /// Actualiza un template existente.
        /// </summary>
        public bool UpdateTemplate(SqlConnection conn, TemplateDTO template)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            string inputJson = template.InputParametersJson;
            if (template.InputParametersJson == null && template.GetInputParameters() != null)
            {
                template.SetInputParameters(template.GetInputParameters());
                inputJson = template.InputParametersJson;
            }

            return _templatesAccess.UpdateTemplate(
                conn,
                template.Id,
                template.CreatedBy.ToLowerInvariant(),
                template.Name,
                template.Description,
                inputJson
            );
        }

        /// <summary>
        /// Marca un template como inactivo (soft delete).
        /// </summary>
        public bool DeleteTemplate(SqlConnection conn, int templateId, string username)
        {
            return _templatesAccess.DeleteTemplate(conn, templateId, username);
        }

        /// <summary>
        /// Obtiene todas las plantillas activas de un usuario
        /// </summary>
        public List<TemplateDTO> GetTemplates(SqlConnection conn, string username)
        {
            List<Dictionary<string, object>> templatesRaw = _templatesAccess.GetTemplates(conn, username);
            List<TemplateDTO> list = new List<TemplateDTO>();

            foreach (Dictionary<string, object> record in templatesRaw)
            {
                TemplateDTO dto = new TemplateDTO
                {
                    Id = (int)record[TemplatesAccess.DB_TEMPLATES_TABLE_FIELD_ID],
                    WorkflowId = (string)record[TemplatesAccess.DB_TEMPLATES_TABLE_FIELD_WORKFLOWID],
                    WorkflowVersion = (string)record[TemplatesAccess.DB_TEMPLATES_TABLE_FIELD_WORKFLOWVERSION],
                    CreatedBy = (string)record[TemplatesAccess.DB_TEMPLATES_TABLE_FIELD_CREATEDBY],
                    Name = (string)record[TemplatesAccess.DB_TEMPLATES_TABLE_FIELD_NAME],
                    Description = record[TemplatesAccess.DB_TEMPLATES_TABLE_FIELD_DESCRIPTION]?.ToString(),
                    InputParametersJson = record[TemplatesAccess.DB_TEMPLATES_TABLE_FIELD_INPUTPARAMETERS]?.ToString(),
                    Active = (bool)record[TemplatesAccess.DB_TEMPLATES_TABLE_FIELD_ACTIVE],
                    CreatedAt = (DateTime)record[TemplatesAccess.DB_TEMPLATES_TABLE_FIELD_CREATEDAT]
                };

                list.Add(dto);
            }

            return list;
        }
    }
}
