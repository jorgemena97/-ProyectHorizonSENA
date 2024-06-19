namespace Authentication;

public static class LoginManager
{
    public static bool ValidateLogin(string email, string password,
                                     string userEmail, string userPassword)
    {
        if(userEmail == email && userPassword == password)
        {
            return true;
        }

        return false;
    }
}
