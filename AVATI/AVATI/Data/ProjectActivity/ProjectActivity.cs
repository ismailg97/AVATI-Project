using System.Collections.Generic;

namespace AVATI.Data
{
    public class ProjectActivity
    {
        public string Description { get; set; }
        public int ProjectID { get; set; }
        public int EmployeeID { get; set; }
        public List<string> SoftSkills { get; set; }
        public List<Hardskill> HardSkills { get; set; }
    }
}