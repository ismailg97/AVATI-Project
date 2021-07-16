namespace AVATI.Data
{
    public interface ILoginService
    {
        public int LogIn(string username, string password);

        public string Login_EmpType(int Id);

        public bool CreateLogIn(int employeeID, string username, string password);
    }
}