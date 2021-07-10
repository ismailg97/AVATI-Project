using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AVATI.Data.EmployeeDetailFiles
{
    public class EmployeeDetail
    {
        public int ProposalId { get; set; }
        public int EmployeeId { get; set; }
        
        [Range(1, 7, ErrorMessage = "Zahl muss zwischen 1 und 7 liegen!")]
        public int Rc { get; set; }
        public List<string> Softskills { get; set; } = new List<string>();
        public List<Hardskill> Hardskills { get; set; } = new List<Hardskill>();
        public List<string> Fields { get; set; } = new List<string>();
        public List<string> Roles { get; set; } = new List<string>();
        public List<Tuple<string, LanguageLevel>> Languages { get; set; } = new List<Tuple<string, LanguageLevel>>();

        public List<ProjectActivity> ProjectActivities { get; set; } = new List<ProjectActivity>();
        
        [Range(0, 100, ErrorMessage = "Rabat muss zwischen 0 und 100% liegen")]
        public int Discount;
    }
}