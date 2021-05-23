using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;


namespace SkillNameTest
{
    [TestFixture]
    class UITest
    {
        private IWebDriver _web;

        [Test]
        public void Chrome()
        {
            _web = new ChromeDriver();
            SkillEditTest();
            SkillDeleteTest();

        }

        [Test]

        public void Firefox()
        {
            var options = new FirefoxOptions();
            options.AddAdditionalCapability("acceptSslCerts", true, true);
            options.AddAdditionalCapability("acceptInsecureCerts", true, true);
            _web = new FirefoxDriver(options);
            SkillEditTest();
            SkillDeleteTest();
        }

        public void SkillEditTest()
        {
            _web.Navigate().GoToUrl("https://localhost:5001");
            IWebElement el = _web.FindElement(By.LinkText("Skills"));
            el.Click();
            Assert.AreEqual(_web.Url, "https://localhost:5001/skill");
            el = _web.FindElement(By.Id("bearbeiten"));
            el.Click();
            Assert.AreEqual(_web.Url, "https://localhost:5001/skill/edit/0");
            el = _web.FindElement(By.Id("name"));
            el.SendKeys("test");
            el = _web.FindElement(By.Id("submit"));
            el.Click();
            Assert.AreEqual(_web.Url, "https://localhost:5001/skill");
            el = _web.FindElement(By.Name("test"));


        }

        public void SkillDeleteTest()
        {
            _web.Navigate().GoToUrl("https://localhost:5001");
            IWebElement el = _web.FindElement(By.LinkText("Skills"));
            el.Click();
            Assert.AreEqual(_web.Url, "https://localhost:5001/skill");
            el = _web.FindElement(By.LinkText("Löschen"));
            el.Click();
            Assert.AreEqual(_web.Url, "https://localhost:5001/skill");
           
           
        }

    }
}
