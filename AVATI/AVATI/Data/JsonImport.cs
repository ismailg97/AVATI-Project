using System;
using System.IO;
using System.Text.Json;

namespace AVATI.Data
{
    public class JsonImport
    {
        //Loads JSon file into respective class
        public JSonStructure.Rootclass ImportJsonFile()
        {
            JSonStructure.Rootclass jSonContainer =
                JsonSerializer.Deserialize<JSonStructure.Rootclass>(File.ReadAllText("Data\\datenbasis.json"));


            foreach (var skillString in jSonContainer.skills.MethodenundProzesse.Modellierung)
            {
                Console.WriteLine(skillString);
            }
            return jSonContainer;
        }
    }
}