using AegisTasks.DataAccess.Common;
using AegisTasks.Core.DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace AegisTasks.DataAccess.DataAccesses
{
    public class UsersDataAccess : AegisDataAccessSqlServerBase
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

        private readonly string EXISTS_USERS_TABLE = string.Format(@"
SELECT COUNT(*) 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME = '{0}'",
            DB_USERS_TABLE_NAME
        );

        private readonly string INSERT_USER = string.Format(@"
INSERT INTO {0} ({1}, {2}, {3}, {4})
VALUES (@Username, @FirstName, @LastName, @Password);",
            DB_USERS_TABLE_NAME,
            DB_USERS_TABLE_FIELD_USERNAME,
            DB_USERS_TABLE_FIELD_FIRSTNAME,
            DB_USERS_TABLE_FIELD_LASTNAME,
            DB_USERS_TABLE_FIELD_PASSWORD
        );

        private readonly string DELETE_USER = string.Format(@"
DELETE FROM {0}
WHERE {1} = @Username;",
            DB_USERS_TABLE_NAME,
            DB_USERS_TABLE_FIELD_USERNAME
        );

        private readonly string UPDATE_USER_PASSWORD = string.Format(@"
UPDATE {0}
SET {1} = @Password
WHERE {2} = @Username;",
            DB_USERS_TABLE_NAME,
            DB_USERS_TABLE_FIELD_PASSWORD,
            DB_USERS_TABLE_FIELD_USERNAME
        );

        private readonly string SELECT_USER_PASSWORD = string.Format(@"
SELECT {0}
FROM {1}
WHERE {2} = @Username;",
            DB_USERS_TABLE_FIELD_PASSWORD,
            DB_USERS_TABLE_NAME,
            DB_USERS_TABLE_FIELD_USERNAME
        );

        private readonly string SELECT_USER = string.Format(@"
SELECT {0}, {1}, {2}, {3}, {4}
FROM {5}
WHERE {0} = @Username;",
            DB_USERS_TABLE_FIELD_USERNAME,
            DB_USERS_TABLE_FIELD_FIRSTNAME,
            DB_USERS_TABLE_FIELD_LASTNAME,
            DB_USERS_TABLE_FIELD_PASSWORD,
            DB_USERS_TABLE_FIELD_CREATEDAT,
            DB_USERS_TABLE_NAME
        );

        public UsersDataAccess() : base() { }

        public override void CreateTable(SqlConnection conn)
        {
            using (SqlCommand command = new SqlCommand(CREATE_USERS_TABLE, conn))
            {
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
        }

        public override void DropTable(SqlConnection conn)
        {
            using (SqlCommand command = new SqlCommand(DROP_USERS_TABLE, conn))
            {
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
        }

        public override bool Exists(SqlConnection conn)
        {
            using (SqlCommand command = new SqlCommand(EXISTS_USERS_TABLE, conn))
            {
                command.CommandType = CommandType.Text;
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        public bool InsertUser(SqlConnection conn, UserDTO user)
        {
            using (SqlCommand command = new SqlCommand(INSERT_USER, conn))
            {
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@Password", user.Password);

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }

        public bool DeleteUser(SqlConnection conn, string username)
        {
            using (SqlCommand command = new SqlCommand(DELETE_USER, conn))
            {
                command.Parameters.AddWithValue("@Username", username);
                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }

        public bool UpdatePassword(SqlConnection conn, string username, string hashedPassword)
        {
            using (SqlCommand command = new SqlCommand(UPDATE_USER_PASSWORD, conn))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", hashedPassword);

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }

        public string GetUserPassword(SqlConnection conn, string username)
        {
            using (SqlCommand command = new SqlCommand(SELECT_USER_PASSWORD, conn))
            {
                command.Parameters.AddWithValue("@Username", username);
                object result = command.ExecuteScalar();
                return result?.ToString();
            }
        }

        public UserDTO GetUser(SqlConnection conn, string username)
        {
            using (SqlCommand command = new SqlCommand(SELECT_USER, conn))
            {
                command.Parameters.AddWithValue("@Username", username);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserDTO
                        {
                            Username = reader.GetString(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Password = reader.GetString(3),
                            CreatedAt = reader.GetDateTime(4)
                        };
                    }
                }
            }

            return null;
        }
    }
}
