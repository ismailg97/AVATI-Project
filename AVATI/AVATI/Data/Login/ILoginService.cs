namespace AVATI.Data
{
    public interface ILoginService
    {
        /// <summary>
        /// Checks whether specified Username & Password - Combination exists in Database
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Id of Employee</returns>
        public int LogIn(string username, string password);

        /// <summary>
        /// Returns EmployeeType of specific employee
        /// </summary>
        /// <param name="Id">Identifier Employee</param>
        /// <returns>Category in string format</returns>
        public string Login_EmpType(int Id);

        /// <summary>
        /// Creates a new account with specified username and password in DB for employeeId
        /// </summary>
        /// <param name="employeeID">Corresponding EmployeeId</param>
        /// <param name="username">Specified Username</param>
        /// <param name="password">Specified Password</param>
        /// <returns></returns>
        public bool CreateLogIn(string username, string password);

        /// <summary>
        /// Checks if passed username is already in Use
        /// </summary>
        /// <param name="username">Specified Username</param>
        /// <returns>true, if username is unused</returns>
        bool CheckUsernameAvailable(string username);

        /// <summary>
        /// Deletes Login Entry with passed Username from Database
        /// </summary>
        /// <param name="username">Specified Username</param>
        /// <returns>true, if Login Entry was deleted succesfully</returns>
        bool DeleteLogin(string username);
    }
}