using System;
using System.Collections.Generic;
using System.Data.Common;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AVATI.Data.EmployeeDetailFiles
{
    public class EmployeeDetailService : IEmployeeDetailService
    {
        private readonly IConfiguration _configuration;

        public DbConnection GetConnection()
        {
            return new SqlConnection
                (_configuration.GetConnectionString("AVATI-Database"));
        }

        public EmployeeDetailService(IConfiguration configuration)
        {
            _configuration = configuration;
            EmployeeDetails = new List<EmployeeDetail>();
        }

        public List<EmployeeDetail> EmployeeDetails;

        public bool UpdateEmployeeDetail(EmployeeDetail employeeDetail)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateEmployeeDetail(int employeeId, int proposalId, EmployeeDetail employeeDetail)
        {
            EmployeeDetail temp;
            if ((temp = EmployeeDetails.Find(e => e.ProposalId == proposalId && e.EmployeeId == employeeId)) == null)
            {
                EmployeeDetails.Add(new EmployeeDetail()
                {
                    EmployeeId = employeeId, ProposalId = proposalId, Rc = employeeDetail.Rc,
                    Softskills = employeeDetail.Softskills,
                    Hardskills = employeeDetail.Hardskills, Fields = employeeDetail.Fields,
                    Languages = employeeDetail.Languages,
                    Roles = employeeDetail.Roles
                });
                return true;
            }
            else
            {
                temp.Fields = employeeDetail.Fields;
                temp.Roles = employeeDetail.Roles;
                temp.Hardskills = employeeDetail.Hardskills;
                temp.Languages = employeeDetail.Languages;
                temp.Rc = employeeDetail.Rc;
                temp.Softskills = employeeDetail.Softskills;
                return true;
            }
        }

        public bool CopyDetail(int proposalId, int newId, int emp)
        {
            using DbConnection db = GetConnection();
            int tempRC = db.QuerySingle<int>(
                "SELECT AltRC from EmployeeDetail Where EmployeeId = @empID and ProposalID = @propId",
                new {empID = emp, propId = proposalId});
            db.Execute("INSERT INTO EmployeeDetail VALUES(@propId, @empId, @rc)",
                new {propId = newId, empId = emp, @rc = tempRC});

            foreach (var softskill in db.Query<string>(
                "SELECT Softskill from EmployeeDetail_Softskill Where ProposalID = @propId and EmployeeID = @empId",
                new {propId = proposalId, empId = emp}))
            {
                db.Execute("INSERT INTO EmployeeDetail_Softskill VALUES(@newPropId, @empId, @softskillCopy)",
                    new {newPropId = newId, empId = emp, softskillCopy = softskill});
            }

            foreach (var field in db.Query<string>(
                "SELECT Field from EmployeeDetail_Field Where ProposalID = @propId and EmployeeID = @empId",
                new {propId = proposalId, empId = emp}))
            {
                db.Execute("INSERT INTO EmployeeDetail_Field VALUES(@newPropId, @empId, @Copy)",
                    new {newPropId = newId, empId = emp, Copy = field});
            }

            foreach (var role in db.Query<string>(
                "SELECT Role from EmployeeDetail_Role Where ProposalID = @propId and EmployeeID = @empId",
                new {propId = proposalId, empId = emp}))
            {
                db.Execute("INSERT INTO EmployeeDetail_Role VALUES(@newPropId, @empId, @Copy)",
                    new {newPropId = newId, empId = emp, Copy = role});
            }

            foreach (var hardskill in db.Query<string>(
                "SELECT Hardskill from EmployeeDetail_Hardskill Where ProposalID = @propId and EmployeeID = @empId",
                new {propId = proposalId, empId = emp}))
            {
                db.Execute("INSERT INTO EmployeeDetail_Hardskill VALUES(@newPropId, @empId, @Copy)",
                    new {newPropId = newId, empId = emp, Copy = hardskill});
            }

            foreach (var language in db.Query<string>(
                "SELECT Language from EmployeeDetail_Language Where ProposalID = @propId and EmployeeID = @empId",
                new {propId = proposalId, empId = emp}))
            {
                db.Execute("INSERT INTO EmployeeDetail_Language VALUES(@newPropId, @empId, @Copy)",
                    new {newPropId = newId, empId = emp, Copy = language});
            }

            return true;
        }

        public EmployeeDetail GetEmployeeDetail(int employeeId, int proposalId)
        {
            EmployeeDetail temp = new EmployeeDetail();
            using DbConnection db = GetConnection();
            db.Open();
            temp.Fields =
                new List<string>(db.Query<string>(
                    "SELECT Field from EmployeeDetail_Field WHERE EmployeeId = @empId and ProposalId = @propId",
                    new {empId = employeeId, propId = proposalId}));
            temp.Softskills = new List<string>(db.Query<string>(
                "SELECT Softskill from EmployeeDetail_Softskill WHERE EmployeeId = @empId and ProposalId = @propId",
                new {empId = employeeId, propId = proposalId}));
            temp.Languages = new List<Tuple<string, LanguageLevel>>();
            foreach (var language in db.Query<string>(
                "SELECT Language from EmployeeDetail_Language  WHERE EmployeeId = @empId and ProposalId = @propId",
                new {empId = employeeId, propId = proposalId}))
            {
                temp.Languages.Add(new Tuple<string, LanguageLevel>(language,
                    db.QuerySingle<LanguageLevel>(
                        "SELECT Level from Employee_Language WHERE EmployeeId = @emp and Language = @lang",
                        new {emp = employeeId, lang = language})));
            }

            temp.Rc = db.QuerySingle<int>(
                "SELECT AltRC from EmployeeDetail WHERE EmployeeId = @empId and ProposalId = @propId",
                new {empId = employeeId, propId = proposalId});
            temp.Hardskills = new List<Hardskill>();
            foreach (var hardskill in db.Query<string>(
                "SELECT Hardskill from EmployeeDetail_Hardskill WHERE EmployeeId = @empId and ProposalId = @propId",
                new {empId = employeeId, propId = proposalId}))
            {
                temp.Hardskills.Add(new Hardskill() {Description = hardskill});
            }

            return temp;
        }

        public bool DeleteEmployeeDetail(int employeeId, int proposalId)
        {
            using DbConnection db = GetConnection();
            db.Open();
            db.Execute("DELETE FROM EmployeeDetail_Softskill WHERE EmployeeId = @empId and ProposalID = @propId",
                new {empId = employeeId, propId = proposalId});
            db.Execute("DELETE FROM EmployeeDetail_Hardskill WHERE EmployeeId = @empId and ProposalID = @propId",
                new {empId = employeeId, propId = proposalId});
            db.Execute("DELETE FROM EmployeeDetail_Field WHERE EmployeeId = @empId and ProposalID = @propId",
                new {empId = employeeId, propId = proposalId});
            db.Execute("DELETE FROM EmployeeDetail_Language WHERE EmployeeId = @empId and ProposalID = @propId",
                new {empId = employeeId, propId = proposalId});
            db.Execute("DELETE FROM EmployeeDetail_Role WHERE EmployeeId = @empId and ProposalID = @propId",
                new {empId = employeeId, propId = proposalId});
            return true;
        }

        public List<EmployeeDetail> GetAllEmployeeDetail()
        {
            return EmployeeDetails;
        }

        public void DeleteRc(EmployeeDetail input)
        {
            input.Rc = 0;
        }

        public void DeleteField(EmployeeDetail input, string field)
        {
            input.Fields.Remove(field);
        }

        public void DeleteHard(EmployeeDetail input, Hardskill hardskill)
        {
            input.Hardskills.Remove(hardskill);
        }

        public void DeleteSoft(EmployeeDetail input, string softskill)
        {
            input.Softskills.Remove(softskill);
        }

        public void DeleteLang(EmployeeDetail input, Tuple<string, LanguageLevel> lang)
        {
            input.Languages.Remove(lang);
        }

        public void DeleteRole(EmployeeDetail input, string role)
        {
            input.Roles.Remove(role);
        }

        public void AddRc(EmployeeDetail input, int rc)
        {
            input.Rc = rc;
        }

        public void AddField(EmployeeDetail input, string field)
        {
            input.Fields.Add(field);
        }

        public void AddHard(EmployeeDetail input, Hardskill hardskill)
        {
            input.Hardskills.Add(hardskill);
        }

        public void AddSoft(EmployeeDetail input, string softskill)
        {
            input.Softskills.Add(softskill);
        }

        public void AddLang(EmployeeDetail input, Tuple<string, LanguageLevel> lang)
        {
            input.Languages.Add(lang);
        }

        public void AddRole(EmployeeDetail input, string role)
        {
            input.Roles.Add(role);
        }
    }
}