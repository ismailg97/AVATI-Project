namespace AVATI.Data
{
    public class LoginService : ILoginService
    {
        public bool LogIn(string username, string password)
        {
            if (username == "Max Mustermann" && password == "123456")
            {
                return true;
            }
            else return false;
        }
         
    }
}