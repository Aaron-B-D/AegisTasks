using AegisTasks.BLL.DataAccess;
using AegisTasks.Core.Common;
using AegisTasks.Core.DTO;
using System.Globalization;
using System.Threading;

public static class SessionManager
{
    public static UserDTO CurrentUser { get; private set; }
    public static UserParametersDTO CurrentUserParameters { get; private set; }

    public static bool Login(string username, string password)
    {
        if (UserDataAccessBLL.GetUser(username) != null)
        {
            if (UserDataAccessBLL.IsValidPassword(username, password))
            {
                CurrentUser = UserDataAccessBLL.GetUser(username);
                CurrentUserParameters = UserParametersBLL.GetParameters(username);

                applyUserSettings();

                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public static void Logout()
    {
        CurrentUser = null;
        CurrentUserParameters = null;
    }

    public static bool IsLoggedIn
    {
        get
        {
            if (CurrentUser != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private static void applyUserSettings()
    {
        if (CurrentUserParameters != null)
        {
            if (CurrentUserParameters.TryGetParameter<SupportedLanguage>(UserParameterType.LANGUAGE, out UserParameterDTO<SupportedLanguage> language))
            {
                CultureInfo culture = language.Value.ToCulture();

                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }
            else
            {
                Logger.LogError("No se ha podido establecer la configuración de idioma");
            }
        }
    }
}
