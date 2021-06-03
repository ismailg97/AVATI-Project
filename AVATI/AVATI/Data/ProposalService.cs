using System.Collections.Generic;

namespace AVATI.Data
{
    public class ProposalService : IProposalService
    {
        public List<Proposal> Proposals { get; set; }

        public ProposalService()
        {
            Proposals.Add(new Proposal() {});
        }

        public bool CreateProposal(string title, List<string> Softskills, List<string> Fields, List<Hardskill> Hardskills, List<EmployeeTemp> Employees)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateProposal(string title, List<string> Softskills, List<string> Fields, List<Hardskill> Hardskills, List<EmployeeTemp> Employees)
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