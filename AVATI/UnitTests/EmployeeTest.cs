using System;
using System.IO;
using AVATI.Data;
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
        
        [Test]
        public void CheckForAvailable()
        {
            var empService =
                new EmployeeService(
                    Connection);
            
            
            var empList = empService.GetAllEmployees();
            foreach (var emp in empList)
            {

                Assert.IsTrue(emp.Field != null);
                    Assert.IsTrue(emp.Hardskills != null);
                    Assert.IsTrue(emp.HardSkillLevel != null);
                    Assert.IsTrue(emp.Softskills != null);
                    Assert.IsTrue(emp.ProjectActivities != null);
                    Assert.IsTrue(emp.EmployeeID >= 0);
                    Assert.IsTrue(emp.Roles != null);
                    Assert.IsTrue(emp.Language != null);
                    Assert.IsTrue(emp.LanguageName != null);
                
                
                
            }
        }
        
        
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]

        [Test]
        public void TestInput(int rc)
        {
            var empService =
                new EmployeeService(
                    Connection);
            var propService = new ProposalService(Connection);
            var propList = propService.GetAllProposals();

            foreach (var proposal in propList.Result)
            {
                var empList = empService.GetAllEmployees();
                foreach (var emp in empList)
                {
                    emp.Rc = rc;
                    empService.EditEmployeeProfile(emp);
                    Assert.IsTrue(empService.GetEmployeeProfile(emp.EmployeeID) != null);
                    Assert.IsTrue(empService.GetEmployeeProfile(emp.EmployeeID).Rc == rc);
                }
            }
        }
    }
}