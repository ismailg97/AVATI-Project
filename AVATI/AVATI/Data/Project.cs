using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AVATI.Data
{
    
    public class Project
    {
        public int ProjectID { get; set; }
        public string Projecttitel { get; set; }
        public string Projectdescription { get; set; }
        public string[] Projectpurpose { get; set; }
        //public ProjectActivity[] Projectactivities { get; set; }
        public DateTime Runtimme { get; set; }                  
        public List<string> fields { get; set; }
        //public Employee[] Employees { get; set; }
    }
}