using System;
using System.Collections.Generic;

namespace Team12.Data
{
    public class Employee
    {
        public String vorname { get; set; }
        public String nachname { get; set; }
        public List<String> projekte { get; set; }
        public DateTime geburtstag { get; set; }

    }
}
