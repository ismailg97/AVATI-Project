using System;
using System.Collections.Generic;

namespace Team12.Data
{
    public class Employee
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string[] Fin_Projects { get; set; }
        public DateTime Day_of_Birth { get; set; }
    }
}