using System.Data.Common;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AVATI.Data
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration _configuration;
        
        public LoginService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public DbConnection GetConnection()
        {
            return new SqlConnection
                (_configuration.GetConnectionString("AVATI-Database"));
        }
        
        public int LogIn(string username, string password)
        {
            using DbConnection db = GetConnection();
            var un = db.Query<string>("SELECT Username FROM Login WHERE Username=@usern", new {usern = username}).ToList();
            if (un.FirstOrDefault() == null)
            {
                return -1;
            }

            var pw = db.Query<string>("SELECT Password FROM Login WHERE Password = @passw ", new {passw = password})
                .ToList();
            if (pw.FirstOrDefault() == null)
            {
                return -1;
            }

            var id = db.Query<int>("SELECT EmployeeID FROM Login WHERE Username = @usern AND Password=@passw ",
                new {usern = username, passw = password}).ToList();
            return id[0];
        }

        public string Login_EmpType(int Id)
        {
            using DbConnection db = GetConnection();
            var empType = db.Query<string>("SELECT EmpType FROM Employee WHERE EmployeeID=@id ", new {id = Id}).ToList();
            return empType[0];
        }

        public bool CreateLogIn(int employeeID, string username, string password)
        {
            using DbConnection db = GetConnection();
            db.Query("INSERT INTO Login VALUES (@id, @user, @pass)", new
            {
                id = employeeID, user = username, pass = password
            });
            return true;
        }
    }
}