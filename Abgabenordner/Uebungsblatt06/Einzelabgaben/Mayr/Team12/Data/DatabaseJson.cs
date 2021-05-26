using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Team12.Data
{
    public class DatabaseJson
    {
        public List<string> LoadJson()
        {
            /*List<string> list = new List<string>();
            using (StreamReader r = new StreamReader("Data/datenbasis.json"))
            {
                string json = r.ReadToEnd();
                dynamic array = JsonConvert.DeserializeObject(json);
                foreach(string item in array.skills.SprachenundFrameworks.Sprachen) //runtime binding on null reference??
                {
                    list.Add(item);
                }
            }
            return list;*/
            List<string> list = new List<string> { };
            string json = File.ReadAllText("Data/datenbasis.json");
            JObject raw_results = JObject.Parse(json);
            JEnumerable<JToken> results = raw_results["skills"]["Sprachen und Frameworks"]["Sprachen"].Children();
            foreach (var item in results)
            {
                list.Add(item.ToString());
            }
            return list;
        }
    }
}