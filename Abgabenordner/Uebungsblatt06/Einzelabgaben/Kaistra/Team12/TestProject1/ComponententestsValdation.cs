using NUnit.Framework;

namespace Team12.Data
{
    public class ComponententestsValidation
    {
        private Skill _skill;

        [Test]
        public void UnitTest1ReturnsTrueHARDSKILLALWAYSTRUE()
        {
            _skill = new Skill();
            _skill.Name = "Hardskillbennennung";
            _skill.type = Skilltype.Hardskill;

            var attrib = new SkillNameConventionAttribut();
            var result = attrib.IsValid(_skill);
            Assert.That(result, Is.True);
        }

        [Test]
        public void UnitTest1ReturnsTrue2SOFTSKILLFALSE()
        {
            _skill = new Skill();
            _skill.Name = "SOFTSKILL!§)&$(§/";
            _skill.type = Skilltype.Hardskill;

            var attrib = new SkillNameConventionAttribut();
            var result = attrib.IsValid(_skill);
            Assert.That(result, Is.True);
        }

        [Test]
        public void UnitTest1ReturnsTrue3FALLSOFTSKILLTRUE()
        {
            _skill = new Skill();
            _skill.Name = "teamkompetent";
            _skill.type = Skilltype.Softskill;

            var attrib = new SkillNameConventionAttribut();
            var result = attrib.IsValid(_skill);
            Assert.That(result, Is.True);
        }
    }
}