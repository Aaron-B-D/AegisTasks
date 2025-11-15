using AegisTasks.Core.DTO;
using AegisTasks.DataAccess.DataAccesses;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace AegisTasks.BLL.DataAccess
{
    public static class UserDataAccessBLL
    {
        private static readonly UsersDataAccess _UsersDataAccess = new UsersDataAccess();

        static UserDataAccessBLL() { }

        public static bool CreateUser(UserDTO user)
        {
            string validationError;
            if (!isPasswordValid(user.Password, out validationError))
            {
                throw new ArgumentException("Contraseña inválida: " + validationError);
            }

            user.Username = user.Username.ToLowerInvariant();
            user.Password = hashPassword(user.Password);

            using (SqlConnection conn = DataAccessBLL.CreateConnection())
            {
                conn.Open();
                bool userCreated = _UsersDataAccess.InsertUser(conn, user);
                bool userParametersCreated = UserParametersBLL.CreateUserParameters(user.Username);
                return userCreated && userParametersCreated;
            }
        }

        public static bool DeleteUser(string username)
        {
            username = username.ToLowerInvariant();

            using (SqlConnection conn = DataAccessBLL.CreateConnection())
            {
                conn.Open();
                bool userParamsDeleted = UserParametersBLL.DeleteUserParameters(username);
                bool userDeleted = _UsersDataAccess.DeleteUser(conn, username);
                return userDeleted && userParamsDeleted;
            }
        }

        public static bool UpdatePassword(string username, string currentPassword, string newPassword)
        {
            username = username.ToLowerInvariant();

            using (SqlConnection conn = DataAccessBLL.CreateConnection())
            {
                conn.Open();

                string storedHashed = _UsersDataAccess.GetUserPassword(conn, username);
                if (!verifyPassword(currentPassword, storedHashed))
                {
                    throw new ArgumentException("La contraseña actual no es correcta.");
                }

                string validationError;
                if (!isPasswordValid(newPassword, out validationError))
                {
                    throw new ArgumentException("Contraseña inválida: " + validationError);
                }

                string hashedPassword = hashPassword(newPassword);
                return _UsersDataAccess.UpdatePassword(conn, username, hashedPassword);
            }
        }

        public static bool CheckPassword(string username, string password)
        {
            username = username.ToLowerInvariant();

            using (SqlConnection conn = DataAccessBLL.CreateConnection())

            {
                conn.Open();
                string storedHashed = _UsersDataAccess.GetUserPassword(conn, username);
                if (verifyPassword(password, storedHashed))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static UserDTO GetUser(string username)
        {
            username = username.ToLowerInvariant();

            using (SqlConnection conn = DataAccessBLL.CreateConnection())

            {
                conn.Open();
                UserDTO user = _UsersDataAccess.GetUser(conn, username);
                if (user != null)
                {
                    // No devolvemos la contraseña
                    user.Password = null;
                }
                return user;
            }
        }

        private static bool isPasswordValid(string password, out string error)
        {
            error = "";
            if (password.Length < 8)
            {
                error = "Debe tener al menos 8 caracteres.";
                return false;
            }
            if (!Regex.IsMatch(password, "[A-Z]"))
            {
                error = "Debe contener al menos una letra mayúscula.";
                return false;
            }
            if (!Regex.IsMatch(password, "[a-z]"))
            {
                error = "Debe contener al menos una letra minúscula.";
                return false;
            }
            if (!Regex.IsMatch(password, "[0-9]"))
            {
                error = "Debe contener al menos un número.";
                return false;
            }
            if (!Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            {
                error = "Debe contener al menos un carácter especial.";
                return false;
            }
            return true;
        }

        private static string hashPassword(string password)
        {
            SHA256 sha = SHA256.Create();
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        private static bool verifyPassword(string password, string hashed)
        {
            string hashOfInput = hashPassword(password);
            if (StringComparer.Ordinal.Compare(hashOfInput, hashed) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
