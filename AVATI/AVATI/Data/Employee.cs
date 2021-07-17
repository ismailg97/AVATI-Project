using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Net.Mime;
using Blazorise;
using Microsoft.AspNetCore.Components;



namespace AVATI.Data
{
    public class Employee
    {
        [NotNull][Required]
        public int EmployeeID { get; set; }
        
        [NotNull][Required(ErrorMessage = "Vorname ist erforderlich")]
        public string FirstName { get; set; }
        
        [NotNull][Required(ErrorMessage = "Nachname ist erforderlich")]
        public string LastName { get; set; }
        public string Image { get; set; }
        public float RelevantWorkExperience { get; set; }
        
        public DateTime EmploymentTime { get; set; }
        public int Rc { get; set; }
        public List<string> Softskills { get; set; } = new List<string>();
        public List<Hardskill> Hardskills { get; set; } = new List<Hardskill>();
        public List<Tuple<Hardskill, int>> HardSkillLevel { get; set; } = new List<Tuple<Hardskill, int>>();
        public List<string> Field { get; set; } = new List<string>();
        public List<string> Roles { get; set; } = new List<string>();
        public List<string> LanguageName { get; set; } = new List<string>();
        public List<Tuple<string, LanguageLevel>> Language { get; set; } = new List<Tuple<string, LanguageLevel>>();
        
        [NotNull]
        public EmployeeType EmpType { get; set; }
        
        [NotNull]
        public bool IsActive  { get; set; } = true;
        public List<ProjectActivity> ProjectActivities { get; set; } = new List<ProjectActivity>();
    }
}