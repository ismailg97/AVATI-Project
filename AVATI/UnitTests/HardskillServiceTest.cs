using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AVATI.Data;
using NUnit.Framework;

namespace UnitTests
{
    public class ServiceForDatasTest
    {

        public static IEnumerable<TestCaseData> GetDataGetHardskill()
        {
            var testCaseData = new List<TestCaseData>();
            testCaseData.Add(new TestCaseData("C", new List<string>(){"Sprachen"}, true));
            testCaseData.Add(new TestCaseData("SAP Cloud Platform", new List<string>(){"Cloud", "Plattformen"}, true));
            testCaseData.Add(new TestCaseData("Objektorientierte Entwicklung", new List<string>(){"Software Engineering"}, true));
            testCaseData.Add(new TestCaseData("Keras", new List<string>(){"Bibliotheken"}, true));
            testCaseData.Add(new TestCaseData("Sketch", new List<string>(){"Digitales Design"}, true));
            testCaseData.Add(new TestCaseData("Apache Tomcat", new List<string>(){"Server- und Infrastruktur-Management"}, true));
            testCaseData.Add(new TestCaseData("Sprachen und Frameworks", null, false));
            testCaseData.Add(new TestCaseData("Agile Scaling", null, false));
            testCaseData.Add(new TestCaseData("Betriebssysteme", null, false));
            testCaseData.Add(new TestCaseData("wejiro3290fjlsfopsü", null, false));
            return testCaseData.AsEnumerable();
        }
        
        public static IEnumerable<TestCaseData> GetDataGetCategory()
        {
            var testCaseData = new List<TestCaseData>();
            testCaseData.Add(new TestCaseData("Sprachen und Frameworks", null, new List<string>(){ "Sprachen", "Frameworks", "Bibliotheken"}, true));
            testCaseData.Add(new TestCaseData("Konzept- und Ideenmanagement", "Tools", new List<string>(){ "Draw.io", "MetroRetro", "FreeMind", "Miro"}, true));
            testCaseData.Add(new TestCaseData("Agile Scaling", "Methoden und Prozesse", new List<string>(){ "Scaled Agile Framework (SAFe)", 
                "Disciplined Agile Delivery (DaD)", "Large Scale SCRUM (LeSS)"}, true));
            testCaseData.Add(new TestCaseData("Betriebssysteme/Cloud/Plattformen/Hardware", null, new List<string>() { "Betriebssysteme",
                 "Cloud", "Hardware", "Plattformen"}, true));
            testCaseData.Add(new TestCaseData("C", null, null, false));
            testCaseData.Add(new TestCaseData("SAP Cloud Platform", null, null, false));
            testCaseData.Add(new TestCaseData("Objektorientierte Entwicklung", null, null, false));
            testCaseData.Add(new TestCaseData("Keras", null, null, false));
            testCaseData.Add(new TestCaseData("wejiro3290fjlsfopsü", null, null, false));
            return testCaseData.AsEnumerable();
        }

        public static IEnumerable<TestCaseData> GetDataCudCategory()
        {
            var testCaseData = new List<TestCaseData>();
            testCaseData.Add(new TestCaseData(
                new List<string>(){"1NewHardskill", "2NewHardskill", "3NewHardskill", "4NewHardskill"},
                new Hardskill()
                {
                    Description = "NewCategory1", 
                    Uppercat= new List<string>(){"Expertise"},
                    Subcat = new List<string>(){"1NewHardskill", "2NewHardskill"},
                    IsHardskill = false
                }, 
                new Hardskill()
                {
                    Description = "NewCategory2", 
                    Uppercat= new List<string>(){"Expertise"},
                    Subcat = new List<string>(){"2NewHardskill", "3NewHardskill", "4NewHardskill"},
                    IsHardskill = false
                }));
            testCaseData.Add(new TestCaseData(
                new List<string>(){"NewSkilling1", "NewSkilling2"},
                new Hardskill()
                {
                    Description = "NewCategory3", 
                    Uppercat= new List<string>(){"Methoden und Prozesse"},
                    Subcat = new List<string>(){"Agile Methoden", "NewSkilling1", "NewSkilling2"},
                    IsHardskill = false
                }, 
                new Hardskill()
                {
                    Description = "NewCategory4", 
                    Uppercat= new List<string>(),
                    Subcat = new List<string>(),
                    IsHardskill = false
                }));
            testCaseData.Add(new TestCaseData(
                new List<string>(){"SkillNew1", "SkillNew2", "SkillNew3"},
                new Hardskill()
                {
                    Description = "NewCategory5", 
                    Uppercat= new List<string>(),
                    Subcat = new List<string>(),
                    IsHardskill = false
                }, 
                new Hardskill()
                {
                    Description = "NewCategory5", 
                    Uppercat= new List<string>(),
                    Subcat = new List<string>() {"SkillNew1", "SkillNew2", "SkillNew3"},
                    IsHardskill = false
                }));
            return testCaseData.AsEnumerable();
        }

