using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Team12.Data
{
    public class DatabaseJson
    {
        List<string> skill = new List<string>();

        public List<string> LoadJsonSkills()                                             //json datei aus datenbasis 'rausladen'
        {
            using (StreamReader r = new StreamReader("data\\datenbasis.json"))
            {
                string json = r.ReadToEnd();
                dynamic
                    array = JsonConvert
                        .DeserializeObject(
                            json); 
                //Quelle.: https://stackoverflow.com/questions/13297563/read-and-parse-a-json-file-in-c-sharp

                foreach (string item in array.skills.SprachenundFrameworks.Sprachen) //-> gleiche schreibweise wie in javasrcipt
                {
                    skill.Add(item);
                }
            }

            return skill;
        }
    }
}