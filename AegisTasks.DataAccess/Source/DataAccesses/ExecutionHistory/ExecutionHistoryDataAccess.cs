using AegisTasks.DataAccess.Common;
using Microsoft.Data.SqlClient;
using System;

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

        private readonly string CREATE_EXECUTION_HISTORY_TABLE = string.Format(@"
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = '{0}' AND xtype = 'U')
BEGIN
    CREATE TABLE {0} (
        {1} INT IDENTITY(1,1) PRIMARY KEY,
        {2} NVARCHAR(100) NOT NULL,
        {3} NVARCHAR(255) NOT NULL,
        {4} DATETIME NULL,
        {5} DATETIME NULL,
        {6} BIT NOT NULL DEFAULT 0
    );
END",
            DB_EXECUTION_HISTORY_TABLE_NAME,
            DB_EXECUTION_HISTORY_TABLE_FIELD_ID,
            DB_EXECUTION_HISTORY_TABLE_FIELD_WORKFLOWID,
            DB_EXECUTION_HISTORY_TABLE_FIELD_WORKFLOWNAME,
            DB_EXECUTION_HISTORY_TABLE_FIELD_STARTDATE,
            DB_EXECUTION_HISTORY_TABLE_FIELD_ENDDATE,
            DB_EXECUTION_HISTORY_TABLE_FIELD_SUCCESS
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

        public ExecutionHistoryDataAccess() : base() { }

        public override void CreateTable(SqlConnection conn)
        {
            using (var command = new SqlCommand(CREATE_EXECUTION_HISTORY_TABLE, conn))
            {
                command.CommandType = System.Data.CommandType.Text;
                command.ExecuteNonQuery();
            }
        }

        public override void DropTable(SqlConnection conn)
        {
            using (var command = new SqlCommand(DROP_EXECUTION_HISTORY_TABLE, conn))
            {
                command.CommandType = System.Data.CommandType.Text;
                command.ExecuteNonQuery();
            }
        }

        public override bool Exists(SqlConnection conn)
        {
            using (var command = new SqlCommand(EXISTS_EXECUTION_HISTORY_TABLE, conn))
            {
                command.CommandType = System.Data.CommandType.Text;
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
    }
}
