using System;
using System.Collections.Generic;
using System.Linq;

namespace AVATI.Data
{
    public class ProposalService : IProposalService
    {
        public List<Proposal> Proposals { get; set; }


        public ProposalService()
        {
            Proposals = new List<Proposal>()
            {
                new Proposal()
                {
                    ProposalId = 1,
                    ProposalTitle = "Machine Learning with HTML",
                    Softskills = new List<string>() {"Office", "Risk Management Skills"},
                    Fields = new List<string>() {"Marketing", "Sales", "It-Support"},
                    Hardskills = new List<Hardskill>()
                        {new Hardskill() {Description = "C++"}, new Hardskill() {Description = "Python"}},
                    AdditionalInfo = "Das Projekt versucht einer HTML - Seite Tetris beizubringen",
                    Employees = new List<Employee>()
                    {
                        new Employee() {FirstName = "Watson", LastName = "Smith", Rc = 6},
                        new Employee() {FirstName = "Sotaw", LastName = "Smith", Rc = 3},
                        new Employee() {FirstName = "Norman", LastName = "Smith", Rc = 1}
                    }
                },
                new Proposal()
                {
                    ProposalId = 2,
                    ProposalTitle = "Webanwendung mit C++ und Javascript",
                    Softskills = new List<string>() {"Bootstrap 5 Modal Dialogs", "Charisma"},
                    Fields = new List<string>() {"Design", "Wetter", "It-Support"},
                    Hardskills = new List<Hardskill>()
                        {new Hardskill() {Description = "C++"}, new Hardskill() {Description = "JavaScript"}},
                    AdditionalInfo = "Die beiden Programmiersprachen sollen für diese Anwendung kombiniert werden",
                    Employees = new List<Employee>()
                    {
                        new Employee() {FirstName = "Watson", LastName = "Smith", Rc = 6},
                        new Employee() {FirstName = "Watson", LastName = "Smith", Rc = 6},
                        new Employee() {FirstName = "Watson", LastName = "Smith", Rc = 3}
                    }
                },
                new Proposal()
                {
                    ProposalId = 3,
                    ProposalTitle = "Facial Recognition with Smartphones",
                    Softskills = new List<string>() {"UML - Klassendiagramme", "Risk Management Skills"},
                    Fields = new List<string>() {"Marketing", "Sales", "AI - Development"},
                    Hardskills = new List<Hardskill>()
                        {new Hardskill() {Description = "C#"}, new Hardskill() {Description = "Python"}},
                    AdditionalInfo = "Facial Regognition, die unabhängig vom Smartphone funktioniert",
                    Employees = new List<Employee>()
                    {
                        new Employee() {FirstName = "Watson", LastName = "Smith", Rc = 6},
                        new Employee() {FirstName = "Watson", LastName = "Smith", Rc = 6},
                        new Employee() {FirstName = "Watson", LastName = "Smith", Rc = 6}
                    }
                },
                new Proposal()
                {
                    ProposalId = 4,
                    ProposalTitle = "Machine Learning with HTML v2",
                    Softskills = new List<string>() {"Office", "Risk Management Skills"},
                    Fields = new List<string>() {"Marketing", "Sales", "It-Support"},
                    Hardskills = new List<Hardskill>()
                        {new Hardskill() {Description = "C++"}, new Hardskill() {Description = "Python"}},
                    AdditionalInfo = "Das Projekt versucht einer HTML - Seite Tetris beizubringen",
                    Employees = new List<Employee>()
                    {
                        new Employee() {FirstName = "Watson", LastName = "Smith", Rc = 6},
                        new Employee() {FirstName = "Watson", LastName = "Smith", Rc = 6}
                    }
                },
                new Proposal()
                {
                    ProposalId = 5,
                    ProposalTitle = "Facial Recognition with Security Kameras",
                    Softskills = new List<string>() {"Office", "Risk Management Skills"},
                    Fields = new List<string>() {"Marketing", "Sales", "It-Support"},
                    Hardskills = new List<Hardskill>()
                        {new Hardskill() {Description = "C++"}, new Hardskill() {Description = "Python"}},
                    AdditionalInfo = "Das System soll Einbrecher identifizieren können",
                    Employees = new List<Employee>()
                    {
                        new Employee() {FirstName = "Watson", LastName = "Smith", Rc = 6},
                        new Employee() {FirstName = "Watson", LastName = "Smith", Rc = 6}
                    }
                },
                new Proposal()
                {
                    ProposalId = 6,
                    ProposalTitle = "Web 4.0",
                    Softskills = new List<string>()
                        {"Libre Office", "Risk Management Skills", "Grundlegende Mathematik"},
                    Fields = new List<string>() {"Marketing", "Sales", "It-Support"},
                    Hardskills = new List<Hardskill>()
                        {new Hardskill() {Description = "C++"}, new Hardskill() {Description = "Python"}},
                    AdditionalInfo = "<<Empty>>",
                    Employees = new List<Employee>()
                    {
                        new Employee() {FirstName = "Watson", LastName = "Smith", Rc = 6},
                        new Employee() {FirstName = "Watson", LastName = "Smith", Rc = 6}
                    }
                },
                new Proposal()
                {
                    ProposalId = 7,
                    ProposalTitle = "Machine Learning with HTML v3",
                    Softskills = new List<string>() {"Office", "Risk Management Skills"},
                    Fields = new List<string>() {"Marketing", "Sales", "It-Support"},
                    Hardskills = new List<Hardskill>()
                        {new Hardskill() {Description = "C++"}, new Hardskill() {Description = "Python"}},
                    AdditionalInfo = "Die finale Version des HTML - Projekts",
                    Employees = new List<Employee>()
                    {
                        new Employee() {FirstName = "Watson", LastName = "Smith", Rc = 6}
                    }
                },
            };
        }


        public bool CreateProposal(string title, List<string> Softskills, List<string> Fields,
            List<Hardskill> Hardskills, List<Employee> Employees)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateProposal(int id, Proposal proposal)
        {
            Proposal temp = GetProposal(id);
            if (temp != null)
            {
                temp = proposal;
                return true;
            }

            return false;
        }

        public bool DeleteProposal(int proposalId)
        {
            if (GetProposal(proposalId) != null)
            {
                Proposals.Remove(Proposals.Find(e => e.ProposalId == proposalId));
                return true;
            }

            return false;
        }

        public int CopyProposal(int proposalId)
        {
            Proposal temp;
            if (GetProposal(proposalId) != null)
            {
                temp = GetProposal(proposalId);
                Proposals.Add(new Proposal()
                {
                    ProposalId = Proposals.Max(e => e.ProposalId) + 1, Employees = temp.Employees, Fields = temp.Fields,
                    Hardskills = temp.Hardskills, Softskills = temp.Softskills, AdditionalInfo = temp.AdditionalInfo,
                    ProposalTitle = String.Concat(temp.ProposalTitle, " [KOPIE]") 
                });
                
                return Proposals.Max(e => e.ProposalId);
            }

            return 0;
        }

        public bool GenerateDocument(int proposalId)
        {
            throw new System.NotImplementedException();
        }

        public Proposal GetProposal(int proposalId)
        {
            if (Proposals.Find(e => e.ProposalId == proposalId) != null)
            {
                return Proposals.Find(e => e.ProposalId == proposalId);
            }
            else
            {
                return null;
            }
        }

        public List<Proposal> GetAllProposal()
        {
            return Proposals;
        }
    }
}