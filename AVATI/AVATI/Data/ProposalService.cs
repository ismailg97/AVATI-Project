using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AVATI.Data.EmployeeDetailFiles;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AVATI.Data
{
    public class ProposalService : IProposalService
    {
        private readonly IConfiguration _configuration;

        private string _connectionString;
        public List<Proposal> Proposals { get; set; }
        private readonly EmployeeDetailService _employeeDetailService;
        public DbConnection GetConnection()
        {
            if (_connectionString != null)
            {
                return new SqlConnection
                    (_connectionString);
            }
            return new SqlConnection
                (_configuration.GetConnectionString("AVATI-Database"));
        }

        public ProposalService(string connectionString)
        {
            _connectionString = connectionString;
        }
        public ProposalService(IConfiguration configuration)
        {
            _configuration = configuration;
            _employeeDetailService = new EmployeeDetailService(configuration);
        }

        public List<string> GetSoftskills(int proposalId)
        {
            return GetProposal(proposalId).Softskills;
        }

        public List<Hardskill> GetHardskills(int proposalId)
        {
            return GetProposal(proposalId).Hardskills;
        }

        public List<string> GetFields(int proposalId)
        {
            return GetProposal(proposalId).Fields;
        }

        public bool GenerateDocument(int proposalId)
        {
            //TODO: in service-klasse einbinden
            return false;
        }

        public bool RemoveEmployee(int propId, int empId)
        {
            using DbConnection db = GetConnection();
            db.Execute("Delete FROM EmployeeDetail_Softskill WHERE ProposalId = @prop and EmployeeId = @emp",
                new {prop = propId, emp = empId});
            db.Execute("Delete FROM EmployeeDetail_Hardskill WHERE ProposalId = @prop and EmployeeId = @emp",
                new {prop = propId, emp = empId});
            db.Execute("Delete FROM EmployeeDetail_Field WHERE ProposalId = @prop and EmployeeId = @emp",
                new {prop = propId, emp = empId});
            db.Execute("Delete FROM EmployeeDetail_Language WHERE ProposalId = @prop and EmployeeId = @emp",
                new {prop = propId, emp = empId});
            db.Execute("Delete FROM EmployeeDetail_Role WHERE ProposalId = @prop and EmployeeId = @emp",
                new {prop = propId, emp = empId});
            db.Execute("Delete FROM EmployeeDetail WHERE ProposalId = @prop and EmployeeId = @emp",
                new {prop = propId, emp = empId});
            return true;
        }

        public bool AddEmployee(int propId, int empl, int rc)
        {
            using DbConnection db = GetConnection();
            Employee tempEmp =
                db.QuerySingle<Employee>("SELECT * FROM Employee WHERE EmployeeID = @emp", new {emp = empl});
            if (tempEmp == null)
            {
                return false;
            }

            db.Execute("INSERT INTO EmployeeDetail VALUES(@prop, @emp, @oldRc, 0)",
                new {prop = propId, emp = empl, oldRc = rc});


            return true;
        }

        public bool UpdateProposal(int id, Proposal proposal)
        {
            using DbConnection db = GetConnection();
            int idToUSe = id;
            if (proposal.ProposalTitle.Length > 70 || proposal.ProposalTitle is null or "")
            {
                return false;
            }
            var result = db.Query<Proposal>("SELECT * FROM Proposal WHERE ProposalID = @propId",
                new {propId = id});
            if (result.FirstOrDefault() == null)
            {
                db.Execute("INSERT INTO Proposal VALUES(@proposalTitle, @Info, @beg, @end)",
                    new
                    {
                        proposalTitle = proposal.ProposalTitle,
                        Info = proposal.AdditionalInfo ?? "[Keine Zusatzinformationen]",
                        beg = proposal.Start.ToString("d", DateTimeFormatInfo.InvariantInfo),
                        end = proposal.End.ToString("d", DateTimeFormatInfo.InvariantInfo)
                    });
                idToUSe = db.QueryFirst<int>("SELECT max(ProposalID) from Proposal");
            }
            else
            {
                db.Execute(
                    "update Proposal set ProposalTitle = @propTitle, AdditionalInfo = @addInfo, ProposalEnd = @end, ProposalBegin = @start where ProposalId = @propId",
                    new
                    {
                        propTitle = proposal.ProposalTitle ?? "Leer",
                        addInfo = proposal.AdditionalInfo ?? "[Keine Zusatzinformationen", propId = idToUSe,
                        start = proposal.Start, end = proposal.End
                    });
            }

            foreach (var softskill in proposal.Softskills)
            {
                if (db.Query<string>(
                    "SELECT Softskill from Proposal_Softskill WHERE ProposalId = @propId and Softskill = @soft",
                    new {propId = idToUSe, soft = softskill}).FirstOrDefault() == null)
                {
                    db.Query<string>("INSERT INTO Proposal_Softskill VALUES(@propId, @softskillToAdd)",
                        new {propId = idToUSe, softskillToAdd = softskill});
                }
            }

            foreach (var field in proposal.Fields)
            {
                if (db.Query<string>(
                    "SELECT Field from Proposal_Fields WHERE ProposalId = @propId and Field = @fieldSe",
                    new {propId = idToUSe, fieldSe = field}).FirstOrDefault() == null)
                {
                    db.Query<string>("INSERT INTO Proposal_Fields VALUES(@propId, @fieldToAdd)",
                        new {propId = idToUSe, fieldToAdd = field});
                }
            }

            foreach (var hardskill in proposal.Hardskills)
            {
                if (db.Query<string>(
                    "SELECT Hardskill from Proposal_Hardskill WHERE ProposalId = @propId and Hardskill = @desc",
                    new {propId = idToUSe, desc = hardskill.Description}).FirstOrDefault() == null)
                {
                    db.Query<string>("INSERT INTO Proposal_Hardskill VALUES(@propId, @hardskillToAdd)",
                        new {propId = idToUSe, hardskillToAdd = hardskill.Description});
                }
            }

            return true;
        }

        public bool DeleteProposal(int proposalId)
        {
            using DbConnection db = GetConnection();
            foreach (var empId in db.Query<int>("SELECT EmployeeID from EmployeeDetail WHERE ProposalID = @prop",
                new {prop = proposalId}))
            {
                _employeeDetailService.DeleteEmployeeDetail(empId, proposalId);
            }

            if (db.Query<Proposal>("SELECT * FROM PROPOSAL WHERE ProposalId = @propID", new {propId = proposalId})
                .FirstOrDefault() == null)
            {
                return false;
            }
            else
            {
                db.Execute("DELETE FROM Proposal WHERE ProposalId = @propId", new {propId = proposalId});
                return true;
            }
        }

        public int CopyProposal(int proposalId)
        {
            using DbConnection db = GetConnection();
            Proposal temp;
            if ((temp = db.Query<Proposal>("SELECT * FROM Proposal WHERE ProposalId = @propId",
                    new {propId = proposalId})
                .FirstOrDefault()) == null)
            {
                return 0;
            }

            if (temp.ProposalTitle.Contains("[KOPIE]"))
            {
                temp.ProposalTitle = temp.ProposalTitle.Remove(temp.ProposalTitle.Length -7);
            }
            temp.Start = db.QuerySingle<DateTime>("SELECT ProposalBegin from Proposal WHERE ProposalId = @proId",
                new {proId = proposalId});
            temp.End = db.QuerySingle<DateTime>("SELECT ProposalEnd from Proposal WHERE ProposalId = @proId",
                new {proId = proposalId});
            db.Execute("INSERT INTO Proposal VALUES(@proposalTitle, @Info, @beg, @end)",
                new
                {
                    proposalTitle = temp.ProposalTitle + " [KOPIE]",
                    Info = temp.AdditionalInfo ?? "[Keine Zusatzinformationen]",
                    beg = temp.Start.ToString("d", DateTimeFormatInfo.InvariantInfo),
                    end = temp.End.ToString("d", DateTimeFormatInfo.InvariantInfo)
                });
            int newId = db.Query<int>("SELECT max(ProposalID) from Proposal").First();
            foreach (var hardskill in db.Query<string>(
                "SELECT Hardskill FROM Proposal_Hardskill WHERE ProposalID = @propId", new {propId = proposalId}))
            {
                db.Execute("INSERT INTO Proposal_Hardskill VALUES(@id, @desc)", new {id = newId, desc = hardskill});
            }

            foreach (var field in db.Query<string>("SELECT Field FROM Proposal_Fields WHERE ProposalID = @propId",
                new {propId = proposalId}))
            {
                db.Execute("INSERT INTO Proposal_Fields VALUES(@id, @desc)", new {id = newId, desc = field});
            }

            foreach (var softskill in db.Query<string>(
                "SELECT Softskill FROM Proposal_Softskill WHERE ProposalID = @propId", new {propId = proposalId}))
            {
                db.Execute("INSERT INTO Proposal_Softskill VALUES(@id, @desc)", new {id = newId, desc = softskill});
            }

            foreach (var emp in db.Query<int>("SELECT EmployeeId from EmployeeDetail WHERE ProposalID = @propId",
                new {propId = proposalId}))
            {
                _employeeDetailService.CopyDetail(proposalId, newId, emp);
            }

            return newId;
        }


        public Proposal GetProposal(int proposalId)
        {
            Proposal temp;
            using DbConnection db = GetConnection();
            if ((temp = db.Query<Proposal>("SELECT * FROM Proposal Where ProposalId = @propId",
                    new {propId = proposalId})
                .FirstOrDefault()) == null)
            {
                return null;
            }
            else
            {
                foreach (string hardskill in db.Query<string>(
                    "SELECT Hardskill FROM Proposal_Hardskill where ProposalId = @propId",
                    new {propId = proposalId}).ToList())
                {
                    temp.Hardskills.Add(new Hardskill() {Description = hardskill});
                }

                foreach (var employeeId in db.Query<int>(
                    "SELECT EmployeeID from EmployeeDetail WHERE ProposalId = @propId", new {propId = proposalId}))
                {
                    temp.Employees.Add(db.QuerySingle<Employee>("SELECT * FROM Employee WHERE EmployeeID = @empId",
                        new {empId = employeeId}));
                    temp.AltRc.Add(employeeId,
                        db.QuerySingle<int>(
                            "SELECT AltRC FROM EmployeeDetail WHERE EmployeeID = @empId and ProposalID = @prop",
                            new {empId = employeeId, prop = proposalId}));
                }

                temp.Softskills = new List<string>(db
                    .Query<string>("SELECT SOFTSKILL FROM Proposal_Softskill where ProposalId = @propId",
                        new {propId = proposalId}).ToList());
                temp.Fields = new List<string>(db.Query<string>(
                    "SELECT Field from Proposal_Fields where ProposalId = @propId", new {propId = proposalId}));
                temp.Start = db.QuerySingle<DateTime>("SELECT ProposalBegin from Proposal WHERE ProposalId = @proId",
                    new {proId = proposalId});
                temp.End = db.QuerySingle<DateTime>("SELECT ProposalEnd from Proposal WHERE ProposalId = @proId",
                    new {proId = proposalId});
                return temp;
            }
        }

        public async Task<List<Proposal>> GetAllProposals()
        {
            await using DbConnection db = GetConnection();
            List<Proposal> proposals = new List<Proposal>(await db.QueryAsync<Proposal>("SELECT * FROM Proposal"));
            foreach (var proposal in proposals)
            {
                foreach (string hardskill in await db.QueryAsync<string>(
                    "SELECT Hardskill FROM Proposal_Hardskill where ProposalId = @propId",
                    new {propId = proposal.ProposalID}))
                {
                    proposal.Hardskills.Add(new Hardskill() {Description = hardskill});
                }

                foreach (var employeeId in db.Query<int>(
                    "SELECT EmployeeID from EmployeeDetail WHERE ProposalId = @propId",
                    new {propId = proposal.ProposalID}))
                {
                    proposal.Employees.Add(db.QuerySingle<Employee>("SELECT * FROM Employee WHERE EmployeeID = @empId",
                        new {empId = employeeId}));
                    proposal.AltRc.Add(employeeId,
                        db.QuerySingle<int>(
                            "SELECT AltRC from EmployeeDetail WHERE EmployeeID = @emp and ProposalID = @prop",
                            new {emp = employeeId, prop = proposal.ProposalID}));
                }

                proposal.Softskills = new List<string>(db
                    .Query<string>("SELECT SOFTSKILL FROM Proposal_Softskill where ProposalId = @propId",
                        new {propId = proposal.ProposalID}).ToList());
                proposal.Fields = new List<string>(db.Query<string>(
                    "SELECT Field from Proposal_Fields where ProposalId = @propId",
                    new {propId = proposal.ProposalID}));
                proposal.Start = db.QuerySingle<DateTime>(
                    "SELECT ProposalBegin from Proposal WHERE ProposalId = @proId",
                    new {proId = proposal.ProposalID});
                proposal.End = db.QuerySingle<DateTime>("SELECT ProposalEnd from Proposal WHERE ProposalId = @proId",
                    new {proId = proposal.ProposalID});
            }

            proposals.Reverse();
            return proposals;
        }

        public bool UpdateAltRc(int proposalId, int empId, int newRc)
        {
            using DbConnection db = GetConnection();
            if (db.Execute(
                "Update EmployeeDetail SET AltRC = @newRCLevel Where ProposalID = @propId and EmployeeId = @employeeId",
                new {newRCLevel = newRc, propId = proposalId, employeeId = empId}) != 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}