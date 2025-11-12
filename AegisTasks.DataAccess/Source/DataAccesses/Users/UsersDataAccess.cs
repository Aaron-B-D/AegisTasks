using AegisTasks.DataAccess.Common;
using AegisTasks.DataAccess.ConnectionFactory;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AegisTasks.DataAccess.DataAccesses
{
    public class UsersDataAccess: AegisDataAccessSqlServerBase
    {
        public static readonly string DB_USERS_TABLE_NAME = "Users";

        public static readonly string DB_USERS_TABLE_FIELD_USERNAME = "Username";
        public static readonly string DB_USERS_TABLE_FIELD_FIRSTNAME = "FirstName";
        public static readonly string DB_USERS_TABLE_FIELD_LASTNAME = "LastName";
        public static readonly string DB_USERS_TABLE_FIELD_PASSWORD = "Password";
        public static readonly string DB_USERS_TABLE_FIELD_CREATEDAT = "CreatedAt";

        private readonly string CREATE_USERS_TABLE = string.Format(@"
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = '{0}' AND xtype = 'U')
BEGIN
    CREATE TABLE {0} (
        {1} NVARCHAR(50) NOT NULL PRIMARY KEY,
        {2} NVARCHAR(100) NOT NULL,
        {3} NVARCHAR(150) NOT NULL,
        {4} NVARCHAR(255) NOT NULL,
        {5} DATETIME DEFAULT GETDATE()
    );
END",
                    DB_USERS_TABLE_NAME,
                    DB_USERS_TABLE_FIELD_USERNAME,
                    DB_USERS_TABLE_FIELD_FIRSTNAME,
                    DB_USERS_TABLE_FIELD_LASTNAME,
                    DB_USERS_TABLE_FIELD_PASSWORD,
                    DB_USERS_TABLE_FIELD_CREATEDAT
                );

        private readonly string DROP_USERS_TABLE = string.Format(@"
IF EXISTS (SELECT * FROM sysobjects WHERE name = '{0}' AND xtype = 'U')
BEGIN
    DROP TABLE {0};
END",
            DB_USERS_TABLE_NAME
        );

        public UsersDataAccess() : base()
        {}

        public override void CreateTable(SqlConnection conn)
        {
            using (var command = new SqlCommand(CREATE_USERS_TABLE, conn))
            {
                command.CommandType = System.Data.CommandType.Text;
                command.ExecuteNonQuery();
            }
        }

        public override void DropTable(SqlConnection conn)
        {
            using (var command = new SqlCommand(DROP_USERS_TABLE, conn))
            {
                command.CommandType = System.Data.CommandType.Text;
                command.ExecuteNonQuery();
            }
        }
    }
}
