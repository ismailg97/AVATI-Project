using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AVATI.Data
{
    public class LoadFieldFromJson
    {
        private readonly List<string> fields = new();

        public List<string> LoadFielFromJsonFile()
        {
            JSonStructure.Rootclass jsoncontainer =
                JsonSerializer.Deserialize<JSonStructure.Rootclass>(File.ReadAllText("Data/datenbasis.json"));
            
            foreach(var field in jsoncontainer.fields)
            {
                fields.Add(field);
            }
            return fields;
        }
        
    }
}