using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Drawing;
using Dapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Extensions.Configuration;

namespace AVATI.Data
{
    public class EmployeeService : IEmployeeService
    {
        private ProjectActivityService2 _projectActivityService;
        private string _connection;
        private readonly IConfiguration _configuration;
        public List<Employee> Employees { get; set; }


        public EmployeeService(IConfiguration configuration)
        {
            _configuration = configuration;
            _projectActivityService = new ProjectActivityService2(_configuration.GetConnectionString("AVATI-Database"));
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
        
        public EmployeeService(string connect)
        {
            _projectActivityService = new ProjectActivityService2(connect);
            _connection = connect;
        }

        
        public List<Employee> GetAllEmployees()
        {
            using DbConnection db = GetConnection();
            List<Employee> empList = new List<Employee>();
            var employees = db.Query<Employee>("select * from Employee").ToList();
            foreach (var emp in employees)
            {
                Employee employee;

                var employeeMain =
                    db.Query<Employee>("select * from Employee where EmployeeId = @ID", new {ID = emp.EmployeeID})
                        .ToList();
                employee = employeeMain[0];

                var fields = db.Query<string>("select Field from Employee_Field where EmployeeId = @ID",
                    new {ID = emp.EmployeeID}).ToList();
                foreach (var field in fields)
                {
                    employee.Field.Add(field);
                }

                var hardskills = db.Query<string>("select Hardskill from Employee_Hardskill where EmployeeId = @ID",
                    new {ID = emp.EmployeeID}).ToList();
                foreach (var hardskill in hardskills)
                {
                    Hardskill h = new Hardskill() {Description = hardskill};
                    var level = db
                        .Query<int>(
                            "select Employee_Hardskill.Level from Employee_Hardskill  where EmployeeId = @ID AND Hardskill = @desc",
                            new {ID = emp.EmployeeID, desc = hardskill}).ToList();
                    employee.Hardskills.Add(h);
                    employee.HardSkillLevel.Add(new Tuple<Hardskill, int>(h, level.First()));
                }

                var languages =
                    db.Query<string>("SELECT Employee_Language.Language FROM Employee_Language where EmployeeId = @ID ",
                        new {ID = emp.EmployeeID}).ToList();
                foreach (var language in languages)
                {
                    var level = db
                        .Query<string>(
                            "SELECT Employee_Language.Level FROM Employee_Language where EmployeeId = @ID AND Employee_Language.Language = @desc ",
                            new {ID = emp.EmployeeID, desc = language}).ToList();
                    employee.Language.Add(new Tuple<string, LanguageLevel>(language, GetLanguageLevel(level.First())));
                }

                var softskills = db.Query<string>("select Softskill from Employee_Softskill where EmployeeId = @ID",
                    new {ID = emp.EmployeeID}).ToList();
                foreach (var softskill in softskills)
                {
                    employee.Softskills.Add(softskill);
                }

                var roles = db.Query<string>("select Role from Employee_Role where EmployeeId = @ID",
                    new {ID = emp.EmployeeID}).ToList();
                foreach (var role in roles)
                {
                    employee.Roles.Add(role);
                }

                var rc = db.Query<int>("SELECT RCLevel from Employee WHERE EmployeeId = @ID",
                    new {ID = emp.EmployeeID}).FirstOrDefault();
                employee.Rc = rc;
                employee.EmployeeID = emp.EmployeeID;
                empList.Add(employee);
            }


            return empList;
        }

        public int CreateEmployeeProfile(Employee emp, string username)
        {
            using DbConnection db = GetConnection();
            db.Query(
                "INSERT INTO Employee VALUES ( @Firstname ,@Lastname ,@RWE, @EmpTime,@Rc,@EmpType, @IA, @img)",
                new
                {
                     Firstname = emp.FirstName, Lastname= emp.LastName, img = emp.Image ,
                    RWE = emp.RelevantWorkExperience, EmpTime = emp.EmploymentTime, EmpType = emp.EmpType.ToString(), Rc = emp.Rc,
                    IA = emp.IsActive
                });

            var id = db.QuerySingle<int>(
                "Select EmployeeID From Employee WHERE Firstname = @Firstname AND Lastname = @Lastname", new
                {
                    Firstname = emp.FirstName, LastName = emp.LastName
                });
            
            foreach (var field in emp.Field)
            {
                db.Query("INSERT INTO Employee_Field VALUES (@ID, @fields)", new {ID = id, fields = field});
            }

            foreach (var hardskill in emp.HardSkillLevel)
            {
                db.Query(
                    "INSERT INTO Employee_HardSkill VALUES (@ID, @Desc,@Level )",
                    new {ID = id, Desc = hardskill.Item1.Description, Level = hardskill.Item2});
            }

            foreach (var language in emp.Language)
            {
                db.Query("INSERT INTO Employee_Language VALUES (@ID, @DESC, @LEVEL)",
                    new {ID = id, DESC = language.Item1, LEVEL = language.Item2.ToString()});
            }
            

            foreach (var roles in emp.Roles)
            {
                db.Query("INSERT INTO Employee_Role VALUES (@ID, @role)", new {ID = id, role = roles});
            }

            foreach (var softskills in emp.Softskills)
            {
                db.Query("INSERT INTO Employee_Softskill VALUES (@ID, @softskill)",
                    new {ID = id, softskill = softskills});
            }

            db.Query("UPDATE Login SET EmployeeID=@ID WHERE Username=@user", new { ID = id, user = username});

            return id;
        }

        public bool EditEmployeeProfile(Employee emp)
        {
            using DbConnection db = GetConnection();
            
            db.Query(
                "UPDATE Employee SET Firstname= @Firstname ,Lastname = @Lastname , Image = @img , WorkExperience = @RWE, EmploymentTime = @EmpTime,RCLevel = @RC, IsActive = @IA WHERE EmployeeID = @ID",
                new
                {
                    ID = emp.EmployeeID, Firstname = emp.FirstName, Lastname = emp.LastName, IMAGE = emp.Image,
                    RWE = emp.RelevantWorkExperience, EmpTime = emp.EmploymentTime, RC = emp.Rc, IA = emp.IsActive, img = emp.Image
                });
            
            

            db.Query("DELETE FROM Employee_Field WHERE EmployeeID = @ID", new {ID = emp.EmployeeID});
            if (emp.Field.Any())
            {
                foreach (var field in emp.Field)
                {
                    db.Query("INSERT INTO Employee_Field VALUES (@ID, @FIELD)",
                        new {ID = emp.EmployeeID, FIELD = field});
                }
            }

            List<string> HardSkillList = new List<string>(db.Query<string>(
                "SELECT Hardskill FROM Employee_Hardskill WHERE EmployeeID =@id", new {id = emp.EmployeeID}));
            foreach (var hard in HardSkillList)
            {
                if (emp.Hardskills.Find(x => x.Description == hard) == null)
                {
                    db.Query("DELETE FROM Employee_Hardskill WHERE Hardskill = @hardsk AND EmployeeID=@id",
                        new {hardsk = hard, id = emp.EmployeeID});
                } 
            }
            
            foreach (var hard in emp.HardSkillLevel)
            {
                if (HardSkillList.Find(x => x.Equals(hard.Item1.Description)) == null)
                {
                    db.Query("INSERT INTO Employee_Hardskill VALUES (@id, @hardsk,@level)",
                        new {hardsk = hard.Item1.Description, id = emp.EmployeeID, level=hard.Item2});
                }
                else
                {
                    db.Query("UPDATE Employee_Hardskill SET Level = @level WHERE Hardskill = @hardsk AND EmployeeID = @id",
                        new {hardsk = hard.Item1.Description, id = emp.EmployeeID, level=hard.Item2});
                }
            }
            
            
            //db.Query("DELETE FROM Employee_Hardskill WHERE EmployeeID = @ID", new {ID = emp.EmployeeID});
            //if (emp.HardSkillLevel.Any())
            //{
            //    foreach (var hardskill in emp.HardSkillLevel)
            //    {
            //db.Query("INSERT INTO Employee_Hardskill VALUES (@ID, @DESC, @LEVEL)",
            //            new {ID = emp.EmployeeID, DESC = hardskill.Item1.Description, LEVEL = hardskill.Item2});
            //    }
            //}

            db.Query("DELETE FROM Employee_Language WHERE EmployeeID = @ID", new {ID = emp.EmployeeID});
            if (emp.Language.Any())
            {
                foreach (var language in emp.Language)
                {
                    db.Query("INSERT INTO Employee_Language VALUES (@ID, @DESC, @LEVEL)",
                        new {ID = emp.EmployeeID, DESC = language.Item1, LEVEL = language.Item2.ToString()});
                }
            }

            db.Query("DELETE FROM Employee_Role WHERE EmployeeID = @ID", new {ID = emp.EmployeeID});
            if (emp.Roles.Any())
            {
                foreach (var role in emp.Roles)
                {
                    db.Query("INSERT INTO Employee_Role VALUES (@ID, @ROLE)",
                        new {ID = emp.EmployeeID, ROLE = role});
                }
            }

            List<string> SoftSkillList = new List<string>(db.Query<string>(
                "SELECT Softskill FROM Employee_Softskill WHERE EmployeeID =@id", new {id = emp.EmployeeID}));
            foreach (var soft in SoftSkillList)
            {
                if (emp.Softskills.Find(x => x.Equals(soft)) == null)
                {
                    db.Query("DELETE FROM Employee_Softskill WHERE Softskill = @softsk AND EmployeeID=@id",
                        new {softsk = soft, id = emp.EmployeeID});
                } 
            }
            
            foreach (var soft in emp.Softskills)
            {
                if (SoftSkillList.Find(x => x.Equals(soft)) == null)
                {
                    db.Query("INSERT INTO Employee_Softskill VALUES(@id, @softsk)",
                        new {softsk = soft, id = emp.EmployeeID});
                } 
            }
            
            //db.Query("DELETE FROM Employee_Softskill WHERE EmployeeID = @ID", new {ID = emp.EmployeeID});
            //if (emp.Softskills.Any())
            //{
            //    foreach (var softskill in emp.Softskills)
            //    {
            //        db.Query("INSERT INTO Employee_Softskill VALUES (@ID, @SOFTSKILL)",
            //            new {ID = emp.EmployeeID, SOFTSKILL = softskill});
            //    }
            //}
            //-----------ProjectActivities-----------
            var oldActivities = _projectActivityService.GetProjectActivitiesOfEmployee(emp.EmployeeID);
            var newActivities = new List<ProjectActivity>(emp.ProjectActivities);
            
            foreach (var oldActivity in oldActivities)
            {
                var newActivity = newActivities.Find(x => x.ProjectActivityID == oldActivity.ProjectActivityID);
                if (newActivity == null)
                {
                    continue;
                };

                if (oldActivity.Description != null && newActivity.Description == oldActivity.Description)
                {
                    _projectActivityService.UpdateSkillsToActivity(newActivity.ProjectActivityID, newActivity.HardSkills, newActivity.SoftSkills);
                } 
                else if (oldActivity.Description != null && newActivity.Description == null)
                {
                    _projectActivityService.DeleteProjectActivityToEmployee(oldActivity.ProjectActivityID);
                }

                newActivities.Remove(newActivity);
            }

            foreach (var activity in newActivities)
            {
                if (activity.Description == null) continue;
                _projectActivityService.SetProjectActivityToEmployee(activity);
            }
            //--------------------------------------------

            return true;
        }

        public Employee GetEmployeeProfile(int employeeId)
        {
            using DbConnection db = GetConnection();

            Employee employee = new Employee();

            var employees = db.Query<Employee>("select * from Employee where EmployeeID = @ID", new {ID = employeeId})
                .ToList();
            employee = employees[0];

            var employeesRC =
                db.Query<int>("select RCLevel from Employee where EmployeeID = @ID", new {ID = employeeId}).ToList();
            employee.Rc = employeesRC[0];

            var employeesWE =
                db.Query<float>("select WorkExperience from Employee where EmployeeID = @ID", new {ID = employeeId})
                    .ToList();
            employee.RelevantWorkExperience = employeesWE[0];
            

            var fields = db
                .Query<string>("select Field from Employee_Field where EmployeeId = @ID", new {ID = employeeId})
                .ToList();
            foreach (var field in fields)
            {
                employee.Field.Add(field);
            }

            var hardskills = db.Query<string>("select Hardskill from Employee_Hardskill where EmployeeId = @ID",
                new {ID = employeeId}).ToList();
            foreach (var hardskill in hardskills)
            {
                Hardskill h = new Hardskill() {Description = hardskill};
                var level = db
                    .Query<int>(
                        "select Employee_Hardskill.Level from Employee_Hardskill  where EmployeeId = @ID AND Hardskill = @desc",
                        new {ID = employeeId, desc = hardskill}).ToList();
                employee.Hardskills.Add(h);
                employee.HardSkillLevel.Add(new Tuple<Hardskill, int>(h, level.First()));
            }

            var languages =
                db.Query<string>("SELECT Employee_Language.Language FROM Employee_Language where EmployeeId = @ID ",
                    new {ID = employeeId}).ToList();
            foreach (var language in languages)
            {
                var level = db
                    .Query<string>(
                        "SELECT Employee_Language.Level FROM Employee_Language where EmployeeId = @ID AND Employee_Language.Language = @desc ",
                        new {ID = employeeId, desc = language}).ToList();
                employee.Language.Add(new Tuple<string, LanguageLevel>(language, GetLanguageLevel(level.First())));
            }

            var softskills = db.Query<string>("select Softskill from Employee_Softskill where EmployeeId = @ID",
                new {ID = employeeId}).ToList();
            foreach (var softskill in softskills)
            {
                employee.Softskills.Add(softskill);
            }

            var roles = db.Query<string>("select Role from Employee_Role where EmployeeId = @ID", new {ID = employeeId})
                .ToList();
            foreach (var role in roles)
            {
                
                employee.Roles.Add(role);
            }

            employee.ProjectActivities = _projectActivityService.GetProjectActivitiesOfEmployee(employeeId);

            return employee;
        }

        public bool EditStatus(int employeeId, bool status)
        {
            using DbConnection db = GetConnection();
            db.Query<Employee>("update Employee set IsActive=@active where EmployeeID = @ID",
                new {ID = employeeId, active = status});
            return true;
        }

        public string GetDefaultPicture()
        {
            using DbConnection db = GetConnection();
            string defaultImage =
                db.QuerySingle<string>("SELECT Image FROM Employee WHERE Firstname = 'PLACE' AND Lastname = 'HOLDER'");
            return defaultImage;
        }

        public bool? GetSatus(int employeeId)
        {
            using DbConnection db = GetConnection();
            var status = db.Query<byte>("select IsActive from Employee where EmployeeId = @ID", new {ID = employeeId})
                .ToList();
            if (status[0] == 1)
            {
                return true;
            }
            else return false;
        }

        public LanguageLevel GetLanguageLevel(string s)
        {
            if (s == "A1")
            {
                return LanguageLevel.A1;
            }
            else if (s == "A2")
            {
                return LanguageLevel.A2;
            }
            else if (s == "B1")
            {
                return LanguageLevel.B1;
            }
            else if (s == "B2")
            {
                return LanguageLevel.B2;
            }
            else if (s == "C1")
            {
                return LanguageLevel.C1;
            }
            else return LanguageLevel.C2;
        }
        
    }
}