using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AVATI.Data.EmployeeDetailFiles
{
    public class EmployeeDetailService : IEmployeeDetailService
    {
        private string _connection;
        
        private readonly IConfiguration _configuration;

        public DbConnection GetConnection()
        {
            if (_connection != null)
            {
                return new SqlConnection(_connection);
            }
            return new SqlConnection
                (_configuration.GetConnectionString("AVATI-Database"));
        }

        public EmployeeDetailService(IConfiguration configuration)
        {
            _configuration = configuration;
            EmployeeDetails = new List<EmployeeDetail>();
        }

        public EmployeeDetailService(string connect)
        {
            _connection = connect;
        }
        
        public List<EmployeeDetail> EmployeeDetails;
        

        public async Task<bool> UpdateEmployeeDetail(int employeeId, int proposalId, EmployeeDetail employeeDetail)
        {
            await using DbConnection db = GetConnection();
            var rows = await db.ExecuteAsync("Update EmployeeDetail SET Discount = @disc WHERE ProposalId = @pro and EmployeeId = @emp",
                new
                {
                    pro = proposalId, emp = employeeId, disc = employeeDetail.Discount
                });
            
            var rows2 = await db.ExecuteAsync("Update EmployeeDetail SET Hours = @hour WHERE ProposalId = @pro and EmployeeId = @emp",
                new
                {
                    pro = proposalId, emp = employeeId, hour = employeeDetail.Hours
                });
            Console.WriteLine("Rows affected: " + rows + " Discount: " + employeeDetail.Discount);
            foreach (var soft in employeeDetail.Softskills)
            {
                if (db.Query<string>(
                    "SELECT Softskill from EmployeeDetail_Softskill WHERE ProposalId = @pro and EmployeeId = @emp and Softskill = @softskill",
                    new
                    {
                        pro = proposalId, emp = employeeId, softskill = soft
                    }).FirstOrDefault() == null)
                {
                    await db.ExecuteAsync("INSERT INTO EmployeeDetail_Softskill VALUES(@proId, @empId, @softskill)",
                        new {proId = proposalId, empId = employeeId, softskill = soft});
                }
            }

            foreach (var softskill in db.Query<string>(
                "SELECT Softskill from EmployeeDetail_Softskill WHERE ProposalId = @pro and EmployeeId = @emp",
                new
                {pro = proposalId, emp = employeeId, 
                }))
            {
                if (!employeeDetail.Softskills.Contains(softskill))
                {
                    await db.ExecuteAsync("DELETE FROM EmployeeDetail_Softskill WHERE ProposalID = @pro and EmployeeID = @emp AND Softskill = @soft", new
                    {
                        pro = proposalId, emp = employeeId, soft = softskill
                    });
                }
            }
            foreach (var hard in employeeDetail.Hardskills)
            {
                if (db.Query<string>(
                    "SELECT Hardskill from EmployeeDetail_Hardskill WHERE ProposalId = @pro and EmployeeId = @emp and Hardskill = @hardskill",
                    new
                    {
                        pro = proposalId, emp = employeeId, hardskill = hard.Description
                    }).FirstOrDefault() == null)
                {
                    db.Execute("INSERT INTO EmployeeDetail_Hardskill VALUES(@proId, @empId, @hardskill)",
                        new {proId = proposalId, empId = employeeId, hardskill = hard.Description});
                }
            }

            foreach (var hardskill in db.Query<string>(
                "SELECT Hardskill from EmployeeDetail_Hardskill WHERE ProposalId = @pro and EmployeeId = @emp",
                new
                {pro = proposalId, emp = employeeId, 
                }))
            {
                if (employeeDetail.Hardskills.Find(e => e.Description.Equals(hardskill)) == null)
                {
                    await db.ExecuteAsync("DELETE FROM EmployeeDetail_Hardskill WHERE ProposalID = @pro and EmployeeID = @emp and Hardskill = @hard", new
                    {
                        pro = proposalId, emp = employeeId, hard = hardskill
                    });
                }
            }
            
            foreach (var lang in employeeDetail.Languages)
            {
                if (db.Query<string>(
                    "SELECT Language from EmployeeDetail_Language WHERE ProposalId = @pro and EmployeeId = @emp and Language = @language",
                    new
                    {
                        pro = proposalId, emp = employeeId, language = lang.Item1
                    }).FirstOrDefault() == null)
                {
                    await db.ExecuteAsync("INSERT INTO EmployeeDetail_Language VALUES(@proId, @empId, @language)",
                        new {proId = proposalId, empId = employeeId, language = lang.Item1});
                }
            }

            foreach (var language in db.Query<string>(
                "SELECT Language from EmployeeDetail_Language WHERE ProposalId = @pro and EmployeeId = @emp",
                new
                {pro = proposalId, emp = employeeId, 
                }))
            {
                if (employeeDetail.Languages.Find(e => e.Item1.Equals(language)) == null)
                {
                    SqlMapper.Execute(db, "DELETE FROM EmployeeDetail_Language WHERE ProposalID = @pro and EmployeeID = @emp and Language = @lang", new
                    {
                        pro = proposalId, emp = employeeId, lang = language
                    });
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
                    await db.ExecuteAsync("INSERT INTO EmployeeDetail_Field VALUES(@proId, @empId, @fieldd)",
                        new {proId = proposalId, empId = employeeId, fieldd = field});
                }
            }
            
            foreach (var field in db.Query<string>(
                "SELECT Field from EmployeeDetail_Field WHERE ProposalId = @pro and EmployeeId = @emp",
                new
                {pro = proposalId, emp = employeeId, 
                }))
            {
                if (employeeDetail.Fields.Find(e => e.Equals(field)) == null)
                {
                    await db.ExecuteAsync("DELETE FROM EmployeeDetail_Field WHERE ProposalID = @pro and EmployeeID = @emp and  Field = @lang", new
                    {
                        pro = proposalId, emp = employeeId, lang = field
                    });
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
                    await db.ExecuteAsync("INSERT INTO EmployeeDetail_Role VALUES(@proId, @empId, @Role)",
                        new {proId = proposalId, empId = employeeId, Role = role});
                }
            }
            foreach (var role in db.Query<string>(
                "SELECT Role from EmployeeDetail_Role WHERE ProposalId = @pro and EmployeeId = @emp",
                new
                {pro = proposalId, emp = employeeId, 
                }))
            {
                if (employeeDetail.Roles.Find(e => e.Equals(role)) == null)
                {
                    await db.ExecuteAsync("DELETE FROM EmployeeDetail_Role WHERE ProposalID = @pro and EmployeeID = @emp and  Role = @lang", new
                    {
                        pro = proposalId, emp = employeeId, lang = role
                    });
                }
            }
            
            await db.ExecuteAsync(
                "UPDATE EmployeeDetail_ProjectActivity SET InDetail = 0 WHERE ProposalID = @propId AND ProjectActivityID in (SELECT ProjectActivityID FROM ProjectActivity_Project_Employee WHERE EmployeeID = @empId)",
                new{ propId = proposalId, empId = employeeId});

            foreach (var projectact in employeeDetail.ProjectActivities)
            {
                var exists = db.QuerySingle<int>("SELECT COUNT(*) FROM EmployeeDetail_ProjectActivity WHERE ProjectActivityID = @proactId AND ProposalID = @propId",
                    new{ proactId = projectact.ProjectActivityID, propId = proposalId });
                if (exists == 0)
                {
                    await db.ExecuteAsync("INSERT INTO EmployeeDetail_ProjectActivity VALUES (@proactId, @propId, 1)",
                        new {proactId = projectact.ProjectActivityID, propId = proposalId});
                }
                else if (exists == 1)
                {
                    await db.ExecuteAsync("UPDATE EmployeeDetail_ProjectActivity SET InDetail = 1 WHERE ProjectActivityID = @proactId AND ProposalID = @propId",
                        new {proactId = projectact.ProjectActivityID, propId = proposalId});
                }
                else
                    return false;
            }

            return true;
        }

        public bool CopyDetail(int proposalId, int newId, int emp)
        {
            using DbConnection db = GetConnection();
            int tempRc = db.QuerySingle<int>(
                "SELECT AltRC from EmployeeDetail Where EmployeeId = @empID and ProposalID = @propId",
                new {empID = emp, propId = proposalId});
            int discount = db.QuerySingle<int>("SELECT Discount from EmployeeDetail Where EmployeeId = @empID and ProposalID = @propId",
                new {empID = emp, propId = proposalId});
            int hours = db.QuerySingle<int>("SELECT Hours from EmployeeDetail Where EmployeeId = @empID and ProposalID = @propId",
                new {empID = emp, propId = proposalId});
            db.Execute("INSERT INTO EmployeeDetail VALUES(@propId, @empId, @rc, @dc, @hou)",
                new {propId = newId, empId = emp, rc = tempRc, dc = discount, hou = hours});
            
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

            foreach (var projectactId in db.Query<int>(
                "SELECT p.ProjectActivityID FROM ProjectActivity_Project_Employee p LEFT JOIN EmployeeDetail_ProjectActivity e ON p.ProjectActivityID = e.ProjectActivityID WHERE EmployeeID = @empId and ProposalID = @propId and InDetail = 1",
                new {propId = proposalId, empId = emp}))
            {
                db.Execute("INSERT INTO EmployeeDetail_ProjectActivity VALUES (@proactId, @newPropId, 1)", 
                    new{proactId = projectactId, newPropId = newId});
            }

            return true;
        }

        public async Task<EmployeeDetail> GetEmployeeDetail(int employeeId, int proposalId)
        {
            EmployeeDetail temp = new EmployeeDetail();
            await using DbConnection db = GetConnection();
            temp.Discount =
                    db.QuerySingle<int>(
                        "SELECT Discount from EmployeeDetail WHERE EmployeeId = @empId and ProposalID = @propId",
                        new {empId = employeeId, propId = proposalId});
            temp.Hours = db.QuerySingle<int>(
                "SELECT Hours from EmployeeDetail WHERE EmployeeId = @empId and ProposalID = @propId",
                new {empId = employeeId, propId = proposalId});
            temp.Roles = new List<string>(db.Query<string>(
                "SELECT Role from EmployeeDetail_Role WHERE EmployeeId = @empId and ProposalId = @propId",
                new {empId = employeeId, propId = proposalId}));
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
                var level = db.Query<string>(
                    "SELECT Employee_Language.Level from Employee_Language WHERE EmployeeId = @emp and Language = @lang",
                    new {emp = employeeId, lang = language}).ToList();
                string langaugeLevel = level.ToString();

                temp.Languages.Add(new Tuple<string, LanguageLevel>(language, GetLanguageLevel(level.First())));
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

            temp.ProjectActivities = db.Query<ProjectActivity>(
                "SELECT * FROM ProjectActivity_Project_Employee p LEFT JOIN EmployeeDetail_ProjectActivity e ON p.ProjectActivityID = e.ProjectActivityID WHERE EmployeeID = @empId and ProposalID = @propId and InDetail = 1",
            new {empId = employeeId, propId = proposalId}).ToList();

            foreach (var projectact in temp.ProjectActivities)
            {
                Console.WriteLine("ProjectActivityID: " + projectact.ProjectActivityID + " Description: " 
                              + projectact.Description + " ProjectID: " + projectact.ProjectID + " EmployeeID: " + projectact.EmployeeID);
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
            db.Execute(
                "DELETE FROM EmployeeDetail_ProjectActivity WHERE ProposalID = @propId AND ProjectActivityID IN (SELECT ProjectActivityID FROM ProjectActivity_Project_Employee WHERE EmployeeID = @empId)",
                 new{ empId = employeeId, propId = proposalId });
            return true;
        }

        public List<EmployeeDetail> GetAllEmployeeDetail(int proposalId)
        {
            using DbConnection db = GetConnection();
            List<EmployeeDetail> employeeList =
                new List<EmployeeDetail>(db.Query<EmployeeDetail>("SELECT * FROM EmployeeDetail WHERE ProposalId = @propId", new {propId = proposalId}));
            foreach (var temp in employeeList)
            {
               temp.Discount =
                    db.QuerySingle<int>(
                        "SELECT Discount from EmployeeDetail WHERE EmployeeId = @empId and ProposalID = @propId",
                        new {empId = temp.EmployeeId, propId = temp.ProposalId}); 
               temp.Hours = db.QuerySingle<int>(
                   "SELECT Hours from EmployeeDetail WHERE EmployeeId = @empId and ProposalID = @propId",
                   new {empId = temp.EmployeeId, propId = temp.ProposalId}); 
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
                        GetLanguageLevel(db.QuerySingle<string>(
                            "SELECT Level from Employee_Language WHERE EmployeeId = @emp and Language = @lang",
                            new {emp = temp.EmployeeId, lang = language}))));
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
                temp.Roles =
                    new List<string>(db.Query<string>(
                        "SELECT Role from EmployeeDetail_Role WHERE EmployeeId = @empId and ProposalId = @propId",
                        new {empId = temp.EmployeeId, propId = temp.ProposalId}));
                
                temp.ProjectActivities = db.Query<ProjectActivity>(
                    "SELECT * FROM ProjectActivity_Project_Employee p LEFT JOIN EmployeeDetail_ProjectActivity e ON p.ProjectActivityID = e.ProjectActivityID WHERE EmployeeID = @empId and ProposalID = @propId and InDetail = 1",
                    new {empId = temp.EmployeeId, propId = temp.ProposalId}).ToList();
            }

            return employeeList;
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