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

namespace UnitTests
{
    public class ProjectTest
    {
        public string connection;
        public ProjectService ProjectService;
        
        [SetUp]
        public void Setup()
        {
            string json = File.ReadAllText("appsettings.json");
            JObject jObject = JObject.Parse(json);
            var name = (string) jObject["ConnectionStrings"]["TEST-Database"];
            connection = name;
            ProjectService = new ProjectService(connection);
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
                Projecttitel = " asd", Projectbeginning = DateTime.Now,
                Projectend = DateTime.Now, Employees = new List<Employee>(),
                Projectdescription = "sad", Fields = new List<string>(),
                 ProjectActivities = new List<string>(),
            }));
            testCaseData.Add(new TestCaseData(new Project()
            {
                Projecttitel = "Just a small test no biggie", Projectbeginning = DateTime.Now,
                Projectend = DateTime.Now, Employees = new List<Employee>(),
                Projectdescription = "Just a small Project nothing more", Fields = new List<string>(),
                 ProjectActivities = new List<string>(),
            }));
            return testCaseData.AsEnumerable();
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
            Assert.IsTrue(ProjectService.UpdateProject(result));
        }
    }
}