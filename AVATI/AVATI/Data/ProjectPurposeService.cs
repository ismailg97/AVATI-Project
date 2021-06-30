using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Dapper;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AVATI.Data
{
    public class ProjectPurposeService
    {
        private readonly IConfiguration _configuration;
        public List<ProjectPurpose> Purposes;

        public ProjectPurposeService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbConnection GetConnection()
        {
            return new SqlConnection
                (_configuration.GetConnectionString("AVATI-Database"));
        }
        
        public ProjectPurpose CreatePurpose(string description, int ProjectID)
        {
            ProjectPurpose temp = new ProjectPurpose();
            temp.Purpose = description;
            temp.ProjectID = ProjectID;
            Purposes.Add(temp);
            return temp;
        }

        public bool UpdatePurpose(ProjectPurpose purpose)
        {
            using DbConnection db = GetConnection();
            var result = db.Query("SELECT * FROM Projectpurpose where Purpose = @purpose AND ProjectID = @proID",
                new {purpose = purpose.Purpose, proID = purpose.ProjectID});
            if (result.FirstOrDefault() == null)
            {
                return false;
            }
            
            db.Execute("update ProjectPurpose set Purpose = @purp, ProjectActivity = @purpActiv where ProjectID = @proID ", 
                new { purp = purpose.Purpose, purActiv = purpose.AssignedProjectActivity, proID = purpose.ProjectID});
            return true;
        }
        

        public bool DeletePurpose(ProjectPurpose purpose)
        {
            using DbConnection db = GetConnection();
            var result = db.Query<ProjectPurpose>
            ("DELETE FROM Projectpurpose WHERE ProjectID = @proID and Purpose = @purpo",
                new {proID = purpose.ProjectID, purpo = purpose.Purpose});
            if (result.FirstOrDefault() == null)
            {
                return false;
            }
            db.Execute("DELETE FROM Projectpurpose WHERE ProjectID = @proID and Purpose = @purpo",
                new {proID = purpose.ProjectID, purpo = purpose.Purpose});
            return true;
        }

        public bool AddProjectActivityToPurpose(ProjectPurpose purpose)
        {
            using DbConnection db = GetConnection();
            List<string> tempPurpose = db.Query<string>("Select * from Projectpurpose where ProjectID = @proID", 
                new {proID = purpose.ProjectID}).ToList();
            
            if (!tempPurpose.Exists(x => x == purpose.Purpose))
            {
                db.Execute("INSERT INTO Projectpurpose VALUES(@ProID, @purp, NULL)", 
                    new {ProID = purpose.ProjectID, purp = purpose.Purpose});
            }
            return true;
        }

        public List<ProjectPurpose> GettAllPurposesFromProject(int projectID)
        {
            List<ProjectPurpose> _Purposes;
            using DbConnection db = GetConnection();

            _Purposes = new List<ProjectPurpose>(db.Query<ProjectPurpose>("Select * from Projectpurpose where ProjectID = @proID",
                new {proID = projectID}).ToList());
            
            return _Purposes;
        }
        
    }
}