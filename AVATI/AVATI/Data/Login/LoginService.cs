using System;
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
        public string _connection;
        
        public LoginService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public LoginService(string connect)
        {
            _connection = connect;
        }
        
        public DbConnection GetConnection()
        {
            if (_connection != null)
            {
                return new SqlConnection(_connection);
            }
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

            var id = db.Query<string>("SELECT Username FROM Login WHERE Username = @usern AND Password=@passw AND EmployeeID is null ",
                new {usern = username, passw = password}).ToList();
            if (id.FirstOrDefault() == username)
            {
                Console.WriteLine("test");
                return -2;
            }
            int Id = db.Query<int>("SELECT EmployeeID FROM Login WHERE Username = @usern AND Password=@passw ",
                new {usern = username, passw = password}).SingleOrDefault();
            return Id;
        }

        public string Login_EmpType(int Id)
        {
            using DbConnection db = GetConnection();
            var empType = db.Query<string>("SELECT EmpType FROM Employee WHERE EmployeeID=@id ", new {id = Id}).ToList();
            return empType[0];
        }

        public bool CreateLogIn( string username, string password)
        {
            using DbConnection db = GetConnection();

            if (username.Length > 70 || username is null or "")
            {
                return false;
            }
            if (password.Length > 70 || password is null or "")
            {
                return false;
            }
            
            if (CheckUsernameAvailable(username))
            {
               db.Query("INSERT INTO Login VALUES (NULL, @user, @pass)", new
                               {
                                   user = username, pass = password
                               });
                               return true; 
            }

            return false;


            
        }

        public bool CheckUsernameAvailable(string username)
        {
            using DbConnection db = GetConnection();
            
            if ( username is null or "" || username.Length > 70 )
            {
                return false;
            }
            
            if (db.Query<string>("Select Username From Login Where Username=@user ", new
            {
                user = username
            }).FirstOrDefault() != null)
            {
                return false;
            }

            return true;
        }

        public bool DeleteLogin(string username)
        {
            using DbConnection db = GetConnection();
            
            
            if (username.Length > 70 || username is null or "")
            {
                return false;
            }
            
            
            if (!CheckUsernameAvailable(username))
            {
              db.Query("Delete from Login Where Username=@user", new
                          {
                              user = username
                          });  
              return true;
            }

            return false;

        }
    }
}