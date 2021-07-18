using System;
using System.IO;
using AVATI.Data;
using AVATI.Data.EmployeeDetailFiles;
using DocumentFormat.OpenXml.Bibliography;
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


        [TestCase("Test123","123", true)]
        [TestCase("Test234"," 123", true)]
        [TestCase("Nichtdrin", "123", false)]
        [TestCase("","", false)]

        [Test]
        public void LoginCheckTest(string username, string password, bool isValid)
        {
            var loginService = new LoginService(Connection);
            if (isValid)
            {
                loginService.CreateLogIn(username, password);
                Assert.IsTrue(loginService.LogIn(username, password) == -2);
            }
            else
            {
                Assert.IsFalse(loginService.LogIn(username, password) >= 0);
            }
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
                loginService.DeleteLogin(username);
                Assert.IsTrue(loginService.CheckUsernameAvailable(username));
            }
            else
            {
                loginService.CreateLogIn(username, "1234");
                Assert.IsFalse(loginService.CheckUsernameAvailable(username));
                loginService.DeleteLogin(username);
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

        
        
        [TestCase("EtePetete", "geheim", true )]
        [TestCase("", "geheim", false )]
        [TestCase("Test22", "", false )]
        [TestCase("Test23", "123456", false )]
        [TestCase("Something", "1234", true )]
        [TestCase("asdfjaoiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii" +
                  "iiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii",  "asdfjaoiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii" +
            "iiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii", false)]
        
        [Test]
        public void DeleteLoginTest(string username, string password, bool isValid)
        {
            var loginService = new LoginService(Connection);
            if (isValid)
            {
                loginService.CreateLogIn(username, password);
                Assert.IsTrue(loginService.DeleteLogin(username));
            }
            else
            {
            
                Assert.IsFalse(loginService.DeleteLogin(username));
            }
        }
        
        
        
    }
}