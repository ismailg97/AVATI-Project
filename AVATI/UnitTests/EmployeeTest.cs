using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AVATI.Data;
using AVATI.Data.ValidationAttributes;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;

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
        
        
        public static IEnumerable<TestCaseData> GetDateTests()
        {
            var testCaseData = new List<TestCaseData>();
            testCaseData.Add(new TestCaseData(DateTime.Now.AddDays(144), true));
            testCaseData.Add(new TestCaseData(DateTime.Now.AddDays(144), true));
            testCaseData.Add(new TestCaseData(DateTime.Now.AddDays(-30000), false));
            testCaseData.Add(new TestCaseData(DateTime.Now.AddDays(-100000), false));
            testCaseData.Add(new TestCaseData(DateTime.Now.AddDays(-50), true));
            testCaseData.Add(new TestCaseData(DateTime.Now.AddDays(0), true));
            return testCaseData.AsEnumerable();
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
        public void TestInputRcLevel(int rc)
        {
            var empService =
                new EmployeeService(
                    Connection);
            var propService = new ProposalService(Connection);
            var propList = propService.GetAllProposals();

            
                var empList = empService.GetAllEmployees();
                foreach (var emp in empList)
                {
                    emp.Rc = rc;
                    empService.EditEmployeeProfile(emp);
                    Assert.IsTrue(empService.GetEmployeeProfile(emp.EmployeeID) != null);
                    Assert.IsTrue(empService.GetEmployeeProfile(emp.EmployeeID).Rc == rc);
                }
            
        }

        
        [TestCaseSource("GetDateTests")]
        [Test]
        public void TestInputEmploymentTime( DateTime date, bool isValid)
        {
            var empService = new EmployeeService(Connection);
            Employee emp = new Employee()
            {
                EmployeeID = 1, EmploymentTime = date, FirstName = "rsgsr", LastName = "sastz"
            };
            emp.EmploymentTime = date;
            if (isValid)
            {
                Assert.IsTrue(emp.EmploymentTime.Year > 2000);
            }
            else
            {
                Assert.IsFalse(emp.EmploymentTime.Year > 2000);
            }
           
            
            
        }
        
        
        [TestCase("Architektur/Bau/Immobilien")]
        [TestCase( "Druck/Verpackung")]
        [TestCase("Forschung/Entwicklung")]
        [Test]
        public void TestInputField(string fields)
        {
            var empService =
                new EmployeeService(
                    Connection);
            var empList = empService.GetAllEmployees();

            foreach (var emp in empList)
            {
                if (empService.GetEmployeeProfile(emp.EmployeeID).Field.Find(x => x == fields) == null)
                {
                  emp.Field.Add(fields);
                    empService.EditEmployeeProfile(emp);  
                }
                
               
                    Assert.IsTrue(empService.GetEmployeeProfile(emp.EmployeeID) != null);
                    Assert.IsTrue(empService.GetEmployeeProfile(emp.EmployeeID).Field.Find(x => x == fields) != null);
            }
        
        }
        
        
        [TestCase(
            "asdfjaoiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii" +
            "iiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii",
            false)]
        [TestCase("", false)]
        [TestCase("Thine hollowed heavens", true)]
        [TestCase(
            "CAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAPPPPPPPPPPPPSLOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOCKkkkkkkkk",
            false)]
        public async Task TestDbManipulation(string firstname, bool isValid)
        {
            var empService =
                new EmployeeService(
                    Connection);
            var list = empService.GetAllEmployees();
           
            if (list.Count == 0)
            {
                if (isValid)
                {
                    Assert.IsTrue(empService.EditEmployeeProfile(new Employee()
                    {
                        EmployeeID = 1, FirstName = firstname, LastName = firstname
                    }));
                }
                else
                {
                    Assert.IsFalse(empService.EditEmployeeProfile(new Employee()
                    {
                        EmployeeID = 1, FirstName = firstname, LastName = firstname
                    }));
                }
            }
            else
            {
                list[0].FirstName = firstname;
                list[0].LastName = firstname;
                if (isValid)
                {
                    Assert.IsTrue(empService.EditEmployeeProfile(list[0]));
                    Assert.IsTrue(empService.GetEmployeeProfile(list[0].EmployeeID) != null &&
                                  empService.GetEmployeeProfile(list[0].EmployeeID).FirstName == firstname);
                    Assert.IsTrue(empService.GetEmployeeProfile(list[0].EmployeeID) != null &&
                                  empService.GetEmployeeProfile(list[0].EmployeeID).LastName == firstname);
                }
                else
                {
                    Assert.IsFalse(empService.EditEmployeeProfile(list[0]));
                    Assert.IsTrue(empService.GetEmployeeProfile(list[0].EmployeeID) != null &&
                                  empService.GetEmployeeProfile(list[0].EmployeeID).FirstName != firstname);
                    Assert.IsTrue(empService.GetEmployeeProfile(list[0].EmployeeID) != null &&
                                  empService.GetEmployeeProfile(list[0].EmployeeID).LastName != firstname);
                }
            }
        }
        
        
        [Test]
        public void EditandGetStatus()
        {
            var empService = new EmployeeService(Connection);
            bool status = false;
            var allEmps = empService.GetAllEmployees();
            foreach (var emp in allEmps)
            {
                
                Assert.IsTrue(empService.EditStatus(emp.EmployeeID, status));
                Assert.IsTrue(empService.GetStatus(emp.EmployeeID) == status);
            }
            
        }
    }
}