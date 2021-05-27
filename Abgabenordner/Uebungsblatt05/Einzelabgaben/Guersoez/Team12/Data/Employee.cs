using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlazor.Data
{
    public class Employee
    {
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public List<string> Projekte { get; set; }
        public DateTime dateTime { get; set; }


    }
}
