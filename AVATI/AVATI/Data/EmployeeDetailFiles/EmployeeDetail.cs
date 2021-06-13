using System;
using System.Collections.Generic;

namespace AVATI.Data.EmployeeDetailFiles
{
    public class EmployeeDetail
    {
        public int EmployeeId { get; set; }
        public int ProposalId { get; set; }
        
        public int Rc { get; set; }
        public List<string> Softskills { get; set; } = new List<string>();
        public List<Hardskill> Hardskills { get; set; } = new List<Hardskill>();
        public List<string> Fields { get; set; } = new List<string>();
        public List<string> Roles { get; set; } = new List<string>();
        public List<Tuple<string, LanguageLevel>> Languages { get; set; } = new List<Tuple<string, LanguageLevel>>();

        public bool SoftVis(string softskill)
        {
            return Softskills.Contains(softskill);
        }

        public bool HardVis(Hardskill hardskill)
        {
            return Hardskills.Find(e => e.Description.Equals(hardskill.Description)) != null;
        }

        public bool RolesVis(string role)
        {
            return Roles.Contains(role);
        }

        public bool FieldVis(string field)
        {
            return Fields.Contains(field);
        }

        public bool LanguageVis(string language)
        {
            return Languages.Find(e => e.Item1 == language) != null;
        }

        public bool RcVis()
        {
            return Rc != 0;
        }
    }
}