        public static IEnumerable<TestCaseData> GetDataCudHardskill()
        {
            var testCaseData = new List<TestCaseData>();
            testCaseData.Add(new TestCaseData(
                new Hardskill()
                {
                    Description = "NewHardskill1", 
                    Uppercat= new List<string>(){"Expertise", "Schnittstellen und Protokolle", "Frameworks"},
                    Subcat = null,
                    IsHardskill = true
                }, 
                new Hardskill()
                {
                    Description = "NewHardskill2", 
                    Uppercat= new List<string>(){"Cloud"},
                    Subcat = null,
                    IsHardskill = true
                }));
            
            testCaseData.Add(new TestCaseData(
                new Hardskill()
                {
                    Description = "HardskillNew", 
                    Uppercat= new List<string>(){"Optimierung/technische Mathematik", "Tools"},
                    Subcat = null,
                    IsHardskill = true
                }, 
                new Hardskill()
                {
                    Description = "HardskillNew", 
                    Uppercat= new List<string>(){"Optimierung/technische Mathematik", "Projektmanagement Tools", "Datenbanken"},
                    Subcat = null,
                    IsHardskill = true
                }));
            
            testCaseData.Add(new TestCaseData(
                new Hardskill()
                {
                    Description = "NewOne", 
                    Uppercat= new List<string>(){"Frameworks"},
                    Subcat = null,
                    IsHardskill = true
                }, 
                new Hardskill()
                {
                    Description = "NewTwo", 
                    Uppercat= new List<string>(){"Frameworks"},
                    Subcat = null,
                    IsHardskill = true
                }));
            return testCaseData.AsEnumerable();
        }

        [TestCaseSource("GetDataGetHardskill")]
        public async Task TestGetHardskill(string skill, List<string> upperCat, bool exists)
        {
            var testHardskillService = new HardskillService(true);
            var hardskill = await testHardskillService.GetHardskill(skill);
            if(exists)
                Assert.IsNotNull(hardskill);
            else
            {
                Assert.IsNull(hardskill);
                return;
            }
            Assert.IsTrue(hardskill.Uppercat.All(upperCat.Contains));
            Assert.IsTrue(hardskill.IsHardskill);
            Assert.IsTrue(hardskill.Subcat == null);
        }

        [TestCaseSource("GetDataGetCategory")]
        public async Task TestGetCategory(string cat, string upperCat, List<string> subCat, bool exists)
        {
            var testHardskillService = new HardskillService(true);
            var category = await testHardskillService.GetHardskillCategory(cat);
            if(exists)
                Assert.IsNotNull(category);
            else
            {
                Assert.IsNull(category);
                return;
            }
            if(upperCat == null)
                Assert.IsFalse(category.Uppercat.Any());
            else
                Assert.IsTrue(category.Uppercat.Count == 1 && category.Uppercat[0] == upperCat);
            Assert.IsFalse(category.IsHardskill);
            Assert.IsTrue(category.Subcat.All(subCat.Contains));
        }
        
