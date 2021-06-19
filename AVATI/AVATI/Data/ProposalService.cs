using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AVATI.Data
{
    public class ProposalService : IProposalService
    {
        private readonly IConfiguration _configuration;
        public List<Proposal> Proposals { get; set; }

        public DbConnection GetConnection()
        {
            return new SqlConnection
                (_configuration.GetConnectionString("AVATI-Database"));
        }

        
        public ProposalService(IConfiguration configuration)
        {
            _configuration = configuration;
            Proposals = new List<Proposal>()
            {

                new Proposal()
                {
                    ProposalID = 1,
                    ProposalTitle = "Machine Learning with HTML",
                    Softskills = new List<string>() {"Office", "Risk Management Skills"},
                    Fields = new List<string>() {"Marketing", "Sales", "It-Support"},
                    Hardskills = new List<Hardskill>()
                        {new Hardskill() {Description = "C++"}, new Hardskill() {Description = "Python"}},
                    AdditionalInfo = "Das Projekt versucht einer HTML - Seite Tetris beizubringen",

                },
                new Proposal()
                {
                    ProposalID = 2,
                    ProposalTitle = "Webanwendung mit C++ und Javascript",
                    Softskills = new List<string>() {"Bootstrap 5 Modal Dialogs", "Charisma"},
                    Fields = new List<string>() {"Design", "Wetter", "It-Support"},
                    Hardskills = new List<Hardskill>()
                        {new Hardskill() {Description = "C++"}, new Hardskill() {Description = "JavaScript"}},
                    AdditionalInfo = "Die beiden Programmiersprachen sollen für diese Anwendung kombiniert werden",
                    Employees = new List<Employee>()

                },
                new Proposal()
                {
                    ProposalID = 3,
                    ProposalTitle = "Facial Recognition with Smartphones",
                    Softskills = new List<string>() {"UML - Klassendiagramme", "Risk Management Skills"},
                    Fields = new List<string>() {"Marketing", "Sales", "AI - Development"},
                    Hardskills = new List<Hardskill>()
                        {new Hardskill() {Description = "C#"}, new Hardskill() {Description = "Python"}},
                    AdditionalInfo = "Facial Regognition, die unabhängig vom Smartphone funktioniert",
                    Employees = new List<Employee>()

                },
                new Proposal()
                {
                    ProposalID = 4,
                    ProposalTitle = "Machine Learning with HTML v2",
                    Softskills = new List<string>() {"Office", "Risk Management Skills"},
                    Fields = new List<string>() {"Marketing", "Sales", "It-Support"},
                    Hardskills = new List<Hardskill>()
                        {new Hardskill() {Description = "C++"}, new Hardskill() {Description = "Python"}},
                    AdditionalInfo = "Das Projekt versucht einer HTML - Seite Tetris beizubringen",
                    Employees = new List<Employee>()

                },
                new Proposal()
                {
                    ProposalID = 5,
                    ProposalTitle = "Facial Recognition with Security Kameras",
                    Softskills = new List<string>() {"Office", "Risk Management Skills"},
                    Fields = new List<string>() {"Marketing", "Sales", "It-Support"},
                    Hardskills = new List<Hardskill>()
                        {new Hardskill() {Description = "C++"}, new Hardskill() {Description = "Python"}},
                    AdditionalInfo = "Das System soll Einbrecher identifizieren können",
                    Employees = new List<Employee>()

                },
                new Proposal()
                {
                    ProposalID = 6,
                    ProposalTitle = "Web 4.0",
                    Softskills = new List<string>()
                        {"Libre Office", "Risk Management Skills", "Grundlegende Mathematik"},
                    Fields = new List<string>() {"Marketing", "Sales", "It-Support"},
                    Hardskills = new List<Hardskill>()
                        {new Hardskill() {Description = "C++"}, new Hardskill() {Description = "Python"}},
                    AdditionalInfo = "<<Empty>>",
                    Employees = new List<Employee>()

                },
                new Proposal()
                {
                    ProposalID = 7,
                    ProposalTitle = "Machine Learning with HTML v3",
                    Softskills = new List<string>() {"Office", "Risk Management Skills"},
                    Fields = new List<string>() {"Marketing", "Sales", "It-Support"},
                    Hardskills = new List<Hardskill>()
                        {new Hardskill() {Description = "C++"}, new Hardskill() {Description = "Python"}},
                    AdditionalInfo = "Die finale Version des HTML - Projekts",
                    Employees = new List<Employee>()

                },
            };
        }

        public List<string> GetSoftskills(int proposalId)
        {
            return GetProposal(proposalId).Softskills;
        }

        public List<Hardskill> GetHardskills(int proposalId)
        {
            return GetProposal(proposalId).Hardskills;;
        }

        public List<string> GetFields(int proposalId)
        {
            return GetProposal(proposalId).Fields;
        }

        

        public bool UpdateProposal(int id, Proposal proposal)
        {

            using DbConnection db = GetConnection();
            db.Open();
            var result = db.Query<Proposal>("SELECT * FROM Proposal WHERE ProposalID = @propId",
                new {propId = id});
            if (result.FirstOrDefault() == null)
            {
                db.Execute("INSERT INTO Proposal VALUES(@proposalTitle, @Info)",
                    new {proposalTitle = proposal.ProposalTitle ?? "LEER", Info = proposal.AdditionalInfo ?? "[Keine Zusatzinformationen]"});
            }
            else
            {
                db.Execute("update Proposal set ProposalTitle = @propTitle, AdditionalInfo = @addInfo where ProposalId = @propId", new
                    {propTitle = proposal.ProposalTitle ?? "Leer", addInfo = proposal.AdditionalInfo ?? "[Keine Zusatzinformationen", propId = id});
            }
            
            
            return false;
        }

        public bool DeleteProposal(int proposalId)
        {
            using DbConnection db = GetConnection();
            db.Open();
            if (db.Query<Proposal>("SELECT * FROM PROPOSAL WHERE ProposalId = @propID", new {propId = proposalId})
                .FirstOrDefault() == null)
            {
                Console.WriteLine("fehler beim löschen des Probosals");
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
            db.Open();
            if ((temp = db.Query<Proposal>("SELECT * FROM Proposal WHERE ProposalId = @propId", new {propId = proposalId})
                .FirstOrDefault()) == null)
            {
                return 0;
            }
            else
            {
                db.Execute("INSERT INTO Proposal VALUES(@title, @addInfo)",
                    new {title = temp.ProposalTitle, addInfo = temp.AdditionalInfo});
                return db.Query<int>("SELECT max(ProposalID) from Proposal").First();
            }
            
        }

        public bool GenerateDocument(int proposalId)
        {
            throw new System.NotImplementedException();
        }

        public Proposal GetProposal(int proposalId)
        {
            Proposal temp;
            using DbConnection db = GetConnection();
            db.Open();
            if ((temp = db.Query<Proposal>("SELECT * FROM Proposal Where ProposalId = @propId", new {propId = proposalId})
                .FirstOrDefault()) == null)
            {
                Console.WriteLine("FEEEEEHLER DAS PROPOSAL JIBT ES NICHT");
                return null;
            }
            else
            {
                return temp;
            }
        }

        public List<Proposal> GetAllProposals()
        {
            return Proposals;
        }
    }
}