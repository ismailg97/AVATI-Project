using System;
using System.Collections.Generic;
using System.Linq;

namespace AVATI.Data
{
    public class ProposalService : IProposalService
    {
        public List<Proposal> Proposals { get; set; }
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

        public ProposalService()
        {
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
            else
            {
                if (proposal.ProposalTitle == null)
                {
                    proposal.ProposalTitle = "leer";
                }
                proposal.ProposalID = Proposals.Max(e => e.ProposalID) + 1;
                Proposals.Add(proposal);
            }

            return false;
        }

        public bool DeleteProposal(int proposalId)
        {
            if (GetProposal(proposalId) != null)
            {
                Proposals.Remove(Proposals.Find(e => e.ProposalID == proposalId));
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
                    ProposalID = Proposals.Max(e => e.ProposalID) + 1, Employees = temp.Employees, Fields = temp.Fields,
                    Hardskills = temp.Hardskills, Softskills = temp.Softskills, AdditionalInfo = temp.AdditionalInfo,
                    ProposalTitle = String.Concat(temp.ProposalTitle, " [KOPIE]")
                });

                return Proposals.Max(e => e.ProposalID);
            }

            return 0;
        }

        public bool GenerateDocument(int proposalId)
        {
            throw new System.NotImplementedException();
        }

        public Proposal GetProposal(int proposalId)
        {
            if (Proposals.Find(e => e.ProposalID == proposalId) != null)
            {
                return Proposals.Find(e => e.ProposalID == proposalId);
            }
            else
            {
                return null;
            }
        }

        public List<Proposal> GetAllProposals()
        {
            return Proposals;
        }
    }
}