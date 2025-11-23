using System;
using System.Collections.Generic;
using System.Text;

namespace AegisTasks.Core.DTO
{
    public class UserDTO
    {
        public static readonly string ADMIN_USER_USERNAME = "admin";


        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Password { get; set; } = string.Empty;

        // Constructor por defecto
        public UserDTO() { }

        // Constructor parametrizado
        public UserDTO(string username, string firstName, string lastName, string password, DateTime createdAt)
        {
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            CreatedAt = createdAt;
        }

        public UserDTO(string username, string firstName, string lastName, string password)
        {
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
        }
    }
}
