using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using Team12.Data;

namespace TestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SkillNameTest1()
        {
            Skill skill = new Skill();
            skill.SkillType = Skill.SkillCategory.Softskill;
            skill.Name = "!!!fehlerhaftebeennenen,sollteeineExceptionthrown";
            Assert.Throws<ValidationException>(delegate
            {
                List<ValidationAttribute> val = new List<ValidationAttribute>();
                val.Add(new SkillNameConventionAttribut());
                Validator.ValidateValue(skill, new ValidationContext(skill), val);
            });
        }

        [Test]
        public void SkillNameTest2()
        {
            Skill skill = new Skill();
            skill.SkillType = Skill.SkillCategory.Softskill;
            skill.Name = "dieserskillNameSollteiegentlichgültigsein";
            Assert.DoesNotThrow(delegate
            {
                List<ValidationAttribute> val = new List<ValidationAttribute>();
                val.Add(new SkillNameConventionAttribut());
                Validator.ValidateValue(skill, new ValidationContext(skill), val);
            });
        }

        [Test]
        public void SkillNameTest3()
        {
            Skill skill = new Skill();
            skill.SkillType = Skill.SkillCategory.Hardskill;
            skill.Name = "fürhardskillsistdiebennenungjaeigentlichnichtrelevant";
            Assert.DoesNotThrow(delegate
            {
                List<ValidationAttribute> val = new List<ValidationAttribute>();
                val.Add(new SkillNameConventionAttribut());
                Validator.ValidateValue(skill, new ValidationContext(skill), val);
            });
        }
    }
}