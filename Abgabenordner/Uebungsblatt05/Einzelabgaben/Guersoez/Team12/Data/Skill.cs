using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;


namespace Team12.Data { 
    public struct Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Category SkillType { get; set; }
        public enum Category
        {
            Hardskill,
            Softskill,
        }


    }

}