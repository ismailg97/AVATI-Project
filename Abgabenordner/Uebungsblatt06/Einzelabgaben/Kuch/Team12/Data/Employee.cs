using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Team12.Data
{
    public class Employee
    {
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public DateTime Geburtstag { get; set; }

        public string[] PrevProjects { get; set; }


    }
}
