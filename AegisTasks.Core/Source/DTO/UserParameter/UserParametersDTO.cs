using AegisTasks.Core.DTO;
using System.Collections.Generic;

public class UserParametersDTO
{
    public string Username { get; set; }
    public Dictionary<UserParameterType, object> Parameters { get; set; }
        = new Dictionary<UserParameterType, object>();

    public UserParametersDTO(string username)
    {
        Username = username;
    }

    public void SetParameter<T>(UserParameterDTO<T> parameter)
    {
        Parameters[parameter.Type] = parameter;
    }

    public bool TryGetParameter<T>(UserParameterType type, out UserParameterDTO<T> parameter)
    {
        parameter = null;
        if (Parameters.TryGetValue(type, out object obj) && obj is UserParameterDTO<T> dto)
        {
            parameter = dto;
            return true;
        }
        return false;
    }
}