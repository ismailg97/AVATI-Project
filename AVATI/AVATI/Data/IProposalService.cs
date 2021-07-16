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

        /// <summary>
        /// Loads Proposal from Database and compares with proposal Value. Deletes/Adds Attribute that got changed, If Id == 0 Creates new Proposal
        /// </summary>
        /// <param name="Id">Identification for Proposal</param>
        /// <param name="proposal">Object containing all values</param>
        /// <returns>True if Update successful, False if incorrect Id</returns>
        public bool UpdateProposal(int Id, Proposal proposal);

        /// <summary>
        /// Deletes Proposal including all values, Employees and related EmployeeDetail-Entries from Database
        /// </summary>
        /// <param name="proposalId">Identifier Proposal</param>
        /// <returns></returns>
        public bool DeleteProposal(int proposalId);

        public int CopyProposal(int proposalId);

        public bool GenerateDocument(int proposalId);

        public bool RemoveEmployee(int propId, int emp);

        /// <summary>
        /// Adds individual Employee to Proposal, also Initializes EmployeeDetail
        /// </summary>
        /// <param name="propId">Identifies Proposal</param>
        /// <param name="emp">Identifies Employee</param>
        /// <param name="rc">Added for potential alternate RC</param>
        /// <returns>true if successful</returns>
        public bool AddEmployee(int propId, int emp, int rc);
        
        /// <summary>
        /// Returns Singular Proposal including Fields, Softskills and Hardskills
        /// Does NOT include Attributes from Employees
        /// </summary>
        /// <param name="proposalId">Identifies Proposal</param>
        /// <returns>Object value if proposalId was found, null else</returns>
        public Proposal GetProposal(int proposalId);

        /// <summary>
        /// Returns a Task-Object of a List of all Proposals in Database including Fields, Sotfskills and Hardskills
        /// </summary>
        /// <returns></returns>
        public Task<List<Proposal>> GetAllProposals();
        
        
        /// <summary>
        /// Used to update AltRC of Employee in specific Proposal
        /// </summary>
        /// <param name="proposalId">Identifier Proposal</param>
        /// <param name="empId">Identifier Employee</param>
        /// <param name="newRc">New Rate-Card Value</param>
        /// <returns></returns>
        public bool UpdateAltRc(int proposalId, int empId, int newRc);
    }
}