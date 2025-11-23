using AegisTasks.DataAccess.Common;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace AegisTasks.DataAccess.DataAccesses
{
    /// <summary>
    /// Acceso a datos para plantillas de TaskPlans.
    /// Devuelve registros crudos para que la BLL haga la conversión a DTO.
    /// </summary>
    public class TemplatesAccess : AegisDataAccessSqlServerBase
    {
        public static readonly string DB_TEMPLATES_TABLE_NAME = "Templates";
        public static readonly string DB_TEMPLATES_TABLE_FIELD_ID = "Id";
        public static readonly string DB_TEMPLATES_TABLE_FIELD_WORKFLOWID = "WorkflowId";
        public static readonly string DB_TEMPLATES_TABLE_FIELD_WORKFLOWVERSION = "WorkflowVersion";
        public static readonly string DB_TEMPLATES_TABLE_FIELD_CREATEDBY = "CreatedBy";
        public static readonly string DB_TEMPLATES_TABLE_FIELD_NAME = "Name";
        public static readonly string DB_TEMPLATES_TABLE_FIELD_DESCRIPTION = "Description";
        public static readonly string DB_TEMPLATES_TABLE_FIELD_INPUTPARAMETERS = "InputParameters";
        public static readonly string DB_TEMPLATES_TABLE_FIELD_ACTIVE = "Active";
        public static readonly string DB_TEMPLATES_TABLE_FIELD_CREATEDAT = "CreatedAt";

        private readonly string CREATE_TEMPLATES_TABLE = $@"
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = '{DB_TEMPLATES_TABLE_NAME}' AND xtype = 'U')
BEGIN
    CREATE TABLE {DB_TEMPLATES_TABLE_NAME} (
        {DB_TEMPLATES_TABLE_FIELD_ID} INT IDENTITY(1,1) PRIMARY KEY,
        {DB_TEMPLATES_TABLE_FIELD_WORKFLOWID} NVARCHAR(100) NOT NULL,
        {DB_TEMPLATES_TABLE_FIELD_WORKFLOWVERSION} NVARCHAR(50) NOT NULL,
        {DB_TEMPLATES_TABLE_FIELD_CREATEDBY} NVARCHAR(50) NOT NULL,
        {DB_TEMPLATES_TABLE_FIELD_NAME} NVARCHAR(255) NOT NULL,
        {DB_TEMPLATES_TABLE_FIELD_DESCRIPTION} NVARCHAR(MAX) NULL,
        {DB_TEMPLATES_TABLE_FIELD_INPUTPARAMETERS} NVARCHAR(MAX) NULL,
        {DB_TEMPLATES_TABLE_FIELD_ACTIVE} BIT NOT NULL DEFAULT 1,
        {DB_TEMPLATES_TABLE_FIELD_CREATEDAT} DATETIME DEFAULT GETDATE()
    );
END";

        private readonly string DROP_TEMPLATES_TABLE = $@"
IF EXISTS (SELECT * FROM sysobjects WHERE name = '{DB_TEMPLATES_TABLE_NAME}' AND xtype = 'U')
BEGIN
    DROP TABLE {DB_TEMPLATES_TABLE_NAME};
END";

        private readonly string EXISTS_TEMPLATES_TABLE = $@"
SELECT COUNT(*) 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME = '{DB_TEMPLATES_TABLE_NAME}'";

        private readonly string INSERT_TEMPLATE = $@"
INSERT INTO {DB_TEMPLATES_TABLE_NAME} 
({DB_TEMPLATES_TABLE_FIELD_WORKFLOWID}, {DB_TEMPLATES_TABLE_FIELD_WORKFLOWVERSION}, {DB_TEMPLATES_TABLE_FIELD_CREATEDBY}, 
 {DB_TEMPLATES_TABLE_FIELD_NAME}, {DB_TEMPLATES_TABLE_FIELD_DESCRIPTION}, {DB_TEMPLATES_TABLE_FIELD_INPUTPARAMETERS})
OUTPUT INSERTED.{DB_TEMPLATES_TABLE_FIELD_ID}
VALUES (@WorkflowId, @WorkflowVersion, @CreatedBy, @Name, @Description, @InputParameters);";

        private readonly string UPDATE_TEMPLATE = $@"
UPDATE {DB_TEMPLATES_TABLE_NAME} SET
    {DB_TEMPLATES_TABLE_FIELD_NAME} = @Name,
    {DB_TEMPLATES_TABLE_FIELD_DESCRIPTION} = @Description,
    {DB_TEMPLATES_TABLE_FIELD_INPUTPARAMETERS} = @InputParameters
WHERE {DB_TEMPLATES_TABLE_FIELD_ID} = @Id AND {DB_TEMPLATES_TABLE_FIELD_CREATEDBY} = @CreatedBy AND {DB_TEMPLATES_TABLE_FIELD_ACTIVE} = 1;";

        private readonly string DELETE_TEMPLATE = $@"
UPDATE {DB_TEMPLATES_TABLE_NAME} 
SET {DB_TEMPLATES_TABLE_FIELD_ACTIVE} = 0 
WHERE {DB_TEMPLATES_TABLE_FIELD_ID} = @Id AND {DB_TEMPLATES_TABLE_FIELD_CREATEDBY} = @CreatedBy;";

        private readonly string SELECT_TEMPLATES_FOR_USER = $@"
SELECT * 
FROM {DB_TEMPLATES_TABLE_NAME} 
WHERE {DB_TEMPLATES_TABLE_FIELD_CREATEDBY} = @CreatedBy AND {DB_TEMPLATES_TABLE_FIELD_ACTIVE} = 1
ORDER BY {DB_TEMPLATES_TABLE_FIELD_CREATEDAT} DESC;";

        public TemplatesAccess() : base() { }

        public override void CreateTable(SqlConnection conn)
        {
            using (var cmd = new SqlCommand(CREATE_TEMPLATES_TABLE, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public override void DropTable(SqlConnection conn)
        {
            using (var cmd = new SqlCommand(DROP_TEMPLATES_TABLE, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public override bool Exists(SqlConnection conn)
        {
            using (var cmd = new SqlCommand(EXISTS_TEMPLATES_TABLE, conn))
            {
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        /// <summary>
        /// Inserta un nuevo template y devuelve su Id.
        /// </summary>
        public int InsertTemplate(SqlConnection conn, string workflowId, string workflowVersion, string createdBy, string name, string description, string inputParametersJson)
        {
            using (var cmd = new SqlCommand(INSERT_TEMPLATE, conn))
            {
                cmd.Parameters.AddWithValue("@WorkflowId", workflowId);
                cmd.Parameters.AddWithValue("@WorkflowVersion", workflowVersion);
                cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@InputParameters", inputParametersJson ?? (object)DBNull.Value);

                return (int)cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Actualiza un template existente.
        /// </summary>
        public bool UpdateTemplate(SqlConnection conn, int id, string createdBy, string name, string description, string inputParametersJson)
        {
            using (var cmd = new SqlCommand(UPDATE_TEMPLATE, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@InputParameters", inputParametersJson ?? (object)DBNull.Value);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// Marca un template como inactivo (soft delete).
        /// </summary>
        public bool DeleteTemplate(SqlConnection conn, int id, string createdBy)
        {
            using (var cmd = new SqlCommand(DELETE_TEMPLATE, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@CreatedBy", createdBy);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// Obtiene todos los templates activos de un usuario.
        /// Devuelve cada registro como un diccionario de campos.
        /// </summary>
        public List<Dictionary<string, object>> GetTemplates(SqlConnection conn, string user)
        {
            var list = new List<Dictionary<string, object>>();

            using (var cmd = new SqlCommand(SELECT_TEMPLATES_FOR_USER, conn))
            {
                cmd.Parameters.AddWithValue("@CreatedBy", user);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var record = new Dictionary<string, object>
                        {
                            [DB_TEMPLATES_TABLE_FIELD_ID] = reader.GetInt32(reader.GetOrdinal(DB_TEMPLATES_TABLE_FIELD_ID)),
                            [DB_TEMPLATES_TABLE_FIELD_WORKFLOWID] = reader.GetString(reader.GetOrdinal(DB_TEMPLATES_TABLE_FIELD_WORKFLOWID)),
                            [DB_TEMPLATES_TABLE_FIELD_WORKFLOWVERSION] = reader.GetString(reader.GetOrdinal(DB_TEMPLATES_TABLE_FIELD_WORKFLOWVERSION)),
                            [DB_TEMPLATES_TABLE_FIELD_CREATEDBY] = reader.GetString(reader.GetOrdinal(DB_TEMPLATES_TABLE_FIELD_CREATEDBY)),
                            [DB_TEMPLATES_TABLE_FIELD_NAME] = reader.GetString(reader.GetOrdinal(DB_TEMPLATES_TABLE_FIELD_NAME)),
                            [DB_TEMPLATES_TABLE_FIELD_DESCRIPTION] = reader[DB_TEMPLATES_TABLE_FIELD_DESCRIPTION]?.ToString(),
                            [DB_TEMPLATES_TABLE_FIELD_INPUTPARAMETERS] = reader[DB_TEMPLATES_TABLE_FIELD_INPUTPARAMETERS]?.ToString(),
                            [DB_TEMPLATES_TABLE_FIELD_ACTIVE] = reader.GetBoolean(reader.GetOrdinal(DB_TEMPLATES_TABLE_FIELD_ACTIVE)),
                            [DB_TEMPLATES_TABLE_FIELD_CREATEDAT] = reader.GetDateTime(reader.GetOrdinal(DB_TEMPLATES_TABLE_FIELD_CREATEDAT))
                        };

                        list.Add(record);
                    }
                }
            }

            return list;
        }
    }
}
