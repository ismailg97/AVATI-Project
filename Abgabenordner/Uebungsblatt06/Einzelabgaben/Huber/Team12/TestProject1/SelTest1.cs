using System.IO;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace TestProject1
{
    [TestFixture]
    public class SelTest1
    {
        private IWebDriver _webDriver;

        [Test]
        public void ChromeTest1()
        {
            //Driver for chrome
            _webDriver = new ChromeDriver();
            TestSkillCreation(); //Test if Creating of a Skills Works properly
            //TestGenerateDokument();  //Tests whether Document gets created and downloaded

        }

        [Test]

        public void FirefoxTest1()
        {
            //fixes the "unsafe certificate" issue
            var options = new FirefoxOptions();
            options.AddAdditionalCapability("acceptSslCerts", true, true);
            options.AddAdditionalCapability("acceptInsecureCerts", true, true);
            //Driver for firefox
            _webDriver = new FirefoxDriver(options);
            Thread.Sleep(1000);
            TestSkillCreation();
            TestGenerateDokument();
        }

        public void TestSkillCreation()
        {
            _webDriver.Navigate().GoToUrl("https://localhost:5001");
            Thread.Sleep(3000);
            IWebElement btn = _webDriver.FindElement(By.LinkText("Skills"));
            btn.Click();
            Assert.AreEqual(_webDriver.Url, "https://localhost:5001/skill");
            Thread.Sleep(3000);
            btn = _webDriver.FindElement(By.Id("addButton"));
            btn.Click();
            Assert.AreEqual(_webDriver.Url, "https://localhost:5001/skill/edit");
            Thread.Sleep(3000);
            btn = _webDriver.FindElement(By.Id("skillName"));
            btn.SendKeys("testSkill123");
            Thread.Sleep(3000);
            btn = _webDriver.FindElement(By.Id("submit"));
            btn.Click();
            Thread.Sleep(3000);
            Assert.AreEqual(_webDriver.Url, "https://localhost:5001/Skill");
            Thread.Sleep(1000);
            btn = _webDriver.FindElement(By.Name("testSkill123"));
            

        }

        public void TestGenerateDokument()
        {
            File.Delete(@"C:\Users\anton\Downloads\test.docx");
            _webDriver.Navigate().GoToUrl("https://localhost:5001");
            Thread.Sleep(3000);
            IWebElement dl = _webDriver.FindElement(By.Id("generateDocument"));
            dl.Click();
            Thread.Sleep(3000);
            Assert.IsTrue(File.Exists(@"C:\Users\anton\Downloads\test.docx"));
        }
    }
}