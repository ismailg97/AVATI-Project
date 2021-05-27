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
    class UiTest
    {
        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        [Obsolete]
        public void SkillEditTest()
        {
           var options = new FirefoxOptions();
            options.AddAdditionalCapability("acceptSslCerts", true, true);
            options.AddAdditionalCapability("acceptInsecureCerts", true, true);
            IWebDriver _web = new FirefoxDriver(options);
            _web.Navigate().GoToUrl("https://localhost:5001");
            Thread.Sleep(1000);
            IWebElement el = _web.FindElement(By.LinkText("Skills"));
            el.Click();
            Thread.Sleep(1000);
            
            el = _web.FindElement(By.Id("bearbeiten"));
            el.Click();
            Thread.Sleep(1000);
            
            el = _web.FindElement(By.Id("name"));
            el.SendKeys("test");
            Thread.Sleep(1000);
            el = _web.FindElement(By.Id("submit"));
            el.Click();
            Thread.Sleep(1000);
            
            el = _web.FindElement(By.Name("test"));



        }

        [Test]
        [Obsolete]
        public void SkillDeleteTest()
        {
            var options = new FirefoxOptions();
            options.AddAdditionalCapability("acceptSslCerts", true, true);
            options.AddAdditionalCapability("acceptInsecureCerts", true, true);
            IWebDriver _web = new FirefoxDriver(options);
            _web.Navigate().GoToUrl("https://localhost:5001");
            Thread.Sleep(1000);
            IWebElement el = _web.FindElement(By.LinkText("Skills"));
            el.Click();
            Thread.Sleep(1000);
            
            el = _web.FindElement(By.LinkText("Löschen"));
            el.Click();
            Thread.Sleep(1000);
            
           
           
        }

    }
}
