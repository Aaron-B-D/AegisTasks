using AegisTasks.Core.DTO;
using AegisTasks.DataAccess.Common;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace AegisTasks.DataAccess.DataAccesses
{
    public class UserParametersAccess : AegisDataAccessSqlServerBase
    {
        public static readonly string DB_USER_PARAMETERS_TABLE_NAME = "UserParameters";

        public static readonly string DB_USER_PARAMETERS_TABLE_FIELD_ID = "Id";
        public static readonly string DB_USER_PARAMETERS_TABLE_FIELD_USERNAME = "Username";
        public static readonly string DB_USER_PARAMETERS_TABLE_FIELD_PARAMETER = "ParameterType";
        public static readonly string DB_USER_PARAMETERS_TABLE_FIELD_VALUE = "Value";

        private readonly string CREATE_TABLE = string.Format(@"
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = '{0}' AND xtype = 'U')
BEGIN
    CREATE TABLE {0} (
        {1} INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        {2} NVARCHAR(50) NOT NULL,
        {3} INT NOT NULL,
        {4} NVARCHAR(50) NOT NULL,
        CONSTRAINT FK_UserParameters_Users FOREIGN KEY ({2}) REFERENCES {5}({6})
    );
END",
            DB_USER_PARAMETERS_TABLE_NAME,
            DB_USER_PARAMETERS_TABLE_FIELD_ID,
            DB_USER_PARAMETERS_TABLE_FIELD_USERNAME,
            DB_USER_PARAMETERS_TABLE_FIELD_PARAMETER,
            DB_USER_PARAMETERS_TABLE_FIELD_VALUE,
            UsersDataAccess.DB_USERS_TABLE_NAME,
            UsersDataAccess.DB_USERS_TABLE_FIELD_USERNAME
        );

        private readonly string DROP_TABLE = string.Format(@"
IF EXISTS (SELECT * FROM sysobjects WHERE name = '{0}' AND xtype = 'U')
BEGIN
    DROP TABLE {0};
END",
            DB_USER_PARAMETERS_TABLE_NAME
        );

        private readonly string EXISTS_TABLE = string.Format(@"
SELECT COUNT(*) 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME = '{0}';",
            DB_USER_PARAMETERS_TABLE_NAME
        );

        private readonly string INSERT_PARAMETER = string.Format(@"
INSERT INTO {0} ({1}, {2}, {3})
VALUES (@Username, @ParameterType, @Value);",
            DB_USER_PARAMETERS_TABLE_NAME,
            DB_USER_PARAMETERS_TABLE_FIELD_USERNAME,
            DB_USER_PARAMETERS_TABLE_FIELD_PARAMETER,
            DB_USER_PARAMETERS_TABLE_FIELD_VALUE
        );

        private readonly string UPDATE_PARAMETER = string.Format(@"
UPDATE {0}
SET {3} = @Value
WHERE {1} = @Username AND {2} = @ParameterType;",
            DB_USER_PARAMETERS_TABLE_NAME,
            DB_USER_PARAMETERS_TABLE_FIELD_USERNAME,
            DB_USER_PARAMETERS_TABLE_FIELD_PARAMETER,
            DB_USER_PARAMETERS_TABLE_FIELD_VALUE
        );

        private readonly string DELETE_PARAMETERS = string.Format(@"
DELETE FROM {0}
WHERE {1} = @Username;",
            DB_USER_PARAMETERS_TABLE_NAME,
            DB_USER_PARAMETERS_TABLE_FIELD_USERNAME
        );

        private readonly string SELECT_PARAMETER = string.Format(@"
SELECT {3}
FROM {0}
WHERE {1} = @Username AND {2} = @ParameterType;",
            DB_USER_PARAMETERS_TABLE_NAME,
            DB_USER_PARAMETERS_TABLE_FIELD_USERNAME,
            DB_USER_PARAMETERS_TABLE_FIELD_PARAMETER,
            DB_USER_PARAMETERS_TABLE_FIELD_VALUE
        );

        public UserParametersAccess() : base() { }

        public override void CreateTable(SqlConnection conn)
        {
            using (SqlCommand command = new SqlCommand(CREATE_TABLE, conn))
            {
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
        }

        public override void DropTable(SqlConnection conn)
        {
            using (SqlCommand command = new SqlCommand(DROP_TABLE, conn))
            {
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
        }

        public override bool Exists(SqlConnection conn)
        {
            using (SqlCommand command = new SqlCommand(EXISTS_TABLE, conn))
            {
                command.CommandType = CommandType.Text;
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        public bool AddParameter(SqlConnection conn, string username, UserParameterType parameterType, string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), "El valor del parámetro no puede ser nulo");
            }

            using (SqlCommand command = new SqlCommand(INSERT_PARAMETER, conn))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@ParameterType", (int)parameterType);
                command.Parameters.AddWithValue("@Value", value);
                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }

        public bool UpdateParameter(SqlConnection conn, string username, UserParameterType parameterType, string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), "El valor del parámetro no puede ser nulo");
            }

            using (SqlCommand command = new SqlCommand(UPDATE_PARAMETER, conn))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@ParameterType", (int)parameterType);
                command.Parameters.AddWithValue("@Value", value);
                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }

        public bool DeleteUserParameters(SqlConnection conn, string username)
        {
            using (SqlCommand command = new SqlCommand(DELETE_PARAMETERS, conn))
            {
                command.Parameters.AddWithValue("@Username", username);
                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }

        public string GetParameter(SqlConnection conn, string username, UserParameterType parameterType)
        {
            using (SqlCommand command = new SqlCommand(SELECT_PARAMETER, conn))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@ParameterType", (int)parameterType);
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return result.ToString();
                }
                return null;
            }
        }
    }
}