        [TestCase("Konzept- und Ideenmanagement", true)]
        [TestCase("TestCategory", true)]
        [TestCase("Betriebssysteme", true)]
        [TestCase("Expertise", true)]
        [TestCase("Sprachen und Frameworks", false)]
        [TestCase("Methoden und Prozesse", false)]
        [TestCase("C", false)]
        [TestCase("Objektorientierte Entwicklung", false)]
        [TestCase("dsfdsfdsssssssssssdfds", false)]
        public async Task TestContainsAnyHardskills(string category, bool contains)
        {
            var testHardskillService = new HardskillService(true);
            Assert.IsTrue(await testHardskillService.ContainsAnyHardskills(category) == contains);
        }

        [TestCase("Konzept- und Ideenmanagement", true)]
        [TestCase("Betriebssysteme", true)]
        [TestCase("Expertise", true)]
        [TestCase("TestCategory", false)]
        [TestCase("Sprachen und Frameworks", false)]
        [TestCase("Methoden und Prozesse", false)]
        [TestCase("C", false)]
        [TestCase("Objektorientierte Entwicklung", false)]
        [TestCase("dsfdsfdsssssssssssdfds", false)]
        public async Task TestContainsJustHardskills(string category, bool contains)
        {
            var testHardskillService = new HardskillService(true);
            Assert.IsTrue(await testHardskillService.ContainsJustHardskills(category) == contains);
        }

        [TestCase("", false)]
        [TestCase("          ", false)]
        [TestCase("    ", false)]
        [TestCase("Fdsds", true)]
        [TestCase("    Fdsds", true)]
        [TestCase("Fdsds      ", true)]
        public void TestCheckIsEmptyHardskill(string description, bool shouldPass)
        {
            var testHardskillService = new HardskillService(true);
            Assert.IsTrue(testHardskillService.CheckEmptyHardskill(description) == shouldPass);
        }

        [TestCase("asdasddsasadadsadasdddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd" +
                  "asdddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddsdasasdddddddddddddddddddd" +
                  "dddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddsssssssssssssdfsfdsdsfds", false)]
        [TestCase("sssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss" +
                  "ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss", true)]
        [TestCase("Fdsds", true)]
        public void TestCheckIsTooLongHardskill(string description, bool shouldPass)
        {
            var testHardskillService = new HardskillService(true);
            Assert.IsTrue(testHardskillService.CheckLengthHardskill(description) == shouldPass);
        }

        [TestCase("SonarQube", false)]
        [TestCase("  sonar  qube ", false)]
        [TestCase("Jira", false)]
        [TestCase("jira", false)]
        [TestCase("Systemisches Projektmanagement", false)]
        [TestCase("  SyStemisches    prOjektManagement  ", false)]
        [TestCase("CloudFoundrY  ", false)]
        [TestCase("Bash", false)]
        [TestCase("Fdsdssddsd", true)]
        [TestCase("fjksdfjks", true)]
        [TestCase("NewHardskill", true)]
        public async Task TestCheckExistHardskill(string description, bool shouldPass)
        {
            var testHardskillService = new HardskillService(true);
            Assert.IsTrue(await testHardskillService.CheckExistHardskill(description) == shouldPass);
        }

