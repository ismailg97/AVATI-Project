using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Team12.Data
{
    public class DatabaseJson
    {
        private readonly List<string> skill = new();

        public List<string> LoadJsonSkills() //json datei aus datenbasis 'rausladen'
        {
            using (var r = new StreamReader("data\\datenbasis.json"))
            {
                var json = r.ReadToEnd();
                dynamic
                    array = JsonConvert
                        .DeserializeObject(
                            json);
                //Quelle.: https://stackoverflow.com/questions/13297563/read-and-parse-a-json-file-in-c-sharp

                foreach (string item in
                    array.skills.SprachenundFrameworks.Sprachen) //-> gleiche schreibweise wie in javasrcipt
                    skill.Add(item);
            }

            return skill;
        }
    }
}