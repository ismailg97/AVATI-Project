using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using Team12.Data;

namespace tests
{
    public class ComponententestsValidation
    {
        private Skill _skill;
        
        [Test]
        public void UnitTestFalseInput()
        {
            _skill = new Skill();
            _skill.Name = "SOFTSKILL!§)&$(§/";
            _skill.type = Skilltype.Hardskill;

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(_skill, new ValidationContext(_skill), validationResults, true);
            
            Assert.IsTrue(actual, "Erwartete Anzahl");
            Assert.AreEqual(0, validationResults.Count, "Fehlerhafte Anzahl! Validierungsfehler.");
        }
        [Test]
        public void UnitTestTrueInput()
        {
            _skill = new Skill();
            _skill.Name = "Teamfähigkeit";
            _skill.type = Skilltype.Hardskill;

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(_skill, new ValidationContext(_skill), validationResults, true);
            
            Assert.IsTrue(actual, "Erwartete Anzahl");
            Assert.AreEqual(0, validationResults.Count, "Fehlerhafte Anzahl! Validierungsfehler.");
        }
        

        
    }
}