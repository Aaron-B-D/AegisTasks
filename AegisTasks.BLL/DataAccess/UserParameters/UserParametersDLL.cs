using AegisTasks.Core.Common;
using AegisTasks.Core.DTO;
using AegisTasks.DataAccess.DataAccesses;
using Microsoft.Data.SqlClient;
using System;

namespace AegisTasks.BLL.DataAccess
{
    public static class UserParametersBLL
    {
        private static readonly UserParametersAccess _DataAccess = new UserParametersAccess();

        // Parámetros por defecto tipados
        private static readonly UserParameterDTO<Language> DefaultLanguageParameter =
            new UserParameterDTO<Language>(UserParameterType.LANGUAGE, Language.ENGLISH);

        public static bool CreateUserParameters(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username), "El nombre de usuario no puede ser nulo o vacío");
            }

            username = username.ToLowerInvariant();

            using (SqlConnection conn = DataAccessBLL.CreateConnection())
            {
                conn.Open();

                // Insertar los parámetros por defecto tipados
                bool inserted = _DataAccess.AddParameter(conn, username,
                    DefaultLanguageParameter.Type,
                    DefaultLanguageParameter.Value.ToString());

                return inserted;
            }
        }

        public static bool ModifyParameter(string username, UserParameterDTO<object> parameter)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username), "El nombre de usuario no puede ser nulo o vacío");
            }
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter), "El parámetro no puede ser nulo");
            }

            username = username.ToLowerInvariant();

            using (SqlConnection conn = DataAccessBLL.CreateConnection())
            {
                conn.Open();
                return _DataAccess.UpdateParameter(conn, username, parameter.Type, parameter.Value.ToString());
            }
        }

        public static UserParameterDTO<UserParameterValueType> GetParameter<UserParameterValueType>(string username, UserParameterType type)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username), "El nombre de usuario no puede ser nulo o vacío");
            }

            username = username.ToLowerInvariant();

            using (SqlConnection conn = DataAccessBLL.CreateConnection())
            {
                conn.Open();
                string value = _DataAccess.GetParameter(conn, username, type);
                if (value != null)
                {
                    UserParameterValueType typedValue = (UserParameterValueType)Enum.Parse(typeof(UserParameterValueType), value);
                    return new UserParameterDTO<UserParameterValueType>(type, typedValue);
                }
                return null;
            }
        }

        public static bool DeleteUserParameters(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username), "El nombre de usuario no puede ser nulo o vacío");
            }

            username = username.ToLowerInvariant();

            using (SqlConnection conn = DataAccessBLL.CreateConnection())
            {
                conn.Open();
                return _DataAccess.DeleteUserParameters(conn, username);
            }
        }
    }
}
