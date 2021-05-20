using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Team12.Data;
using System.IO;

namespace Team12.Data
{
    public class JsonImport : IJsonImport
    {
        List<Skill> allSprachen = new List<Skill>();
        public List<Skill> GetSkills()
        {

            using (StreamReader read = new StreamReader("Data\\datenbasis.json"))
            {
                string jsonstr = read.ReadToEnd();
                dynamic dyn = JsonConvert.DeserializeObject(jsonstr);

                foreach (var bezeichnung in dyn.skills.Sprachen_und_Frameworks.Sprachen)
                {
                    allSprachen.Add(new Skill { Id = 0, Name = bezeichnung, Skilltype = Skilltype.Hardskill });
                }
            }

            return allSprachen;
        }
        
    }
}
