using System.Collections.Generic;

namespace AVATI.Data
{
    public class EmployeeTemp
    {
        public string Name { get; set; }
        public int RC { get; set; } = 1;
        public List<Hardskill> Hardskills { get; set; }
        public List<string> Sotfskills { get; set; }
        public List<string> fields { get; set; }
        public List<string> roles { get; set; }
    }
}