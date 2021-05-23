using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Team12.Data
{
    public class DatabaseJson
    {
        public List<string> skillList = new List<string>();
        public List<string> GetAllSkills()
        {
            
            using (StreamReader r = new StreamReader("data\\datenbasis.json"))
            {
                string json = r.ReadToEnd();
                dynamic array = JsonConvert.DeserializeObject(json);
                foreach (string item in array.skills.SprachenundFrameworks.Sprachen)
                {
                    skillList.Add(item);
                }
                
            }

            return skillList;
        }
    }
}