using System.Collections.Generic;
using System.Threading.Tasks;

namespace AVATI.Data
{
    public interface IProposalService
    {
        public List<Proposal> Proposals { get; set; }

        public List<string> GetSoftskills(int proposalId);

        public List<Hardskill> GetHardskills(int proposalId);

        public List<string> GetFields(int proposalId);

        public bool UpdateProposal(int Id, Proposal proposal);

        public bool DeleteProposal(int proposalId);

        public int CopyProposal(int proposalId);

        public bool GenerateDocument(int proposalId);

        public bool RemoveEmployee(int propId, int emp);

        public bool AddEmployee(int propId, int emp, int rc);
        
        public Proposal GetProposal(int proposalId);

        public Task<List<Proposal>> GetAllProposals();

        public bool UpdateAltRc(int proposalId, int empId, int newRc);
    }
}