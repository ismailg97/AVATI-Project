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
        public List<string> Projectpurpose { get; set; } = new List<string>();
        public List<ProjectActivity> ProjectActivities { get; set; } = new List<ProjectActivity>();
        public DateTime Projectbeginning { get; set; }
        public DateTime Projectend { get; set; }
        public List<string> Fields { get; set; } = new List<string>();

        public List<string> Softskills { get; set; } = new List<string>();

        public List<Hardskill> Hardskills { get; set; } = new List<Hardskill>();
        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}