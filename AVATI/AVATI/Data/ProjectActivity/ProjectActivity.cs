using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace AVATI.Data
{
    public class ProjectActivity
    {
        [Required][NotNull]
        public string Description { get; set; }
        public int ProjectID { get; set; }
        public int EmployeeID { get; set; }
        public int ProjectActivityID { get; set; }
        public List<string> SoftSkills { get; set; } = new List<string>();
        public List<Hardskill> HardSkills { get; set; } = new List<Hardskill>();
        public List<string> HardSkillsDesc { get; set; } = new List<string>();
    }
}