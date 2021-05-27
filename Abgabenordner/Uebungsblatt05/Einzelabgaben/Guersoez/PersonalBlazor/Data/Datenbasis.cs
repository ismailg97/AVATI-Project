using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office.CustomUI;
using Newtonsoft.Json;
using static PersonalBlazor.Data.Skill;

namespace PersonalBlazor.Data
{
    public class Datenbasis : IDatenbasis
    {
        public List<Skill> SkillList = new List<Skill>();


        public List<Skill> LoadJson()
        {
            using (StreamReader r = new StreamReader("data\\datenbasis.json"))
            {
                SkillList.Clear();
                string json = r.ReadToEnd();
                dynamic skillListjson = JsonConvert.DeserializeObject(json);
                foreach (var skill in skillListjson.skills.SprachenundFrameworks.Sprachen)
                {
                    Skill skilln = new Skill();
                    skilln.Name = skill;
                    skilln.Skilltyp = Category.Hardskill;
                    SkillList.Add(skilln);
                }
                return SkillList;
            }
        }
    }
}



