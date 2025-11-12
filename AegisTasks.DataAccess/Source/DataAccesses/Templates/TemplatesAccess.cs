using AegisTasks.DataAccess.Common;
using Microsoft.Data.SqlClient;

namespace AegisTasks.DataAccess.DataAccesses
{
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

        private readonly string CREATE_TEMPLATES_TABLE = string.Format(@"
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = '{0}' AND xtype = 'U')
BEGIN
    CREATE TABLE {0} (
        {1} INT IDENTITY(1,1) PRIMARY KEY,
        {2} NVARCHAR(100) NOT NULL,
        {3} NVARCHAR(50) NOT NULL,
        {4} NVARCHAR(50) NOT NULL,
        {5} NVARCHAR(255) NOT NULL,
        {6} NVARCHAR(MAX) NULL,
        {7} NVARCHAR(MAX) NULL,
        {8} BIT NOT NULL DEFAULT 1,
        {9} DATETIME DEFAULT GETDATE(),
        CONSTRAINT FK_Templates_Users FOREIGN KEY ({4}) REFERENCES {10}({11})
    );
END",
            DB_TEMPLATES_TABLE_NAME,
            DB_TEMPLATES_TABLE_FIELD_ID,
            DB_TEMPLATES_TABLE_FIELD_WORKFLOWID,
            DB_TEMPLATES_TABLE_FIELD_WORKFLOWVERSION,
            DB_TEMPLATES_TABLE_FIELD_CREATEDBY,
            DB_TEMPLATES_TABLE_FIELD_NAME,
            DB_TEMPLATES_TABLE_FIELD_DESCRIPTION,
            DB_TEMPLATES_TABLE_FIELD_INPUTPARAMETERS,
            DB_TEMPLATES_TABLE_FIELD_ACTIVE,
            DB_TEMPLATES_TABLE_FIELD_CREATEDAT,
            UsersDataAccess.DB_USERS_TABLE_NAME,
            UsersDataAccess.DB_USERS_TABLE_FIELD_USERNAME
        );

        private readonly string DROP_TEMPLATES_TABLE = string.Format(@"
IF EXISTS (SELECT * FROM sysobjects WHERE name = '{0}' AND xtype = 'U')
BEGIN
    DROP TABLE {0};
END",
    DB_TEMPLATES_TABLE_NAME
);

        public TemplatesAccess() : base() { }

        public override void CreateTable(SqlConnection conn)
        {
            using (var command = new SqlCommand(CREATE_TEMPLATES_TABLE, conn))
            {
                command.CommandType = System.Data.CommandType.Text;
                command.ExecuteNonQuery();
            }
        }

        public override void DropTable(SqlConnection conn)
        {
            using (var command = new SqlCommand(DROP_TEMPLATES_TABLE, conn))
            {
                command.CommandType = System.Data.CommandType.Text;
                command.ExecuteNonQuery();
            }
        }
    }
}
