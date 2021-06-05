﻿using System;
using System.Collections.Generic;


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
        public List<string> Softskills { get; set; }
        public List<Hardskill> Hardskills { get; set; }
        public List<string> Field { get; set; }
        public List<string> Roles { get; set; }
        public List<Tuple<string,LanguageLevel>> Language { get; set; }
        public EmployeeType EmpType { get; set; }
        public bool IsActive  { get; set; } = true;
    }
}