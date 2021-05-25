using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using static Team12.Data.Skill;

namespace Team12.Data
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
                    skilln.SkillType = Category.Hardskill;
                    SkillList.Add(skilln);
                }
                return SkillList;
            }
        }
    }
}







