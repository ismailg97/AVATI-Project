using System;
using System.IO;
using AVATI.Data;
using AVATI.Data.EmployeeDetailFiles;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace UnitTests
{
    public class LoginTest
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

        
        [TestCase("BitteFunktionier", true)]
        [TestCase("BitteFunktionierNicht", false)]
        [TestCase("Mach was du willst", true)]
        [TestCase("", false)]
        [Test]
        public void CheckUsernameAvailableTest(string username, bool isValid)
        {
            
            
            
            var loginService = new LoginService(Connection);
            
            if (isValid)
            {
                Assert.IsTrue(loginService.CheckUsernameAvailable(username));
            }
            else
            {
                loginService.CreateLogIn(username, "1234");
                
                Assert.IsFalse(loginService.CheckUsernameAvailable(username));
            }
            
        }
        

        [TestCase("EtePetete", "geheim", true )]
        [TestCase("", "geheim", false )]
        [TestCase("Test1", "", false )]
        [TestCase("Test1", "123456", false )]
        [TestCase("Something", "1234", true )]
        [TestCase("asdfjaoiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii" +
                  "iiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii",  "asdfjaoiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii" +
            "iiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii", false)]
        
        [Test]
        public void CreateLoginTest(string username, string password, bool isValid)
        {
            var loginService = new LoginService(Connection);
            if (isValid)
            {
                loginService.DeleteLogin(username);
                Assert.IsTrue(loginService.CreateLogIn(username, password));
            }
            else
            {
                Assert.IsFalse(loginService.CreateLogIn(username, password));
            }
            
            
        }
        
        
        
    }
}