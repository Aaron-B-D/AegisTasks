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

        private readonly string CREATE_EXECUTION_HISTORY_TABLE = string.Format(@"
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = '{0}' AND xtype = 'U')
BEGIN
    CREATE TABLE {0} (
        {1} INT IDENTITY(1,1) PRIMARY KEY,
        {2} NVARCHAR(100) NOT NULL,
        {3} NVARCHAR(255) NOT NULL,
        {4} DATETIME NULL,
        {5} DATETIME NULL,
        {6} BIT NOT NULL DEFAULT 0,
        {7} BIT NOT NULL DEFAULT 1,
        {8} NVARCHAR(50) NOT NULL
    );
END",
            DB_EXECUTION_HISTORY_TABLE_NAME,
            DB_EXECUTION_HISTORY_TABLE_FIELD_ID,
            DB_EXECUTION_HISTORY_TABLE_FIELD_WORKFLOWID,
            DB_EXECUTION_HISTORY_TABLE_FIELD_WORKFLOWNAME,
            DB_EXECUTION_HISTORY_TABLE_FIELD_STARTDATE,
            DB_EXECUTION_HISTORY_TABLE_FIELD_ENDDATE,
            DB_EXECUTION_HISTORY_TABLE_FIELD_SUCCESS,
            DB_EXECUTION_HISTORY_TABLE_FIELD_ACTIVE,
            DB_EXECUTION_HISTORY_TABLE_FIELD_USERNAME
        );

        private readonly string DROP_EXECUTION_HISTORY_TABLE = string.Format(@"
IF EXISTS (SELECT * FROM sysobjects WHERE name = '{0}' AND xtype = 'U')
BEGIN
    DROP TABLE {0};
END",
            DB_EXECUTION_HISTORY_TABLE_NAME
        );

        private readonly string EXISTS_EXECUTION_HISTORY_TABLE = string.Format(@"
SELECT COUNT(*) 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME = '{0}'",
            DB_EXECUTION_HISTORY_TABLE_NAME
        );

        private readonly string INSERT_EXECUTION = string.Format(@"
INSERT INTO {0} ({1}, {2}, {3}, {4}, {5}, {6})
VALUES (@WorkflowId, @WorkflowName, @StartDate, 0, 1, @Username);",
            DB_EXECUTION_HISTORY_TABLE_NAME,
            DB_EXECUTION_HISTORY_TABLE_FIELD_WORKFLOWID,
            DB_EXECUTION_HISTORY_TABLE_FIELD_WORKFLOWNAME,
            DB_EXECUTION_HISTORY_TABLE_FIELD_STARTDATE,
            DB_EXECUTION_HISTORY_TABLE_FIELD_SUCCESS,
            DB_EXECUTION_HISTORY_TABLE_FIELD_ACTIVE,
            DB_EXECUTION_HISTORY_TABLE_FIELD_USERNAME
        );

        private readonly string UPDATE_EXECUTION = string.Format(@"
UPDATE {0}
SET {1} = @EndDate,
    {2} = @Success
WHERE {3} = @Id;",
            DB_EXECUTION_HISTORY_TABLE_NAME,
            DB_EXECUTION_HISTORY_TABLE_FIELD_ENDDATE,
            DB_EXECUTION_HISTORY_TABLE_FIELD_SUCCESS,
            DB_EXECUTION_HISTORY_TABLE_FIELD_ID
        );

        // Query base para recuperar registros de un usuario (sin filtrar Active por defecto)
        private readonly string SELECT_EXECUTIONS_BY_USER_BASE = string.Format(@"
SELECT {0}, {1}, {2}, {3}, {4}, {5}, {6}
FROM {7}
WHERE {8} = @Username",
            DB_EXECUTION_HISTORY_TABLE_FIELD_ID,
            DB_EXECUTION_HISTORY_TABLE_FIELD_WORKFLOWID,
            DB_EXECUTION_HISTORY_TABLE_FIELD_WORKFLOWNAME,
            DB_EXECUTION_HISTORY_TABLE_FIELD_STARTDATE,
            DB_EXECUTION_HISTORY_TABLE_FIELD_ENDDATE,
            DB_EXECUTION_HISTORY_TABLE_FIELD_SUCCESS,
            DB_EXECUTION_HISTORY_TABLE_FIELD_ACTIVE,
            DB_EXECUTION_HISTORY_TABLE_NAME,
            DB_EXECUTION_HISTORY_TABLE_FIELD_USERNAME
        );

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

        public void RegisterExecution(SqlConnection conn, string workflowId, string workflowName, string username)
        {
            using (SqlCommand command = new SqlCommand(INSERT_EXECUTION, conn))
            {
                command.Parameters.AddWithValue("@WorkflowId", workflowId);
                command.Parameters.AddWithValue("@WorkflowName", workflowName);
                command.Parameters.AddWithValue("@StartDate", DateTime.Now);
                command.Parameters.AddWithValue("@Username", username);
                command.ExecuteNonQuery();
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
    }
}
