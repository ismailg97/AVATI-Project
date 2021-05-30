using System;
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
            IWebElement el2 = _web.FindElement(By.LinkText("Skills"));
            el2.Click();
            Thread.Sleep(1000);
            
            el2 = _web.FindElement(By.Id("bearbeiten"));
            el2.Click();
            Thread.Sleep(1000);
            
            el2 = _web.FindElement(By.Id("name"));
            el2.SendKeys("test");
            Thread.Sleep(1000);
            el2 = _web.FindElement(By.Id("absenden"));
            el2.Click();
            Thread.Sleep(1000);
           

            _web.Close();

            _web = new ChromeDriver();
            _web.Navigate().GoToUrl("https://localhost:5001");
            Thread.Sleep(1000);
            IWebElement el = _web.FindElement(By.LinkText("Skills"));
            el.Click();
            Thread.Sleep(1000);
            
            el = _web.FindElement(By.Id("bearbeiten"));
            el.Click();
            Thread.Sleep(1000);
            
            el = _web.FindElement(By.Id("name"));
            el.SendKeys("test2");
            Thread.Sleep(1000);
            el = _web.FindElement(By.Id("absenden"));
            el.Click();
            Thread.Sleep(1000);
            
            _web.Close();

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
            IWebElement el4 = _web.FindElement(By.LinkText("Skills"));
            el4.Click();
            Thread.Sleep(1000);
            
            el4 = _web.FindElement(By.Id("loeschen"));
            el4.Click();
            Thread.Sleep(1000);
            
            
            _web.Close();

            _web = new ChromeDriver();
            _web.Navigate().GoToUrl("https://localhost:5001");
            Thread.Sleep(1000);
            IWebElement el = _web.FindElement(By.LinkText("Skills"));
            el.Click();
            Thread.Sleep(1000);
            
            el4 = _web.FindElement(By.Id("loeschen"));
            el4.Click();
            Thread.Sleep(1000);
            _web.Close();
        }

    }
}
