using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AVATI.Data;
using AVATI.Data.ValidationAttributes;
using Bunit.Extensions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace UnitTests
{
    public class ProjectTest
    {
        public string connection;
        public ProjectService ProjectService;
        public ProjectActivityService2 ActivityService2;
        
        [SetUp]
        public void Setup()
        {
            string json = File.ReadAllText("appsettings.json");
            JObject jObject = JObject.Parse(json);
            var name = (string) jObject["ConnectionStrings"]["TEST-Database"];
            connection = name;
            ProjectService = new ProjectService(connection);
            ActivityService2 = new ProjectActivityService2(connection);
        }

        public static IEnumerable<TestCaseData> GetDateTests()
        {
            var testCaseData = new List<TestCaseData>();
            testCaseData.Add(new TestCaseData(DateTime.Now.AddDays(144), DateTime.Now.AddDays(333), true));
            testCaseData.Add(new TestCaseData(DateTime.Now.AddDays(144), DateTime.Now.AddDays(333), true));
            testCaseData.Add(new TestCaseData(DateTime.Now.AddDays(12), DateTime.Now.AddDays(0), false));
            testCaseData.Add(new TestCaseData(DateTime.Now.AddDays(112), DateTime.Now.AddDays(31), false));
            testCaseData.Add(new TestCaseData(DateTime.Now.AddDays(12), DateTime.Now.AddDays(99), true));
            testCaseData.Add(new TestCaseData(DateTime.Now.AddDays(99), DateTime.Now.AddDays(12), false));
            testCaseData.Add(new TestCaseData(DateTime.Now.AddDays(0), DateTime.Now.AddDays(0), true));
            return testCaseData.AsEnumerable();
        }
        
        public static IEnumerable<TestCaseData> CreateDummyProjects()
        {
            var testCaseData = new List<TestCaseData>();
            testCaseData.Add(new TestCaseData(new Project()
            {
                Projecttitel = "Just a small test no biggie", Projectbeginning = DateTime.Now,
                Projectend = DateTime.Now, Employees = new List<Employee>(),
                Projectdescription = "Just a small Project nothing more", Fields = new List<string>(),
                 ProjectActivities = new List<string>(),
            }));
            testCaseData.Add(new TestCaseData(new Project()
            {
                Projecttitel = "This should also work?", Projectbeginning = DateTime.Now.AddDays(-30),
                Projectend = DateTime.Now.AddDays(30), Employees = new List<Employee>(),
                Projectdescription = "Just a small Project nothing more or less", Fields = new List<string>(),
                 ProjectActivities = new List<string>(),
            }));
            testCaseData.Add(new TestCaseData(new Project()
            {
                Projecttitel = "Jusadssdasaddsasdasadadst a smallasdadsdasdas", Projectbeginning = DateTime.Now,
                Projectend = DateTime.Now, Employees = new List<Employee>(),
                Projectdescription = "Just a smasdsadsaddasdasdasdasdasddsaasddsasaddasdsadsa", Fields = new List<string>(),
                 ProjectActivities = new List<string>(),
            }));
            testCaseData.Add(new TestCaseData(new Project()
            {
                Projecttitel = "@@@@_@@@@@@", Projectbeginning = DateTime.Now.AddDays(-400),
                Projectend = DateTime.Now.AddDays(-200), Employees = new List<Employee>(),
                Projectdescription = "", Fields = new List<string>(),
                ProjectActivities = new List<string>(),
            }));
            testCaseData.Add(new TestCaseData(new Project()
            {
                Projecttitel = " asd", Projectbeginning = DateTime.Now.AddDays(-1),
                Projectend = DateTime.Now, Employees = new List<Employee>(),
                Projectdescription = "sad", Fields = new List<string>(),
                 ProjectActivities = new List<string>(),
            }));
            testCaseData.Add(new TestCaseData(new Project()
            {
                Projecttitel = "Just a small test no biggie", Projectbeginning = DateTime.Now,
                Projectend = DateTime.Now, Employees = new List<Employee>(),
                Projectdescription = "Just a small Project nothing more. On 15 April, Chancellor Angela Merkel spoke of fragile intermediate success" +
                                     " that had been achieved in the fight against the pandemic. " +
                                     "The same day, a first loosening of restrictions was announced, continued in early May, and eventually, holiday travels were allowed in cooperation with other European countries.", Fields = new List<string>(),
                 ProjectActivities = new List<string>(),
            }));
            return testCaseData.AsEnumerable();
        }

        public static IEnumerable<TestCaseData> CreateDummyActivitiets()
        {
            var testCaseData = new List<TestCaseData>();
            testCaseData.Add(new TestCaseData("Just a smol biggie nooo biggie","Hier ist ein neuer String"));
            testCaseData.Add(new TestCaseData("du bringst mich noch auf die Palme!", "wow"));
            testCaseData.Add(new TestCaseData("12345678912345678134567891234567891234567891234567897654327892345675467523445623589279457834695783469587276278356", "oki"));
            testCaseData.Add(new TestCaseData("why?", "wow"));
            testCaseData.Add(new TestCaseData("wow", "ok"));
            return testCaseData.AsEnumerable();
        }

        [TestCaseSource("CreateDummyActivitiets")]
        public void TestValidProjectactivityDescription(string input, string old)
        {
            ActivityService2.AddGlobalProjectActivity(old);
            Assert.IsTrue(ActivityService2.AlreadyExistsGlobalActivity(old));
            ActivityService2.UpdateGlobalProjectActivity(old, input);
            Assert.IsTrue(ActivityService2.AlreadyExistsGlobalActivity(input));
            ActivityService2.DeleteGlobalProjectActivity(input);
            Assert.IsFalse(ActivityService2.AlreadyExistsGlobalActivity(input));
            Assert.IsNotNull(ActivityService2.GetAllGlobalProjectActivities());
        }

        [TestCaseSource("GetDateTests")]
        public void ProjectDateTest(DateTime beginning, DateTime end, bool isValid)
        {
            var project = new Project() {Projectbeginning = beginning, Projectend = end, Projecttitel = "valid"};
            List<ValidationAttribute> validationAttributes = new List<ValidationAttribute>
            {
                new DateTimeValidationAttribute()
            };
            var ctx = new ValidationContext(project);
            if (isValid)
                Assert.DoesNotThrow(delegate { Validator.ValidateValue(project, ctx, validationAttributes); });
            else
                Assert.Throws<ValidationException>(delegate
                {
                    Validator.ValidateValue(project, ctx, validationAttributes);
                });
        }

        [TestCase("asadsasddsa", "asdasddsasadadsadsdas", true)]
        [TestCase("^^__^^!!!@@@loAAAA", "öäöääöäöäö??!!!", true)]
        [TestCase("", "asdasddsasadadsadsdas", false)]
        [TestCase("asadsasddsa", "", true)]
        [TestCase(".",
            "asdasddsasadadsadasdddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd" +
            "asdddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddsdasasdddddddddddddddddddd" +
            "dddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd",
            false)]
        [TestCase("CAPSLOCKISTAUCHINORDNUNG", "", true)]
        [TestCase(
            "DieserTitelistvielzulangdaswäredochvielzuvielfürdiedatenbanklieberetwaskürzerfassenkeinmenschmöchtes" +
            "ovieltextlesenaberdasistnurmeinepersönlichemeinung" +
            "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!",
            ":)", false)]
        public void TestValidationProject(string proposalTitle, string proposalAddInfo, bool isValid)
        {
            var proposal = new Proposal() {ProposalTitle = proposalTitle, AdditionalInfo = proposalAddInfo};
            List<ValidationAttribute> validationAttributes = new List<ValidationAttribute>
            {
                new DateTimeValidationAttribute()
            };
            var ctx = new ValidationContext(proposal);
            if (isValid)
                Assert.DoesNotThrow(delegate { Validator.ValidateValue(proposal, ctx, validationAttributes); });
            else
                Assert.Throws<ValidationException>(delegate
                {
                    Validator.ValidateValue(proposal, ctx, validationAttributes);
                });
        }

        [Test]
        public async Task TestProjectsinDatabase()
        {
            var projectService = new ProjectService(connection);
            var result = projectService.GetAllProjects();
            Assert.IsNotNull(result);
            foreach (var project in result)
            {
                Assert.IsTrue(project.Projecttitel != null);
                Assert.IsTrue(project.ProjectID != 0);
                Assert.IsTrue(project.Employees != null);
                Assert.IsTrue(project.Fields != null);
                Assert.IsTrue(project.Projectdescription != null);
                Assert.IsTrue(project.Projectpurpose != null);
                Assert.IsTrue(project.Projectend >= project.Projectbeginning);
            }
        }

        [TestCaseSource("CreateDummyProjects")]
        public async Task TestCreationOfProjects(Project dummy)
        {
            
            Assert.IsTrue(ProjectService.CreateProject(dummy));
            var list = ProjectService.GetAllProjects();
            Assert.IsNotNull(list);
            var result = list.Find(e =>
                e.Projecttitel.Equals(dummy.Projecttitel) && e.Projectdescription.Equals(dummy.Projectdescription));
            Assert.IsNotNull(result);
            result.Fields = new List<string>()
            {
                "Automobil", "IT", "Kunst/Kultur", "Gastronomie"
            };
            result.Employees.Add(new Employee()
            {
                EmployeeID = 14
            });
            result.Projectbeginning = DateTime.Now;
            result.Projectend = DateTime.Now.AddDays(4000);
            Assert.IsTrue(ProjectService.UpdateProject(result));
            result = list.Find(e =>
                e.Projecttitel.Equals(dummy.Projecttitel) && e.Projectdescription.Equals(dummy.Projectdescription));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Employees.Exists(e => e.EmployeeID == 14));
            
        }
        [Test]
        public void WipeProjects()
        {
            var projects = ProjectService.GetAllProjects();
            Assert.IsTrue(projects != null);
            foreach (var variableProject in projects)
            {
                Assert.IsTrue(ProjectService.DeleteProject(variableProject.ProjectID));
            }
            Assert.IsTrue(!ProjectService.GetAllProjects().Any());
        }
    }
}