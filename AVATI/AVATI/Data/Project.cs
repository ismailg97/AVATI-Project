using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using AVATI.Data.ValidationAttributes;

namespace AVATI.Data
{
    [DateTimeValidationAttribute]
    public class Project
    {
        public int ProjectID { get; set; }          
        
        [Required(ErrorMessage = "Name ist erforderlich")]
        [StringLength(70, ErrorMessage = "Projekttitel ist zu lang (70 Zeichen)")]
        public string Projecttitel { get; set; }    
        
        [StringLength(150, ErrorMessage = "Projektbeschreibung ist zu lang (150 Zeichen)")]
        public string Projectdescription { get; set; }  
        public List<ProjectPurpose> Projectpurpose { get; set; } = new List<ProjectPurpose>(); 
        public List<ProjectActivity> ProjectActivities { get; set; } = new List<ProjectActivity>(); 
        
        public DateTime Projectbeginning { get; set; }  
        
        public DateTime Projectend { get; set; }    
        public List<string> Fields { get; set; } = new List<string>();  
        public List<Employee> Employees { get; set; } = new List<Employee>(); 
    }
}