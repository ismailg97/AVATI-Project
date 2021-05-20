using NUnit.Framework;
using Team12.Data;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace TestProject
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("Teamf‰higkeit", true)]
        [TestCase("Softskill!!", false)]
        [TestCase("Team f‰higkeit", true)]
        [TestCase("Spaﬂ + Fun", false)]
        public void SoftskillTest(string name, bool valid)
        {
            var softskill = new Skill()
            {
                Id = 1,
                Name = name,
                Skilltype = Skilltype.Softskill
            };

            List<ValidationAttribute> valatt = new List<ValidationAttribute>();
            valatt.Add(new SkillNameConventionAttribut());
            var ctx = new ValidationContext(softskill);

            if (!valid) Assert.Throws<ValidationException>(delegate {
                Validator.ValidateValue(softskill, ctx, valatt);
            });
            else Assert.DoesNotThrow(delegate {
                Validator.ValidateValue(softskill, ctx, valatt);
            });
        }
    }
}