        [TestCaseSource("GetDataCudHardskill")]
        public async Task TestCudHardskill(Hardskill oldHardskill, Hardskill newHardskill)
        {
            var testHardskillService = new HardskillService(true);
            
            //----Testing Create----
            Assert.IsTrue(await testHardskillService.CreateHardskill(oldHardskill));
            var created = await testHardskillService.GetHardskill(oldHardskill.Description);
            Assert.IsNotNull(created);
            
            //Testing if relations to UpperCategorys are set too
            if (oldHardskill.Uppercat != null)
            {
                foreach (var category in oldHardskill.Uppercat)
                {
                    Assert.IsTrue((await testHardskillService.GetHardskillCategory(category)).Subcat.Contains(oldHardskill.Description));
                    Assert.IsTrue(created.Uppercat.Contains(category));
                }
            }
            else
                Assert.IsTrue(created.Uppercat.Count == 0);
            
            //----Testing Update----
            Assert.IsTrue(await testHardskillService.UpdateHardskill(newHardskill, oldHardskill));
            var updated = await testHardskillService.GetHardskill(newHardskill.Description);
            Assert.IsNotNull(updated);
            
            //Testing if UpperCategorys are updated too
            if (newHardskill.Uppercat != null)
            {
                foreach (var category in newHardskill.Uppercat)
                {
                    Assert.IsTrue((await testHardskillService.GetHardskillCategory(category)).Subcat.Contains(newHardskill.Description));
                    Assert.IsTrue(updated.Uppercat.Contains(category));
                }
            }
            else
                Assert.IsTrue(updated.Uppercat.Count == 0);

            if (oldHardskill.Description != newHardskill.Description)
            {
                Assert.IsNull(await testHardskillService.GetHardskill(oldHardskill.Description));
                if (oldHardskill.Uppercat != null)
                {
                    foreach (var category in oldHardskill.Uppercat)
                    {
                        Assert.IsFalse((await testHardskillService.GetHardskillCategory(category)).Subcat.Contains(oldHardskill.Description));
                    }
                }
            }
            else
            {
                if (oldHardskill.Uppercat != null && newHardskill.Uppercat != null)
                {
                    foreach (var category in oldHardskill.Uppercat.Where(x => !newHardskill.Uppercat.Contains(x)))
                    {
                        Assert.IsFalse((await testHardskillService.GetHardskillCategory(category)).Subcat.Contains(oldHardskill.Description));
                    }
                }
            }

            //----Testing Delete----
            Assert.IsTrue(await testHardskillService.DeleteHardskill(newHardskill.Description));
            Assert.IsNull(await testHardskillService.GetHardskill(newHardskill.Description));
            
            //Testing if relations to UpperCategorys are deleted too
            if (newHardskill.Uppercat != null)
            {
                foreach (var category in newHardskill.Uppercat)
                {
                    Assert.IsFalse((await testHardskillService.GetHardskillCategory(category)).Subcat.Contains(oldHardskill.Description));
                }
            }
        }

