using System.Collections.Generic;

namespace AVATI.Data
{
    public class Proposal
    {
        public int ProposalId { get; set; }
        public List<string> Softskills { get; set; }
        public List<string> Fields { get; set; }
        public List<Hardskill> Hardskills { get; set; }
        public string AdditionalInfo { get; set; }
        public List<EmployeeTemp> EmployeeTemps { get; set; }
    }
}