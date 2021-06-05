using System.Collections.Generic;

namespace AVATI.Data
{
    public class ProposalService : IProposalService
    {
        public List<Proposal> Proposals { get; set; }

        public void CreateDummyProjects()
        {
            Proposals = new List<Proposal>()
            {
                new Proposal()
                {
                    ProposalId = 1,
                    ProposalTitle = "Machine Learning with HTML",
                    Softskills = new List<string>() {"Office", "Risk Management Skills"},
                    Fields = new List<string>() {"Marketing", "Sales", "It-Support"},
                    Hardskills = new List<Hardskill>() {new Hardskill() {Description = "C++"}, new Hardskill() {Description = "Python"}},
                    AdditionalInfo = "Das Projekt versucht einer HTML - Seite Tetris beizubringen",
                    EmployeeTemps = new List<EmployeeTemp>()
                    {
                        new EmployeeTemp() {Name = "Smith Smitherson", RC = 6},
                        new EmployeeTemp() {Name = "Harold Kumar", RC = 3},
                        new EmployeeTemp() {Name = "Jonathan Lebowski", RC = 1}
                    }
                },
                new Proposal()
                {
                    ProposalId = 2,
                    ProposalTitle = "Webanwendung mit C++ und Javascript",
                    Softskills = new List<string>() {"Bootstrap 5 Modal Dialogs", "Charisma"},
                    Fields = new List<string>() {"Design", "Wetter", "It-Support"},
                    Hardskills = new List<Hardskill>() {new Hardskill() {Description = "C++"}, new Hardskill() {Description = "JavaScript"}},
                    AdditionalInfo = "Die beiden Programmiersprachen sollen für diese Anwendung kombiniert werden",
                    EmployeeTemps = new List<EmployeeTemp>()
                    {
                        new EmployeeTemp() {Name = "Joe Ann", RC = 6},
                        new EmployeeTemp() {Name = "Smith Smitherson", RC = 6},
                        new EmployeeTemp() {Name = "Harold Kumar", RC = 3}
                    }
                },
                new Proposal()
                {
                    ProposalId = 3,
                    ProposalTitle = "Facial Recognition with Smartphones",
                    Softskills = new List<string>() {"UML - Klassendiagramme", "Risk Management Skills"},
                    Fields = new List<string>() {"Marketing", "Sales", "AI - Development"},
                    Hardskills = new List<Hardskill>() {new Hardskill() {Description = "C#"}, new Hardskill() {Description = "Python"}},
                    AdditionalInfo = "Facial Regognition, die unabhängig vom Smartphone funktioniert",
                    EmployeeTemps = new List<EmployeeTemp>()
                    {
                        new EmployeeTemp() {Name = "Joe Watson", RC = 6},
                        new EmployeeTemp() {Name = "Smith Smitherson", RC = 6},
                        new EmployeeTemp() {Name = "Joe Ann", RC = 6}
                    }
                },
                new Proposal()
                {
                    ProposalId = 4,
                    ProposalTitle = "Machine Learning with HTML v2",
                    Softskills = new List<string>() {"Office", "Risk Management Skills"},
                    Fields = new List<string>() {"Marketing", "Sales", "It-Support"},
                    Hardskills = new List<Hardskill>() {new Hardskill() {Description = "C++"}, new Hardskill() {Description = "Python"}},
                    AdditionalInfo = "Das Projekt versucht einer HTML - Seite Tetris beizubringen",
                    EmployeeTemps = new List<EmployeeTemp>()
                    {
                        new EmployeeTemp() {Name = "Joe Watson", RC = 6},
                        new EmployeeTemp() {Name = "Joe Ann", RC = 6}
                    }
                },
                new Proposal()
                {
                    ProposalId = 5,
                    ProposalTitle = "Facial Recognition with Security Kameras",
                    Softskills = new List<string>() {"Office", "Risk Management Skills"},
                    Fields = new List<string>() {"Marketing", "Sales", "It-Support"},
                    Hardskills = new List<Hardskill>() {new Hardskill() {Description = "C++"}, new Hardskill() {Description = "Python"}},
                    AdditionalInfo = "Das System soll Einbrecher identifizieren können",
                    EmployeeTemps = new List<EmployeeTemp>()
                    {
                        new EmployeeTemp() {Name = "Joe Watson", RC = 6},
                        new EmployeeTemp() {Name = "Joe Ann", RC = 6}
                    }
                },
                new Proposal()
                {
                    ProposalId = 6,
                    ProposalTitle = "Web 4.0",
                    Softskills = new List<string>() {"Libre Office", "Risk Management Skills", "Grundlegende Mathematik"},
                    Fields = new List<string>() {"Marketing", "Sales", "It-Support"},
                    Hardskills = new List<Hardskill>() {new Hardskill() {Description = "C++"}, new Hardskill() {Description = "Python"}},
                    AdditionalInfo = "<<Empty>>",
                    EmployeeTemps = new List<EmployeeTemp>()
                    {
                        new EmployeeTemp() {Name = "Joe Watson", RC = 6},
                        new EmployeeTemp() {Name = "Joe Ann", RC = 6}
                    }
                },
                new Proposal()
                {
                    ProposalId = 7,
                    ProposalTitle = "Machine Learning with HTML v3",
                    Softskills = new List<string>() {"Office", "Risk Management Skills"},
                    Fields = new List<string>() {"Marketing", "Sales", "It-Support"},
                    Hardskills = new List<Hardskill>() {new Hardskill() {Description = "C++"}, new Hardskill() {Description = "Python"}},
                    AdditionalInfo = "Die finale Version des HTML - Projekts",
                    EmployeeTemps = new List<EmployeeTemp>()
                    {
                        new EmployeeTemp() {Name = "Joe Ann", RC = 6}
                    }
                },
            };
        }
        public ProposalService()
        {
            
        }

        public bool CreateProposal(string title, List<string> Softskills, List<string> Fields,
            List<Hardskill> Hardskills, List<EmployeeTemp> Employees)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateProposal(string title, List<string> Softskills, List<string> Fields,
            List<Hardskill> Hardskills, List<EmployeeTemp> Employees)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteProposal(int proposalId)
        {
            throw new System.NotImplementedException();
        }

        public bool CopyProposal(int proposalId)
        {
            throw new System.NotImplementedException();
        }

        public bool GenerateDocument(int proposalId)
        {
            throw new System.NotImplementedException();
        }

        public Proposal GetProposal(int proposalId)
        {
            throw new System.NotImplementedException();
        }

        public List<Proposal> GetAllProposal()
        {
            throw new System.NotImplementedException();
        }
    }
}