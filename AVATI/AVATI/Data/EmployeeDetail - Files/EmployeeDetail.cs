using System;
using System.Collections.Generic;

namespace AVATI.Data
{
    public class EmployeeDetail
    {
        public int EmployeeId { get; set; }
        public int ProposalId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int Rc { get; set; }
        public List<string> Softskills { get; set; }
        public List<Hardskill> Hardskills { get; set; }
        public List<string> Fields { get; set; }
        public List<string> Roles { get; set; }
        public List<Tuple<string,LanguageLevel>> Languages { get; set; }
    }
}