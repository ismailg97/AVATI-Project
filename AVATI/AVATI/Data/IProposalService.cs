using System.Collections.Generic;

namespace AVATI.Data
{
    public interface IProposalService
    {
        public List<Proposal> Proposals { get; set; }

        public List<string> GetSoftskills(int proposalId);

        public List<Hardskill> GetHardskills(int proposalId);

        public List<string> GetFields(int proposalId);
                
        public bool CreateProposal(string title, List<string> Softskills, List<string> Fields, List<Hardskill> Hardskills, List<Employee> Employees);

        public bool UpdateProposal(int Id, Proposal proposal);

        public bool DeleteProposal(int proposalId);

        public int CopyProposal(int proposalId);

        public bool GenerateDocument(int proposalId);

        public Proposal GetProposal(int proposalId);

        public List<Proposal> GetAllProposals();
    }
}