using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Team12.Data;


namespace SkillNameTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SkillNameTest2()
        {
            Skill skill = new Skill();
            skill.Name = "gültig";
            skill.SkillType = Skill.Category.Softskill;

            Assert.DoesNotThrow(delegate
            {
                List<ValidationAttribute> validation = new List<ValidationAttribute>();
                validation.Add(new SkillNameConventionAttributAttribute());
                Validator.ValidateValue(skill, new ValidationContext(skill), validation);
            });
        }


        [Test]
        public void SkillNameTest1()
        {
            Skill skill = new Skill();
            skill.Name = "?fehler";
            skill.SkillType = Skill.Category.Softskill;
            
            Assert.Throws<ValidationException>(delegate
            {
                List<ValidationAttribute> validation = new List<ValidationAttribute>();
                validation.Add(new SkillNameConventionAttributAttribute());
                Validator.ValidateValue(skill, new ValidationContext(skill), validation);
            });
        }

    }
}
