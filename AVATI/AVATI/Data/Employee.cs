using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;



namespace AVATI.Data
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image;
        public float RelevantWorkExperience { get; set; }
        public DateTime EmploymentTime { get; set; }
        public int Rc { get; set; }
        public List<string> Softskills { get; set; } = new List<string>();
        public List<Hardskill> Hardskills { get; set; } = new List<Hardskill>();
        public List<Tuple<Hardskill,int>> HardSkillLevel { get; set; }
        public List<string> Field { get; set; } = new List<string>();
        public List<string> Roles { get; set; } = new List<string>();
        public List<string> LanguageName { get; set; } = new List<string>();
        public List<Tuple<string, LanguageLevel>> Language { get; set; } = new List<Tuple<string, LanguageLevel>>();
        public EmployeeType EmpType { get; set; }
        public bool IsActive  { get; set; } = true;
        public List<ProjectActivity> ProjectActivities { get; set; } = new List<ProjectActivity>();
    }
}