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
    public static class TemplateDataAccessBLL
    {
        private static readonly TemplatesAccess _templatesAccess = new TemplatesAccess();

        static TemplateDataAccessBLL() { }

        /// <summary>
        /// Inserta un nuevo template y devuelve su Id.
        /// </summary>
        public static int InsertTemplate(TemplateDTO template)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            // Convertir los InputParams a JSON si no lo están
            if (template.InputParametersJson == null && template.GetInputParameters() != null)
            {
                template.SetInputParameters(template.GetInputParameters());
            }

            string inputJson = template.InputParametersJson;

            template.CreatedBy = template.CreatedBy.ToLowerInvariant();

            using (SqlConnection conn = DataAccessBLL.CreateConnection())
            {
                conn.Open();
                return _templatesAccess.InsertTemplate(
                    conn,
                    template.WorkflowId,
                    template.WorkflowVersion,
                    template.CreatedBy,
                    template.Name,
                    template.Description,
                    inputJson
                );
            }
        }

        /// <summary>
        /// Actualiza un template existente.
        /// </summary>
        public static bool UpdateTemplate(TemplateDTO template)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            if (template.InputParametersJson == null && template.GetInputParameters() != null)
            {
                template.SetInputParameters(template.GetInputParameters());
            }

            string inputJson = template.InputParametersJson;
            template.CreatedBy = template.CreatedBy.ToLowerInvariant();

            using (SqlConnection conn = DataAccessBLL.CreateConnection())
            {
                conn.Open();
                return _templatesAccess.UpdateTemplate(
                    conn,
                    template.Id,
                    template.CreatedBy,
                    template.Name,
                    template.Description,
                    inputJson
                );
            }
        }

        /// <summary>
        /// Marca un template como inactivo (soft delete).
        /// </summary>
        public static bool DeleteTemplate(int templateId, string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));

            username = username.ToLowerInvariant();

            using (SqlConnection conn = DataAccessBLL.CreateConnection())
            {
                conn.Open();
                return _templatesAccess.DeleteTemplate(conn, templateId, username);
            }
        }

        /// <summary>
        /// Obtiene todas las plantillas activas de un usuario
        /// </summary>
        public static List<TemplateDTO> GetTemplates(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));

            username = username.ToLowerInvariant();

            using (SqlConnection conn = DataAccessBLL.CreateConnection())
            {
                conn.Open();
                List<Dictionary<string, object>> templatesRaw = _templatesAccess.GetTemplates(conn, username);
                List<TemplateDTO> list = new List<TemplateDTO>();

                foreach (var record in templatesRaw)
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
}
