using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AVATI.Data.EmployeeDetailFiles
{
    public class EmployeeDetail
    {
        [Required]
        public int ProposalId { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        
        [Range(1, 7, ErrorMessage = "Zahl muss zwischen 1 und 7 liegen!")]
        public int Rc { get; set; }
        [NotNull]
        public List<string> Softskills { get; set; } = new List<string>();
        [NotNull]
        public List<Hardskill> Hardskills { get; set; } = new List<Hardskill>();
        
        [NotNull]
        public List<string> Fields { get; set; } = new List<string>();
        
        [NotNull]
        public List<string> Roles { get; set; } = new List<string>();
        [NotNull]
        public List<Tuple<string, LanguageLevel>> Languages { get; set; } = new List<Tuple<string, LanguageLevel>>();
        [NotNull]

        public List<ProjectActivity> ProjectActivities { get; set; } = new List<ProjectActivity>();
        
        [Range(0, 100, ErrorMessage = "Rabat muss zwischen 0 und 100% liegen")]
        public int Discount;
    }
}