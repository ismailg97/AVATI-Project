using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using AVATI.Data.EmployeeDetailFiles;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AVATI.Data
{
    public class ProjectService : IProjektService
    {
        private readonly IConfiguration _configuration;
        public List<Project> Projects { get; set; }

        public ProjectService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbConnection GetConnection()
        {
            return new SqlConnection
                (_configuration.GetConnectionString("AVATI-Database"));
        }

        public bool CreateProject(Project project)
        {
            if (project == null)
            {
                return false;
            }

            using DbConnection db = GetConnection();
            db.Open();
            db.Execute("INSERT INTO Project VALUES(@title, @description, @dateBeg , @dateEnd)",
                new
                {
                    title = project.Projecttitel, 
                    description = project.Projectdescription,
                    dateBeg = project.Projectbeginning.ToString("d", DateTimeFormatInfo.InvariantInfo),
                    dateEnd = project.Projectend.ToString("d", DateTimeFormatInfo.InvariantInfo)
                });
            foreach (var emp in project.Employees)
            {
            }

            return true;
        }

        public bool UpdateProject(Project project)
        {
            IDbConnection db = GetConnection();
            db.Open();
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
            List<int> tempIds =
                db.Query<int>("SELECT EmployeeID from ProjectActivity_Project_Employee WHERE ProjectID = @prop AND EmployeeID IS NOT NULL ",
                    new {prop = project.ProjectID}).ToList();
            foreach (var empID in tempIds)
            {
                if (!project.Employees.Exists(e => e.EmployeeID == empID))
                {
                    db.Execute(
                        "DELETE FROM ProjectActivity_Project_Employee WHERE ProjectID = @pro AND EmployeeID = @empo",
                        new {pro = project.ProjectID, empo = empID});
                }
            }

            foreach (var pro in project.Employees)
            {
                Console.WriteLine(pro.LastName);
                if (!tempIds.Exists(e => e == pro.EmployeeID))
                {
                    db.Execute("INSERT INTO ProjectActivity_Project_Employee VALUES(@ProID, @EmplId, NULL)",
                        new {ProID = project.ProjectID, EmplId = pro.EmployeeID});
                }
            }
            foreach (var field in project.Fields)
            {
                //TODO Wir haben noch keine Fields - Tabelle
            }

            return true;
        }

        public bool DeleteProject(int projectID)
        {
            IDbConnection db = GetConnection();
            db.Open();
            var result = db.Query<Project>("SELECT * FROM Project WHERE ProjectId = @propId",
                new {propId = projectID});
            if (result.FirstOrDefault() == null)
            {
                Console.WriteLine("we have a problem");
                return false;
            }

            //TODO Victoria fragen, ob hier cascade ist?
            db.Execute("DELETE FROM Project WHERE ProjectID = @proId", new {proId = projectID});
            return true;
        }

        public Project GetProject(int projectID)
        {
            Project temp;
            IDbConnection db = GetConnection();
            db.Open();
            temp = new Project();
            temp.Projecttitel = db.QuerySingle<string>("SELECT Projecttitle from Project WHERE ProjectId = @proId",
                new {proId = projectID});
            temp.Projectdescription = db.QuerySingle<string>(
                "SELECT Projectdescription from Project WHERE ProjectId = @proId",
                new {proId = projectID});
            temp.Projectbeginning = db.QuerySingle<DateTime>(
                "SELECT Projectbegin from Project WHERE ProjectId = @proId",
                new {proId = projectID});
            temp.Projectend = db.QuerySingle<DateTime>("SELECT Projectend from Project WHERE ProjectId = @proId",
                new {proId = projectID});
            temp.Fields = db.Query<string>("SELECT Field from Project_FIeld WHERE ProjectID = @pro",
                new {pro = projectID}).ToList();
            temp.ProjectID = projectID;
            List<int> employeeIds = 
                db.Query<int>("SELECT EmployeeId FROM ProjectActivity_Project_Employee WHERE ProjectID = @pro AND EmployeeID IS NOT NULL",
                    new {pro = projectID}).ToList();
            foreach (var idtolookfor in employeeIds)
            {
                temp.Employees.Add(db.Query<Employee>("SELECT * FROM Employee WHERE EmployeeID = @emp", new {emp = idtolookfor}).FirstOrDefault());
            }
            return temp;
        }

        public List<Project> GetAllProjects()
        {
            IDbConnection db = GetConnection();
            db.Open();
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

        public List<string> GetAllFieldsFromOneProject(int ProjectID)
        {
            IDbConnection db = GetConnection();
            db.Open();
            //TODO List fehlt
            List<string> fields = new List<string>();
            return fields;
        }

        public bool DeleteEmployeeFromProject(int ProjectID, int EmployeeID)
        {
            IDbConnection db = GetConnection();
            db.Open();
            db.Execute("DELETE FROM ProjectActivity_Project_Employee WHERE ProjectID = @pro AND EmployeeID = @emp",
                new {pro = ProjectID, emp = EmployeeID});
            return true;
        }

        public bool UpdateFieldsFromProject(int ProjectID, List<string> fields)
        {
            IDbConnection db = GetConnection();
            db.Open();
            var listInProject = db.Query<string>("SELECT Field from Project_Field WHERE ProjectID = @pro",
                new {pro = ProjectID}).ToList();
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
                    db.Execute("INSERT INTO Project_Field VALUES(@pro, @f)", new {f = fieldAdd, pro = ProjectID});
                }
            }

            return true;
        }
    }
}