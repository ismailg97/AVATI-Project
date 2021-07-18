using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AVATI.Data;
using NUnit.Framework;

namespace UnitTests
{
    public class BasicDataServiceTest
    {

        [TestCase("asdasddsasadadsadasdddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd" +
                  "asdddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddsdasasdddddddddddddddddddd" +
                  "dddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddsssssssssssssdfsfdsdsfds", false)]
        [TestCase("sssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss" +
                  "ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss", true)]
        [TestCase("Fdsds", true)]
        [TestCase("h", true)]
        public void TestCheckLength(string description, bool shouldPass)
        {
            var testBasicDataService = new BasicDataService();
            Assert.IsTrue(testBasicDataService.CheckLengthBasicData(description) == shouldPass);
        }

        [TestCase("", false)]
        [TestCase("          ", false)]
        [TestCase("    ", false)]
        [TestCase("Fdsds", true)]
        [TestCase("    Fdsds", true)]
        [TestCase("Fdsds      ", true)]
        public void TestCheckEmpty(string description, bool shouldPass)
        {
            var testBasicDataService = new BasicDataService();
            Assert.IsTrue(testBasicDataService.CheckEmptyBasicData(description) == shouldPass);
        }

        [TestCase("Interdisziplinärer Sachverstand", false)]
        [TestCase("iN Terdisziplinärer  Sachverstand  ", false)]
        [TestCase("   Soziale  Kompetenz ", false)]
        [TestCase(" Sozial  Kompetenz ", true)]
        [TestCase("Mitarbeiterförderung", false)]
        [TestCase("mitarbeiterförderung ", false)]
        [TestCase("Mitarbeiterforderung ", true)]
        [TestCase("Mitarbeiter-förderung", true)]
        [TestCase("Mitarbeiter-förderung", true)]
        public void TestCheckDesSoftskill(string description, bool shouldPass)
        {
            var testBasicDataService = new BasicDataService();
            Assert.IsTrue(testBasicDataService.CheckDescriptionSoftskill(description) == shouldPass);
        }

        [TestCase("Softskill", "Softskill")]
        [TestCase("Soft Skilling", "Soft Skilling 2")]
        [TestCase("Motivation", "Motivierend")]
        [TestCase("The real Softskill", "Not real Softskill")]
        [TestCase("232vfdng#+fg*", "~}][{fdfds")]
        public void TestCudSoftskill(string oldDescription, string newDescription)
        {
            var testBasicDataService = new BasicDataService();
            Assert.IsTrue(testBasicDataService.CreateSoftSkill(oldDescription));
            Assert.IsFalse(testBasicDataService.CheckDescriptionSoftskill(oldDescription));
            Assert.IsTrue(testBasicDataService.UpdateSoftSkill(newDescription, oldDescription));
            if (oldDescription != newDescription)
                Assert.IsTrue(testBasicDataService.CheckDescriptionSoftskill(oldDescription));
            Assert.IsFalse(testBasicDataService.CheckDescriptionSoftskill(newDescription));
            Assert.IsTrue(testBasicDataService.DeleteSoftSkill(newDescription));
            Assert.IsTrue(testBasicDataService.CheckDescriptionSoftskill(newDescription));
        }
        
        [TestCase("Bildung", false)]
        [TestCase("  bilD ung", false)]
        [TestCase("Bild-ung", true)]
        [TestCase("Gesundheit/Soziales/Pflege", false)]
        [TestCase("Gesundheit / Soziales  /Pflege ", false)]
        [TestCase("Gesundheit Soziales Pflege", true)]
        [TestCase("Gesundheit//Soziales//Pflege", true)]
        [TestCase("Maschinen- und Anlagenbau", false)]
        [TestCase("Maschinen und Anlagenbau", true)]
        public void TestCheckDesField(string description, bool shouldPass)
        {
            var testBasicDataService = new BasicDataService();
            Assert.IsTrue(testBasicDataService.CheckDescriptionField(description) == shouldPass);
        }

        [TestCase("Field", "Field")]
        [TestCase("Fielding", "Fielding 2")]
        [TestCase("Motivation", "Motivierend")]
        [TestCase("The real Field", "Not real Field")]
        [TestCase("232vfdng#+fg*", "~}][{fdfds")]
        public void TestCudField(string oldDescription, string newDescription)
        {
            var testBasicDataService = new BasicDataService();
            Assert.IsTrue(testBasicDataService.CreateField(oldDescription));
            Assert.IsFalse(testBasicDataService.CheckDescriptionField(oldDescription));
            Assert.IsTrue(testBasicDataService.UpdateField(newDescription, oldDescription));
            if (oldDescription != newDescription)
                Assert.IsTrue(testBasicDataService.CheckDescriptionField(oldDescription));
            Assert.IsFalse(testBasicDataService.CheckDescriptionField(newDescription));
            Assert.IsTrue(testBasicDataService.DeleteField(newDescription));
            Assert.IsTrue(testBasicDataService.CheckDescriptionField(newDescription));
        }
        
        [TestCase("Software Developer", false)]
        [TestCase("Agile Coach", false)]
        [TestCase("UI/UX-Designer", false)]
        [TestCase("  UI/UX  -  DesI gner  ", false)]
        [TestCase("UI//UX-Designer", true)]
        [TestCase("Product Owner", false)]
        [TestCase("prOductOwn er  ", false)]
        [TestCase("Product-Owner", true)]
        public void TestCheckDesRole(string description, bool shouldPass)
        {
            var testBasicDataService = new BasicDataService();
            Assert.IsTrue(testBasicDataService.CheckDescriptionRole(description) == shouldPass);
        }

        [TestCase("Role", "Role")]
        [TestCase("Roling", "Roling 2")]
        [TestCase("Motivation", "Motivierend")]
        [TestCase("The real Role", "Not real Role")]
        [TestCase("232vfdng#+fg*", "~}][{fdfds")]
        public void TestCudRole(string oldDescription, string newDescription)
        {
            var testBasicDataService = new BasicDataService();
            Assert.IsTrue(testBasicDataService.CreateRole(oldDescription));
            Assert.IsFalse(testBasicDataService.CheckDescriptionRole(oldDescription));
            Assert.IsTrue(testBasicDataService.UpdateRole(newDescription, oldDescription));
            if (oldDescription != newDescription)
                Assert.IsTrue(testBasicDataService.CheckDescriptionRole(oldDescription));
            Assert.IsFalse(testBasicDataService.CheckDescriptionRole(newDescription));
            Assert.IsTrue(testBasicDataService.DeleteRole(newDescription));
            Assert.IsTrue(testBasicDataService.CheckDescriptionRole(newDescription));
        }
    }
}