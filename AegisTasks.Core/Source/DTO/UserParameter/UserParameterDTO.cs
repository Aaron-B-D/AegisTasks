using System;
using System.Collections.Generic;

namespace AegisTasks.Core.DTO
{
    public enum UserParameterType
    {
        LANGUAGE
    }

    public class UserParameterDTO<UserParameterValueType>
    {
        public UserParameterValueType Value { get; set; }
        public UserParameterType Type { get; set; }

        public UserParameterDTO(UserParameterType type, UserParameterValueType value)
        {
            Type = type;
            Value = value;
        }

        public UserParameterDTO() { }
    }
}
