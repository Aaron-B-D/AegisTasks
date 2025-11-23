using AegisTasks.Core.DTO;
using AegisTasks.DataAccess.Common;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace AegisTasks.DataAccess.DataAccesses
{
    public class ExecutionHistoryDataAccess : AegisDataAccessSqlServerBase
    {
        public static readonly string DB_EXECUTION_HISTORY_TABLE_NAME = "ExecutionHistory";
        public static readonly string DB_EXECUTION_HISTORY_TABLE_FIELD_ID = "Id";
        public static readonly string DB_EXECUTION_HISTORY_TABLE_FIELD_WORKFLOWID = "WorkflowId";
        public static readonly string DB_EXECUTION_HISTORY_TABLE_FIELD_WORKFLOWNAME = "WorkflowName";
        public static readonly string DB_EXECUTION_HISTORY_TABLE_FIELD_STARTDATE = "StartDate";
        public static readonly string DB_EXECUTION_HISTORY_TABLE_FIELD_ENDDATE = "EndDate";
        public static readonly string DB_EXECUTION_HISTORY_TABLE_FIELD_SUCCESS = "Success";
        public static readonly string DB_EXECUTION_HISTORY_TABLE_FIELD_ACTIVE = "Active";
        public static readonly string DB_EXECUTION_HISTORY_TABLE_FIELD_USERNAME = "Username";

        private readonly string CREATE_EXECUTION_HISTORY_TABLE = $@"
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = '{DB_EXECUTION_HISTORY_TABLE_NAME}' AND xtype = 'U')
BEGIN
    CREATE TABLE {DB_EXECUTION_HISTORY_TABLE_NAME} (
        {DB_EXECUTION_HISTORY_TABLE_FIELD_ID} INT IDENTITY(1,1) PRIMARY KEY,
        {DB_EXECUTION_HISTORY_TABLE_FIELD_WORKFLOWID} NVARCHAR(100) NOT NULL,
        {DB_EXECUTION_HISTORY_TABLE_FIELD_WORKFLOWNAME} NVARCHAR(255) NOT NULL,
        {DB_EXECUTION_HISTORY_TABLE_FIELD_STARTDATE} DATETIME NULL,
        {DB_EXECUTION_HISTORY_TABLE_FIELD_ENDDATE} DATETIME NULL,
        {DB_EXECUTION_HISTORY_TABLE_FIELD_SUCCESS} BIT NOT NULL DEFAULT 0,
        {DB_EXECUTION_HISTORY_TABLE_FIELD_ACTIVE} BIT NOT NULL DEFAULT 1,
        {DB_EXECUTION_HISTORY_TABLE_FIELD_USERNAME} NVARCHAR(50) NOT NULL
    );
END";

        private readonly string DROP_EXECUTION_HISTORY_TABLE = $@"
IF EXISTS (SELECT * FROM sysobjects WHERE name = '{DB_EXECUTION_HISTORY_TABLE_NAME}' AND xtype = 'U')
BEGIN
    DROP TABLE {DB_EXECUTION_HISTORY_TABLE_NAME};
END";

        private readonly string EXISTS_EXECUTION_HISTORY_TABLE = $@"
SELECT COUNT(*) 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME = '{DB_EXECUTION_HISTORY_TABLE_NAME}'";

        private readonly string INSERT_EXECUTION_RETURN_ID = $@"
INSERT INTO {DB_EXECUTION_HISTORY_TABLE_NAME} 
({DB_EXECUTION_HISTORY_TABLE_FIELD_WORKFLOWID}, {DB_EXECUTION_HISTORY_TABLE_FIELD_WORKFLOWNAME}, {DB_EXECUTION_HISTORY_TABLE_FIELD_STARTDATE}, {DB_EXECUTION_HISTORY_TABLE_FIELD_SUCCESS}, {DB_EXECUTION_HISTORY_TABLE_FIELD_ACTIVE}, {DB_EXECUTION_HISTORY_TABLE_FIELD_USERNAME})
VALUES (@WorkflowId, @WorkflowName, @StartDate, 0, 1, @Username);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

        private readonly string UPDATE_EXECUTION = $@"
UPDATE {DB_EXECUTION_HISTORY_TABLE_NAME}
SET {DB_EXECUTION_HISTORY_TABLE_FIELD_ENDDATE} = @EndDate,
    {DB_EXECUTION_HISTORY_TABLE_FIELD_SUCCESS} = @Success
WHERE {DB_EXECUTION_HISTORY_TABLE_FIELD_ID} = @Id;";

        private readonly string SELECT_EXISTING_EXECUTION_ID = $@"
SELECT {DB_EXECUTION_HISTORY_TABLE_FIELD_ID} 
FROM {DB_EXECUTION_HISTORY_TABLE_NAME} 
WHERE {DB_EXECUTION_HISTORY_TABLE_FIELD_WORKFLOWID} = @WorkflowId 
  AND {DB_EXECUTION_HISTORY_TABLE_FIELD_ACTIVE} = 1;";

        private readonly string SELECT_EXECUTIONS_BY_USER_BASE = $@"
SELECT {DB_EXECUTION_HISTORY_TABLE_FIELD_ID}, {DB_EXECUTION_HISTORY_TABLE_FIELD_WORKFLOWID}, {DB_EXECUTION_HISTORY_TABLE_FIELD_WORKFLOWNAME}, 
       {DB_EXECUTION_HISTORY_TABLE_FIELD_STARTDATE}, {DB_EXECUTION_HISTORY_TABLE_FIELD_ENDDATE}, {DB_EXECUTION_HISTORY_TABLE_FIELD_SUCCESS}, {DB_EXECUTION_HISTORY_TABLE_FIELD_ACTIVE}
FROM {DB_EXECUTION_HISTORY_TABLE_NAME}
WHERE {DB_EXECUTION_HISTORY_TABLE_FIELD_USERNAME} = @Username";

        public ExecutionHistoryDataAccess() : base() { }

        public override void CreateTable(SqlConnection conn)
        {
            using (SqlCommand command = new SqlCommand(CREATE_EXECUTION_HISTORY_TABLE, conn))
            {
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
        }

        public override void DropTable(SqlConnection conn)
        {
            using (SqlCommand command = new SqlCommand(DROP_EXECUTION_HISTORY_TABLE, conn))
            {
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
        }

        public override bool Exists(SqlConnection conn)
        {
            using (SqlCommand command = new SqlCommand(EXISTS_EXECUTION_HISTORY_TABLE, conn))
            {
                command.CommandType = CommandType.Text;
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        // Método público para registrar ejecución (una fila por workflow activo)
        public int RegisterExecution(SqlConnection conn, string workflowId, string workflowName, string username)
        {
            int? existingId = GetActiveExecutionId(conn, workflowId);
            if (existingId.HasValue)
            {
                return existingId.Value;
            }

            using (SqlCommand command = new SqlCommand(INSERT_EXECUTION_RETURN_ID, conn))
            {
                command.Parameters.AddWithValue("@WorkflowId", workflowId);
                command.Parameters.AddWithValue("@WorkflowName", workflowName);
                command.Parameters.AddWithValue("@StartDate", DateTime.Now);
                command.Parameters.AddWithValue("@Username", username);

                return (int)command.ExecuteScalar();
            }
        }

        public void UpdateExecution(SqlConnection conn, int id, bool success)
        {
            using (SqlCommand command = new SqlCommand(UPDATE_EXECUTION, conn))
            {
                command.Parameters.AddWithValue("@EndDate", DateTime.Now);
                command.Parameters.AddWithValue("@Success", success);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        public List<ExecutionHistoryLineDTO> GetExecutionsByUser(SqlConnection conn, string username, bool includeInactive = false)
        {
            List<ExecutionHistoryLineDTO> results = new List<ExecutionHistoryLineDTO>();

            string query = SELECT_EXECUTIONS_BY_USER_BASE;
            if (!includeInactive)
            {
                query += $" AND {DB_EXECUTION_HISTORY_TABLE_FIELD_ACTIVE} = 1";
            }

            using (SqlCommand command = new SqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@Username", username);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new ExecutionHistoryLineDTO
                        {
                            Id = reader.GetInt32(0),
                            WorkflowId = reader.GetString(1),
                            WorkflowName = reader.GetString(2),
                            StartDate = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                            EndDate = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                            Success = reader.GetBoolean(5),
                            Active = reader.GetBoolean(6)
                        });
                    }
                }
            }

            return results;
        }

        // Método privado para obtener Id de ejecución activa de un workflow
        private int? GetActiveExecutionId(SqlConnection conn, string workflowId)
        {
            using (SqlCommand command = new SqlCommand(SELECT_EXISTING_EXECUTION_ID, conn))
            {
                command.Parameters.AddWithValue("@WorkflowId", workflowId);
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                    return (int)result;
            }
            return null;
        }
    }
}
