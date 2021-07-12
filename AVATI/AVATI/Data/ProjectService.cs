using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AVATI.Data
{
    public class ProjectService : IProjektService
    {
        public List<Project> Projects { get; set; }
        private string ConnectionString; //global connectionstring
        
        public ProjectService(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("AVATI-Database");
        }
        public ProjectService(string connection) //for testing database connections -> for testpurposes
        {
            ConnectionString = connection;
        }
        
        public DbConnection GetConnection()
        {
            return new SqlConnection
                (ConnectionString);
        }

        private bool ExistActivityInProject(int projectId, string activity)
        {
            using var db = GetConnection();
            return db.Query<int>("SELECT COUNT(*) FROM ProjectActivity_Project_Employee WHERE ProjectID = @project AND ProjectActivity = @description",
                new{ project = projectId, description = activity}).Single() > 0;
        }

        public bool CreateProject(Project project)
        {
            if (project == null)
            {
                return false;
            }

            using DbConnection db = GetConnection();
            db.Execute("INSERT INTO Project VALUES(@title, @description, @dateBeg , @dateEnd)",
                new
                {
                    title = project.Projecttitel, description = project.Projectdescription,
                    dateBeg = project.Projectbeginning.ToString("d", DateTimeFormatInfo.InvariantInfo),
                    dateEnd = project.Projectend.ToString("d", DateTimeFormatInfo.InvariantInfo)
                });
            var projectId = db.QuerySingle<int>("SELECT max(ProjectID) FROM Project");
            foreach (var emp in project.Employees)
            {
                Console.WriteLine(emp.FirstName);
                db.Execute("INSERT INTO ProjectActivity_Project_Employee VALUES(@ProID, @EmplId, NULL)",
                    new {ProID = projectId, EmplId = emp.EmployeeID});
            }

            foreach (var field in project.Fields)
            {
                Console.WriteLine(field);
                db.Execute("INSERT INTO Project_Field VALUES(@pro, @f)", new {f = field, pro = projectId});
            }

            return true;
        }

        public bool UpdateProject(Project project)
        {
            foreach (var emp in project.Employees)
            {
                Console.WriteLine(emp.FirstName);
            }
            using IDbConnection db = GetConnection();
            var result = db.Query<Project>("SELECT * FROM Project WHERE ProjectId = @propId",
                new {propId = project.ProjectID});
            if (result.FirstOrDefault() == null)
            {
                Console.WriteLine("we have a problem");
                return false;
            }

            db.Execute(
                "update Project set ProjectTitle = @propTitle, Projectdescription = @addInfo, Projectbegin = @beg, Projectend = @end where ProjectID = @propId",
                new
                {
                    propTitle = project.Projecttitel ?? "Leer",
                    addInfo = project.Projectdescription ?? "[Keine Zusatzinformationen]", propId = project.ProjectID,
                    beg = project.Projectbeginning.ToString("d", DateTimeFormatInfo.InvariantInfo),
                    end = project.Projectend.ToString("d", DateTimeFormatInfo.InvariantInfo)
                });
            var tempIds =
                db.Query<int>("SELECT EmployeeID from ProjectActivity_Project_Employee WHERE ProjectID = @prop AND EmployeeID IS NOT NULL ",
                    new {prop = project.ProjectID}).ToList();

            var activities = new List<string>();
            foreach (var empId in tempIds)
            {
                if (!project.Employees.Exists(e => e.EmployeeID == empId))
                {
                    activities.AddRange(db.Query<string>(
                        "SELECT ProjectActivity FROM ProjectActivity_Project_Employee WHERE ProjectID = @pro AND EmployeeID = @empo",
                        new{ pro = project.ProjectID, empo = empId}).ToList());
                    db.Execute(
                        "DELETE FROM ProjectActivity_Project_Employee WHERE ProjectID = @pro AND EmployeeID = @empo",
                        new {pro = project.ProjectID, empo = empId});
                }
            }

            foreach (var activity in activities)
            {
                if (!ExistActivityInProject(project.ProjectID, activity))
                    db.Execute("INSERT INTO ProjectActivity_Project_Employee VALUES(@ProID, NULL, @description)",
                        new {ProID = project.ProjectID, description = activity});
            }

            foreach (var pro in project.Employees)
            {
                if (!tempIds.Exists(e => e == pro.EmployeeID))
                {
                    db.Execute("INSERT INTO ProjectActivity_Project_Employee VALUES(@ProID, @EmplId, NULL)",
                        new {ProID = project.ProjectID, EmplId = pro.EmployeeID});
                }
            }

            var tempFields = db.Query<string>("SELECT Field FROM Project_Field WHERE ProjectID = @projectId",
                new{ projectId = project.ProjectID });

            var copyFields = new List<string>(project.Fields);

            foreach (var field in tempFields)
            {
                if (!project.Fields.Exists(x => x == field))
                {
                    var deleteRows = db.Execute("DELETE FROM Project_Field WHERE ProjectID = @projectId AND Field = @description",
                        new{ projectId = project.ProjectID, description = field});
                    if (deleteRows != 1) return false;
                    continue;
                }

                copyFields.Remove(field);
            }

            foreach (var field in copyFields)
            {
                var insertRows = db.Execute("INSERT INTO Project_Field WHERE ProjectID = @projectId AND Field = @description",
                    new{ projectId = project.ProjectID, description = field});
                if (insertRows != 1) return false;
            }

            return true;
        }

        public bool DeleteProject(int projectId)
        {
            using IDbConnection db = GetConnection();
            var result = db.Query<Project>("SELECT * FROM Project WHERE ProjectId = @propId",
                new {propId = projectId});
            if (result.FirstOrDefault() == null)
            {
                Console.WriteLine("we have a problem");
                return false;
            }

            db.Execute(
                "DELETE FROM ProjectActivity_Hardskill WHERE ProjectActivityID IN (SELECT ProjectActivityID FROM ProjectActivity_Project_Employee WHERE ProjectID = @project)",
                new { project = projectId});
            
            db.Execute(
                "DELETE FROM ProjectActivity_Softskill WHERE ProjectActivityID IN (SELECT ProjectActivityID FROM ProjectActivity_Project_Employee WHERE ProjectID = @project)",
                new { project = projectId});

            db.Execute("DELETE FROM Project WHERE ProjectID = @proId", new {proId = projectId});
            return true;
        }

        public Project GetProject(int projectId)
        {
            Project temp;
            using IDbConnection db = GetConnection();
            temp = new Project();
            temp.Projecttitel = db.QuerySingle<string>("SELECT Projecttitle from Project WHERE ProjectId = @proId",
                new {proId = projectId});
            temp.Projectdescription = db.QuerySingle<string>(
                "SELECT Projectdescription from Project WHERE ProjectId = @proId",
                new {proId = projectId});
            temp.Projectbeginning = db.QuerySingle<DateTime>(
                "SELECT Projectbegin from Project WHERE ProjectId = @proId",
                new {proId = projectId});
            temp.Projectend = db.QuerySingle<DateTime>("SELECT Projectend from Project WHERE ProjectId = @proId",
                new {proId = projectId});
            temp.Fields = db.Query<string>("SELECT Field from Project_Field WHERE ProjectID = @pro",
                new {pro = projectId}).ToList();
            temp.ProjectID = projectId;
            List<int> employeeIds = 
                db.Query<int>("SELECT EmployeeId FROM ProjectActivity_Project_Employee WHERE ProjectID = @pro AND EmployeeID IS NOT NULL",
                    new {pro = projectId}).ToList();
            foreach (var idtolookfor in employeeIds)
            {
                temp.Employees.Add(db.Query<Employee>("SELECT * FROM Employee WHERE EmployeeID = @emp", new {emp = idtolookfor}).FirstOrDefault());
            }
            return temp;
        }

        public List<Project> GetAllProjects()
        {
            using IDbConnection db = GetConnection();
            Projects = new List<Project>(db.Query<Project>("SELECT ProjectID from Project"));
            foreach (var temp in Projects)
            {
                temp.Projecttitel = db.QuerySingle<string>("SELECT Projecttitle from Project WHERE ProjectId = @proId",
                    new {proId = temp.ProjectID});
                temp.Projectdescription = db.QuerySingle<string>(
                    "SELECT Projectdescription from Project WHERE ProjectId = @proId",
                    new {proId = temp.ProjectID});
                temp.Projectbeginning = db.QuerySingle<DateTime>(
                    "SELECT Projectbegin from Project WHERE ProjectId = @proId",
                    new {proId = temp.ProjectID});
                temp.Projectend = db.QuerySingle<DateTime>("SELECT Projectend from Project WHERE ProjectId = @proId",
                    new {proId = temp.ProjectID});
                temp.Fields = db.Query<string>("SELECT Field from Project_FIeld WHERE ProjectID = @pro",
                    new {pro = temp.ProjectID}).ToList();
            }

            return Projects;
        }
        

        public List<string> GetAllFieldsFromOneProject(int projectId)
        {
            using IDbConnection db = GetConnection();
            //TODO List fehlt
            List<string> fields = new List<string>();
            return fields;
        }

        public bool DeleteEmployeeFromProject(int projectId, int employeeId)
        {
            using IDbConnection db = GetConnection();
            var activities = db.Query<string>("SELECT ProjectActivity FROM ProjectActivity_Project_Employee WHERE ProjectID = @pro AND EmployeeID = @empo",
                new {pro = projectId, empo = employeeId}).ToList();
            db.Execute("DELETE FROM ProjectActivity_Project_Employee WHERE ProjectID = @pro AND EmployeeID = @emp",
                new {pro = projectId, emp = employeeId});
            foreach (var activity in activities)
            {
                if (!ExistActivityInProject(projectId, activity))
                    db.Execute("INSERT INTO ProjectActivity_Project_Employee VALUES(@ProID, NULL, @description)",
                        new {ProID = projectId, description = activity});
            }
            return true;
        }

        public bool UpdateFieldsFromProject(int projectId, List<string> fields)
        {
            using IDbConnection db = GetConnection();
            db.Open();
            var listInProject = db.Query<string>("SELECT Field from Project_Field WHERE ProjectID = @pro",
                new {pro = projectId}).ToList();
            foreach (var pro in listInProject)
            {
                if (!fields.Exists(e => e.Equals(pro)))
                {
                    db.Execute("Delete FROM Project_Field WHERE Field = @f", new {f = pro});
                }
            }

            foreach (var fieldAdd in fields)
            {
                if (!listInProject.Exists(e => e.Equals(fieldAdd)))
                {
                    db.Execute("INSERT INTO Project_Field VALUES(@pro, @f)", new {f = fieldAdd, pro = projectId});
                }
            }

            return true;
        }
    }
}