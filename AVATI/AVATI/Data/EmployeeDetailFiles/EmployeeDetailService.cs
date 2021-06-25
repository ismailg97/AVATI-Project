using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
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
            using DbConnection db = GetConnection();
            db.Open();
            foreach (var soft in employeeDetail.Softskills)
            {
                if (db.Query<string>(
                    "SELECT Softskill from EmployeeDetail_Softskill WHERE ProposalId = @pro and EmployeeId = @emp and Softskill = @softskill",
                    new
                    {
                        pro = proposalId, emp = employeeId, softskill = soft
                    }).FirstOrDefault() == null)
                {
                    db.Execute("INSERT INTO EmployeeDetail_Softskill VALUES(@proId, @empId, @softskill)",
                        new {proId = proposalId, empId = employeeId, softskill = soft});
                }
            }

            foreach (var hard in employeeDetail.Hardskills)
            {
                if (db.Query<string>(
                    "SELECT Hardskill from EmployeeDetail_Hardskill WHERE ProposalId = @pro and EmployeeId = @emp and Hardskill = @hardskill",
                    new
                    {
                        pro = proposalId, emp = employeeId, hardskill = hard
                    }).FirstOrDefault() == null)
                {
                    db.Execute("INSERT INTO EmployeeDetail_Hardskill VALUES(@proId, @empId, @hardskill)",
                        new {proId = proposalId, empId = employeeId, hardskill = hard});
                }
            }

            foreach (var lang in employeeDetail.Languages)
            {
                if (db.Query<string>(
                    "SELECT Language from EmployeeDetail_Language WHERE ProposalId = @pro and EmployeeId = @emp and Language = @language",
                    new
                    {
                        pro = proposalId, emp = employeeId, language = lang
                    }).FirstOrDefault() == null)
                {
                    db.Execute("INSERT INTO EmployeeDetail_Language VALUES(@proId, @empId, @language)",
                        new {proId = proposalId, empId = employeeId, language = lang});
                }
            }

            foreach (var field in employeeDetail.Fields)
            {
                if (db.Query<string>(
                    "SELECT Field from EmployeeDetail_Field WHERE ProposalId = @pro and EmployeeId = @emp and Field = @fieldd",
                    new
                    {
                        pro = proposalId, emp = employeeId, fieldd = field
                    }).FirstOrDefault() == null)
                {
                    db.Execute("INSERT INTO EmployeeDetail_Field VALUES(@proId, @empId, @fieldd)",
                        new {proId = proposalId, empId = employeeId, fieldd = field});
                }
            }

            foreach (var role in employeeDetail.Roles)
            {
                if (db.Query<string>(
                    "SELECT Role from EmployeeDetail_Role WHERE ProposalId = @pro and EmployeeId = @emp and Role = @Role",
                    new
                    {
                        pro = proposalId, emp = employeeId, Role = role
                    }).FirstOrDefault() == null)
                {
                    db.Execute("INSERT INTO EmployeeDetail_Role VALUES(@proId, @empId, @Role)",
                        new {proId = proposalId, empId = employeeId, Role = role});
                }
            }

            return true;
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
            using DbConnection db = GetConnection();
            db.Open();
            List<EmployeeDetail> employeeList =
                new List<EmployeeDetail>(db.Query<EmployeeDetail>("SELECT * FROM EmployeeDetail"));
            foreach (var temp in employeeList)
            {
                temp.Fields =
                    new List<string>(db.Query<string>(
                        "SELECT Field from EmployeeDetail_Field WHERE EmployeeId = @empId and ProposalId = @propId",
                        new {empId = temp.EmployeeId, propId = temp.ProposalId}));
                temp.Softskills = new List<string>(db.Query<string>(
                    "SELECT Softskill from EmployeeDetail_Softskill WHERE EmployeeId = @empId and ProposalId = @propId",
                    new {empId = temp.EmployeeId, propId = temp.ProposalId}));
                temp.Languages = new List<Tuple<string, LanguageLevel>>();
                foreach (var language in db.Query<string>(
                    "SELECT Language from EmployeeDetail_Language  WHERE EmployeeId = @empId and ProposalId = @propId",
                    new {empId = temp.EmployeeId, propId = temp.ProposalId}))
                {
                    temp.Languages.Add(new Tuple<string, LanguageLevel>(language,
                        db.QuerySingle<LanguageLevel>(
                            "SELECT Level from Employee_Language WHERE EmployeeId = @emp and Language = @lang",
                            new {emp = temp.EmployeeId, lang = language})));
                }

                temp.Rc = db.QuerySingle<int>(
                    "SELECT AltRC from EmployeeDetail WHERE EmployeeId = @empId and ProposalId = @propId",
                    new {empId = temp.EmployeeId, propId = temp.ProposalId});
                temp.Hardskills = new List<Hardskill>();
                foreach (var hardskill in db.Query<string>(
                    "SELECT Hardskill from EmployeeDetail_Hardskill WHERE EmployeeId = @empId and ProposalId = @propId",
                    new {empId = temp.EmployeeId, propId = temp.ProposalId}))
                {
                    temp.Hardskills.Add(new Hardskill() {Description = hardskill});
                }
            }

            return employeeList;
        }
    }
}