        [TestCaseSource("GetDataCudCategory")]
        public async Task TestCudCategory(List<string> hardskills, Hardskill oldCategory, Hardskill newCategory)
        {
            var testHardskillService = new HardskillService(true);
            
            //Creating Pseudo-Hardskills
            foreach (var hardskill in hardskills)
            {
                await testHardskillService.CreateHardskill(new Hardskill()
                {
                    Description = hardskill, 
                    Uppercat = null, 
                    Subcat = null, 
                    IsHardskill = true
                });
            }

            //----Testing Create----
            Assert.IsTrue(await testHardskillService.CreateHardskillCategory(oldCategory));
            var created = await testHardskillService.GetHardskillCategory(oldCategory.Description);
            Assert.IsNotNull(created);
            
            //Testing if relations to UpperCategory is set too
            if (oldCategory.Uppercat != null && oldCategory.Uppercat.Any())
            {
                var category = await testHardskillService.GetHardskillCategory(oldCategory.Uppercat[0]);
                Assert.IsTrue(category.Subcat.Contains(oldCategory.Description));
                Assert.IsTrue(created.Uppercat.Count == 1 && created.Uppercat[0] == category.Description);
            }
            else
                Assert.IsTrue(created.Uppercat.Count == 0);
            

            //Testing if relations to SubCategorys are set too
            foreach (var subCat in oldCategory.Subcat)
            {
                var skillOrCat = await testHardskillService.GetHardskillOrCategory(subCat);
                Assert.IsTrue(skillOrCat.Uppercat.Contains(oldCategory.Description));
                Assert.IsTrue(created.Subcat.Contains(subCat));
            }
            
            //----Testing Rename----
            Assert.IsTrue(await testHardskillService.RenameHardskillCategory(oldCategory.Description, newCategory.Description));
            var renamed = await testHardskillService.GetHardskillCategory(newCategory.Description);
            Assert.IsNotNull(renamed);

            if (oldCategory.Description != newCategory.Description)
            {
                Assert.IsNull(await testHardskillService.GetHardskillCategory(oldCategory.Description));
                if (oldCategory.Uppercat != null && oldCategory.Uppercat.Any())
                {
                    var upperCat = await testHardskillService.GetHardskillCategory(oldCategory.Uppercat[0]);
                    Assert.IsTrue(renamed.Uppercat.Count == 1 && renamed.Uppercat[0] == upperCat.Description);
                    Assert.IsTrue(upperCat.Subcat.Contains(newCategory.Description));
                    Assert.IsFalse(upperCat.Subcat.Contains(oldCategory.Description));
                }
                
                foreach (var subCat in oldCategory.Subcat)
                {
                    var skillOrCat = await testHardskillService.GetHardskillOrCategory(subCat);
                    Assert.IsTrue(skillOrCat.Uppercat.Contains(newCategory.Description));
                    Assert.IsFalse(skillOrCat.Uppercat.Contains(oldCategory.Description));
                    Assert.IsTrue(renamed.Subcat.Contains(subCat));
                }
            }
               
            
            //---Testing EditHardskills----
            Assert.IsTrue(await testHardskillService.EditHardskillsCategory(newCategory.Description, newCategory.Subcat));
            var editHardskills = await testHardskillService.GetHardskillCategory(newCategory.Description);
            Assert.IsNotNull(editHardskills);
            
            //Testing if relations to SubCategorys are set too
            foreach (var subCat in newCategory.Subcat)
            {
                var skillOrCat = await testHardskillService.GetHardskillOrCategory(subCat);
                if (!skillOrCat.IsHardskill && !oldCategory.Subcat.Contains(subCat))
                {
                    Assert.IsFalse(skillOrCat.Uppercat.Contains(newCategory.Description));
                    Assert.IsFalse(editHardskills.Subcat.Contains(subCat));
                    continue;
                }
                Assert.IsTrue(skillOrCat.Uppercat.Contains(newCategory.Description));
                Assert.IsTrue(editHardskills.Subcat.Contains(subCat));
            }

            foreach (var subCat in oldCategory.Subcat)
            {
                var skillOrCat = await testHardskillService.GetHardskillOrCategory(subCat);
                if (skillOrCat.IsHardskill && newCategory.Subcat.Contains(subCat) == false)
                {
                    Console.WriteLine(skillOrCat);
                    Assert.IsFalse(skillOrCat.Uppercat.Contains(newCategory.Description));
                    Assert.IsFalse(editHardskills.Subcat.Contains(subCat));
                    continue;
                }
                Assert.IsTrue(skillOrCat.Uppercat.Contains(newCategory.Description));
                Assert.IsTrue(editHardskills.Subcat.Contains(subCat));
            }
            
            //----Testing Delete----
            Assert.IsTrue(await testHardskillService.DeleteHardskillCategory(newCategory.Description));
            Assert.IsNull(await testHardskillService.GetHardskillCategory(newCategory.Description));
            
            foreach (var subCat in newCategory.Subcat)
            {
                var hardskill = await testHardskillService.GetHardskill(subCat);
                if (hardskill == null) continue;
                Assert.IsFalse(hardskill.Uppercat.Contains(newCategory.Description));
                if (oldCategory.Uppercat != null && oldCategory.Uppercat.Any())
                {
                    var upperCat = await testHardskillService.GetHardskillCategory(oldCategory.Uppercat[0]);
                    Assert.IsTrue(hardskill.Uppercat.Contains(oldCategory.Uppercat[0]));
                    Assert.IsTrue(upperCat.Subcat.Contains(subCat));
                    Assert.IsFalse(upperCat.Subcat.Contains(newCategory.Description));
                }
            }

            foreach (var subCat in oldCategory.Subcat)
            {
                var category = await testHardskillService.GetHardskillCategory(subCat);
                if (category == null) continue;
                Assert.IsFalse(category.Uppercat.Contains(newCategory.Description));
                if (oldCategory.Uppercat != null && oldCategory.Uppercat.Any())
                {
                    var upperCat = await testHardskillService.GetHardskillCategory(oldCategory.Uppercat[0]);
                    Assert.IsTrue(category.Uppercat.Contains(oldCategory.Uppercat[0]) && category.Uppercat.Count == 1);
                    Assert.IsTrue(upperCat.Subcat.Contains(subCat));
                    Assert.IsFalse(upperCat.Subcat.Contains(newCategory.Description));
                }
            }
            
            //Deleting Pseudo-Hardskills
            foreach (var hardskill in hardskills)
            {
                await testHardskillService.DeleteHardskill(hardskill);
            }
        }
    }
}