using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using AVATI.Data.ValidationAttributes;

namespace AVATI.Data
{
    [DateTimeValidationAttribute]
    public class Proposal
    {
        [Required(ErrorMessage = "Name ist erforderlich")]
        [StringLength(70, ErrorMessage = "Angebotstitel ist zu lang (70 Zeichen)")]
        public string ProposalTitle { get; set; }
        
        public int ProposalID { get; set; }
        public List<string> Softskills { get; set; } = new List<string>();
        public List<string> Fields { get; set; } = new List<string>();
        public List<Hardskill> Hardskills { get; set; } = new List<Hardskill>();
        
        [StringLength(150, ErrorMessage = "Zusatztext ist zu lang (150 Zeichen)")]
        public string AdditionalInfo { get; set; }
        public List<Employee> Employees { get; set; } = new List<Employee>();

        public Dictionary<int, int> AltRc = new Dictionary<int, int>();
        
        public DateTime Start{ get; set; } = DateTime.Now;
        
        public DateTime End { get; set; } = DateTime.Now;
    }
}