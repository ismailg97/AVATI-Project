using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using DocumentFormat.OpenXml.Office.CustomUI;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlazor.Data
{
    [SkillNameConventionAttribut]
    public class Skill
    {
        public List<Skill> SkillList = new List<Skill>();

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public Category Skilltyp { get; set; }
        public enum Category
        {
            Hardskill,
            Softskill
        }

       
    }
}
