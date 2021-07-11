using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AVATI.Data;
using AVATI.Data.ValidationAttributes;
using NUnit.Framework;
namespace UnitTests
{
    public class Tests
    {
        public void Setup()
        {
            var proposalList = new List<Proposal>();
            proposalList.Add(new Proposal()
            {
                ProposalTitle = "ThisShouldBeFine:)", AdditionalInfo = "Nothingspecail",
                Start = DateTime.Now.AddDays(-12), End = DateTime.Now.AddDays(12)
            });
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
            "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", ":)", false)]
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

        
        
    }
}