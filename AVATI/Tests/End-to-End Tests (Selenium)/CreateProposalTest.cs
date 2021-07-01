using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace Tests
{
    [TestFixture(typeof(FirefoxDriver))]
    [TestFixture(typeof(ChromeDriver))]
    class UiProposalOverview<T> where T : IWebDriver, new()
    {
        private IWebDriver _driver;

        [SetUp]
        public void Setup()
        {
            if (typeof(T) == typeof(FirefoxDriver))
            {
                var options = new FirefoxOptions();
                options.AddAdditionalCapability("acceptInsecureCerts", true, true);
                _driver = new FirefoxDriver(options);
            }
            else
            {
                _driver = new T();
            }
            
        }

        [Test]
        public void CreateProposalTest()
        {
            _driver.Navigate().GoToUrl("https://localhost:5001/");
            var tabBtn = _driver.FindElement(By.Id("proposalOverview"));
            tabBtn.Click();
            Thread.Sleep(2000);

            var createbtn = _driver.FindElement(By.Id("createNew"));
            createbtn.Click();
            Thread.Sleep(2000);

            var element = _driver.FindElement(By.XPath("//*[ text() = 'C#' ]"));
            element.Click();
            
            var titleInput = _driver.FindElement(By.Id("title"));
            titleInput.SendKeys("Das hier ist ein kleines Testprojekt, ganz schnieke eigl. :)");

            var addInput = _driver.FindElement(By.Id("addInfo"));
            addInput.SendKeys("Viel gibt es hier nicht wirklich zu sagen, ich hab halt ein kleines Angebot erstellt ohne drum und dran");
            
            var final = _driver.FindElement(By.Id("submit"));
            final.Click();
            Thread.Sleep(2000);

            var result = _driver.FindElement(By.XPath("//*[ text() = 'Das hier ist ein kleines Testprojekt, ganz schnieke eigl. :)' ]"));
            Assert.IsTrue(result != null);

        }

        
    }
}

