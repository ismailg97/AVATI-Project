using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using Team12.Data;

namespace TestProject1
{
    public class UnitTest1
    {
        private Skill _skill;
        private SkillNameConventionAttribut conAttr;
        [SetUp]
        public void Setup()
        {
            _skill = new Skill() {Name = "test_name", Id = 1, SkillType = Skill.Category.Softskill};
            conAttr = new SkillNameConventionAttribut();
        }
        [TestCase("IwasmitLeerzei chen")]
        [TestCase("nört")]
        public void TestSoftskill(string name)
        {
            _skill.Name = name;
            var context = new ValidationContext(_skill);
            var value = conAttr.GetValidationResult(name, context);
            Assert.AreEqual(ValidationResult.Success, value, name + "ist ein korrekter Name");
            

        }
        [TestCase("!!x-x-x!!")]
        [TestCase("1 falscher Bezeichner???")]
        public void TestSkillInvalid(string name)
        {
            _skill.Name = name;
            var context = new ValidationContext(_skill);
            var value = conAttr.GetValidationResult(name, context);
            Assert.AreNotEqual(ValidationResult.Success, value, name + " ist kein korrekter Name für einen Softskill");
        }
    }
}