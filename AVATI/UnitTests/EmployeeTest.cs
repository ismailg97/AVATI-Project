using System;
using System.IO;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace UnitTests
{
    public class EmployeeTest
    {
        public string Connection;

        [SetUp]
        public void Setup()
        {
            string json = File.ReadAllText("appsettings.json");
            JObject jObject = JObject.Parse(json);
            var name = (string) jObject["ConnectionStrings"]["TEST-Database"];
            Console.WriteLine(name);
            Connection =
                name;
        }
    }
}