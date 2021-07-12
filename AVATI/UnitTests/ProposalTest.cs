using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AVATI.Data;
using AVATI.Data.ValidationAttributes;
using Dapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace UnitTests
{
    public class ProposalTest
    {
        public string connection;

        [SetUp]
        public void Setup()
        {
            string json = File.ReadAllText("appsettings.json");
            JObject jObject = JObject.Parse(json);
            var name = (string) jObject["ConnectionStrings"]["TEST-Database"];
            connection =
                name;
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
        public void ProposalTest1(string proposalTitle, string proposalAddInfo, bool isValid)
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

        [TestCaseSource("GetDateTests")]
        public void ProposalTest2(DateTime beginning, DateTime end, bool isValid)
        {
            var proposal = new Proposal() {Start = beginning, End = end, ProposalTitle = "valid"};
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
        public async Task TestAvailableProposals()
        {
            var proposalService =
                new ProposalService(
                    connection);
            var result = proposalService.GetAllProposals();
            var list = await result;
            Assert.IsNotNull(list);
            foreach (var proposal in list)
            {
                Assert.IsTrue(proposal.ProposalTitle != null);
                Assert.IsTrue(proposal.ProposalID != 0);
                Assert.IsTrue(proposal.Employees != null);
                Assert.IsTrue(proposal.Fields != null);
                Assert.IsTrue(proposal.Hardskills != null);
                Assert.IsTrue(proposal.Softskills != null);
                Assert.IsTrue(proposal.AdditionalInfo != null);
                Assert.IsTrue(proposal.AltRc != null);
                Assert.IsTrue(proposal.End >= proposal.Start);
            }

            proposalService.UpdateProposal(0, new Proposal() {ProposalTitle = "A small test"});
            Assert.IsNotNull(proposalService.GetAllProposals().Result
                .Find(e => e.ProposalTitle.Equals("A small test")));
        }

        [TestCase(
            "asdfjaoiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii" +
            "iiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii", false)]
        [TestCase("", false)]
        [TestCase("Thine hollowed heavens", true)]
        [TestCase(
            "CAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAPPPPPPPPPPPPSLOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOCKkkkkkkkk", false)]
        public async Task TestDbManipulation(string proposalTitle, bool isValid)
        {
            var proposalService =
                new ProposalService(
                    connection);
            var result = proposalService.GetAllProposals();
            var list = await result;
            if (list.Count == 0)
            {
                if (isValid)
                {
                    
                    Assert.IsTrue(proposalService.UpdateProposal(0, new Proposal() {ProposalTitle = proposalTitle}));
                }
                else
                {
                    Assert.IsFalse(proposalService.UpdateProposal(0, new Proposal() {ProposalTitle = proposalTitle}));
                }
            }
            else
            {
                list[0].ProposalTitle = proposalTitle;
                if (isValid)
                {
                    Assert.IsTrue(proposalService.UpdateProposal(list[0].ProposalID, list[0]));
                    Assert.IsTrue(proposalService.GetProposal(list[0].ProposalID) != null && proposalService.GetProposal(list[0].ProposalID).ProposalTitle == proposalTitle);
                }
                else
                {
                    Assert.IsFalse(proposalService.UpdateProposal(list[0].ProposalID, list[0]));
                    Assert.IsTrue(proposalService.GetProposal(list[0].ProposalID) != null && proposalService.GetProposal(list[0].ProposalID).ProposalTitle != proposalTitle);
                }
            }
            
        }
    }
}