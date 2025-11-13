using AegisTasks.DataAccess.Common;
using Microsoft.Data.SqlClient;
using System;

namespace AegisTasks.DataAccess.DataAccesses
{
    public class UserParametersAccess : AegisDataAccessSqlServerBase
    {
        public static readonly string DB_USER_PARAMETERS_TABLE_NAME = "UserParameters";

        public static readonly string DB_USER_PARAMETERS_TABLE_FIELD_USERNAME = "Username";
        public static readonly string DB_USER_PARAMETERS_TABLE_FIELD_LANGUAGE = "Language";

        private readonly string CREATE_USER_PARAMETERS_TABLE = string.Format(@"
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = '{0}' AND xtype = 'U')
BEGIN
    CREATE TABLE {0} (
        {1} NVARCHAR(50) NOT NULL,
        {2} NVARCHAR(10) NULL,
        CONSTRAINT FK_UserParameters_Users FOREIGN KEY ({1}) REFERENCES {3}({4})
    );
END",
            DB_USER_PARAMETERS_TABLE_NAME,
            DB_USER_PARAMETERS_TABLE_FIELD_USERNAME,
            DB_USER_PARAMETERS_TABLE_FIELD_LANGUAGE,
            UsersDataAccess.DB_USERS_TABLE_NAME,
            UsersDataAccess.DB_USERS_TABLE_FIELD_USERNAME
        );

        private readonly string DROP_USER_PARAMETERS_TABLE = string.Format(@"
IF EXISTS (SELECT * FROM sysobjects WHERE name = '{0}' AND xtype = 'U')
BEGIN
    DROP TABLE {0};
END",
            DB_USER_PARAMETERS_TABLE_NAME
        );

        private readonly string EXISTS_USER_PARAMETERS_TABLE = string.Format(@"
SELECT COUNT(*) 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME = '{0}'",
            DB_USER_PARAMETERS_TABLE_NAME
        );

        public UserParametersAccess() : base() { }

        public override void CreateTable(SqlConnection conn)
        {
            using (var command = new SqlCommand(CREATE_USER_PARAMETERS_TABLE, conn))
            {
                command.CommandType = System.Data.CommandType.Text;
                command.ExecuteNonQuery();
            }
        }

        public override void DropTable(SqlConnection conn)
        {
            using (var command = new SqlCommand(DROP_USER_PARAMETERS_TABLE, conn))
            {
                command.CommandType = System.Data.CommandType.Text;
                command.ExecuteNonQuery();
            }
        }

        public override bool Exists(SqlConnection conn)
        {
            using (var command = new SqlCommand(EXISTS_USER_PARAMETERS_TABLE, conn))
            {
                command.CommandType = System.Data.CommandType.Text;
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
    }
}
