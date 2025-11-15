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

    public void SetParameter<UserParameterValueType>(UserParameterDTO<UserParameterValueType> parameter)
    {
        Parameters[parameter.Type] = parameter;
    }

    public bool TryGetParameter<UserParameterValueType>(UserParameterType type, out UserParameterDTO<UserParameterValueType> parameter)
    {
        parameter = null;
        if (Parameters.TryGetValue(type, out object obj) && obj is UserParameterDTO<UserParameterValueType> dto)
        {
            parameter = dto;
            return true;
        }
        return false;
    }
}