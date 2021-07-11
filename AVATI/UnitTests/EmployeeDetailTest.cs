using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using AVATI.Data;
using AVATI.Data.EmployeeDetailFiles;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace UnitTests
{
    public class EmployeeDetailTest
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
                new EmployeeDetailService(
                    Connection);
            var propService = new ProposalService(Connection);
            var propList = propService.GetAllProposals();
            
            foreach (var prop in propList.Result)
            {
                var result = empService.GetAllEmployeeDetail(prop.ProposalID);
                foreach (var empDetail in result)
                {
                    Assert.IsTrue(empDetail.Discount is >= 0 and <= 100);
                    Assert.IsTrue(empDetail.Fields != null);
                    Assert.IsTrue(empDetail.Hardskills != null);
                    Assert.IsTrue(empDetail.Softskills != null);
                    Assert.IsTrue(empDetail.ProjectActivities != null);
                    Assert.IsTrue(empDetail.EmployeeId != 0);
                    Assert.IsTrue(empDetail.ProposalId != 0);
                
                }
                foreach (var vaEmployee in prop.Employees)
                {
                    Assert.IsTrue(result.Exists(e => e.EmployeeId == vaEmployee.EmployeeID));
                }
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
                new EmployeeDetailService(
                    Connection);
            var propService = new ProposalService(Connection);
            var propList = propService.GetAllProposals();

            foreach (var proposal in propList.Result)
            {
                var empDetail = empService.GetAllEmployeeDetail(proposal.ProposalID);
                foreach (var detail in empDetail)
                {
                    propService.UpdateAltRc(proposal.ProposalID, detail.EmployeeId, rc);
                    Assert.IsTrue(empService.GetEmployeeDetail(detail.EmployeeId, proposal.ProposalID).Result.Rc == rc);
                }
            }
        }
    }
}