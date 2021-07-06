using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace Tests
{
    [TestFixture(typeof(FirefoxDriver))]
    [TestFixture(typeof(ChromeDriver))]
    class UiProposalOverview<T> where T : IWebDriver, new()
    {
        private IWebDriver _driver;

        [OneTimeSetUp]
        public void Setup()
        {
            if (typeof(T) == typeof(FirefoxDriver))
            {
                FirefoxDriverService geckoService = FirefoxDriverService.CreateDefaultService();
                geckoService.Host = "::1";
                var options = new FirefoxOptions {AcceptInsecureCertificates = true};
                _driver = new FirefoxDriver(geckoService, options);
            }
            else
            {
                _driver = new T();
                _driver.Manage();
            }
        }

        [Test]
        public void A_CreateProposalTest()
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

            element = _driver.FindElement(By.XPath("//*[ text() = 'Analytische Fähigkeiten' ]"));
            element.Click();

            element = _driver.FindElement(By.XPath("//*[ text() = 'Impulsgeben' ]"));
            element.Click();

            element = _driver.FindElement(By.XPath("//*[ text() = 'IT' ]"));
            element.Click();

            element = _driver.FindElement(By.XPath("//*[ text() = 'Kunst/Kultur' ]"));
            element.Click();

            Thread.Sleep(1000);

            var titleInput = _driver.FindElement(By.Id("title"));
            titleInput.SendKeys("Testprojekt");

            var addInput = _driver.FindElement(By.Id("addInfo"));
            addInput.SendKeys(
                "Viel gibt es hier nicht wirklich zu sagen, ich hab halt ein kleines Angebot erstellt ohne drum und dran");

            Thread.Sleep(2000);

            var final = _driver.FindElement(By.Id("submit"));
            final.Click();
            Thread.Sleep(2000);

            var result = _driver.FindElement(By.XPath("//*[ text() = 'Testprojekt' ]"));
            Assert.IsTrue(result != null);
        }

        [Test]
        public void B_AddEmployeesToProposalTest()
        {
            Thread.Sleep(2000);

            var element = _driver.FindElement(By.Id("sucheProp"));
            element.SendKeys("Testprojekt");

            Thread.Sleep(2000);

            element = _driver.FindElement(By.Id("empProfile"));
            element.Click();

            Thread.Sleep(2000);

            element = _driver.FindElement(By.Id("addEmployee"));
            element.Click();
            Assert.IsTrue(_driver.Url.Contains("/SearchEmployee/"));

            Thread.Sleep(1000);

            var elements = _driver.FindElements(By.Id("hardskillsToAdd"));
            Random rand = new Random();
            foreach (var el in elements.Take(20))
            {
                Console.WriteLine("Result" + el.Text);
                if (rand.Next(30) == 3)
                {
                    Thread.Sleep(1000);
                    el.Click();
                }
            }

            elements = _driver.FindElements(By.Id("categoriesToAdd"));
            foreach (var el in elements.Take(5))
            {
                Console.WriteLine("Result" + el.Text);
                if (rand.Next(3) == 2)
                {
                    Thread.Sleep(1000);
                    el.Click();
                }
            }

            elements = _driver.FindElements(By.Id("softskillsToAdd"));
            foreach (var el in elements.Take(5))
            {
                Console.WriteLine("Result" + el.Text);
                if (rand.Next(2) == 1)
                {
                    Thread.Sleep(1000);
                    el.Click();
                }
            }

            elements = _driver.FindElements(By.Id("rolesToAdd"));
            foreach (var el in elements.Take(2))
            {
                Thread.Sleep(1000);
                el.Click();
            }

            element = _driver.FindElement(By.Id("submitQuery"));
            element.Click();
            Thread.Sleep(10000);
            Assert.IsNotNull(_driver.FindElement(By.Id("result-table")));
            element = _driver.FindElement(By.Id("employeeAdd"));
            element.Click();

            element = _driver.FindElement(By.Id("returnBtn"));
            element.Click();
            Thread.Sleep(2000);
            Assert.IsTrue(_driver.Url.Contains("ProposalOverview"));
        }

        [Test]
        public void C_CopyAndDeleteProposal()
        {
            var element = _driver.FindElement(By.Id("sucheProp"));
            element.Clear();
            element.SendKeys("Testprojekt");

            Thread.Sleep(2000);

            Assert.IsNotNull(_driver.FindElement(By.Id("empName")));


            element = _driver.FindElement(By.Id("empProfile"));
            element.Click();

            Thread.Sleep(2000);

            element = _driver.FindElement(By.Id("copyProposal"));
            element.Click();

            Thread.Sleep(5000);

            element = _driver.FindElement(By.Id("sucheProp"));
            element.Clear();
            element.SendKeys("Testprojekt [KOPIE]");

            Thread.Sleep(2000);
            Assert.IsNotNull(_driver.FindElement(By.XPath("//*[ text() = 'Testprojekt [KOPIE]' ]")));

            element = _driver.FindElement(By.Id("deleteProp"));
            element.Click();
            Thread.Sleep(1000);
            element = _driver.FindElement(By.Id("deletePropConfirm"));
            element.Click();
            Thread.Sleep(3000);
        }

        [Test]
        public void D_EditProposalTest()
        {
            var element = _driver.FindElement(By.Id("sucheProp"));
            element.Clear();
            Thread.Sleep(1000);
            element.SendKeys("Test");
            Thread.Sleep(5000);
            element = _driver.FindElement(By.Id("editProposal"));
            element.Click();
            Thread.Sleep(3000);
            element = _driver.FindElement(By.XPath("//*[ text() = 'Analytische Fähigkeiten' ]"));
            element.Click();

            element = _driver.FindElement(By.XPath("//*[ text() = 'Impulsgeben' ]"));
            element.Click();

            element = _driver.FindElement(By.XPath("//*[ text() = 'IT' ]"));
            element.Click();

            element = _driver.FindElement(By.XPath("//*[ text() = 'Kunst/Kultur' ]"));
            element.Click();
            
            var titleInput = _driver.FindElement(By.Id("title"));
            titleInput.SendKeys("tEsTpRoJeKt mit weniger Attributen");

            var addInput = _driver.FindElement(By.Id("addInfo"));
            addInput.SendKeys(
                "Sehr minimalistisches Angebot");

            Thread.Sleep(2000);

            var final = _driver.FindElement(By.Id("submit"));
            final.Click();
            Thread.Sleep(2000);
        }

        [Test]
        public void E_AlterEmployeeDetail()
        {
            var element = _driver.FindElement(By.Id("empProfile"));
            element.Click();
            Thread.Sleep(3000);
            element = _driver.FindElement(By.Id("empDetail"));
            element.Click();
            Thread.Sleep(3000);
            var final = _driver.FindElement(By.Id("submitDetail"));
            final.Click();
            Thread.Sleep(2000);
            element = _driver.FindElement(By.Id("empDetail"));
            element.Click();
            Thread.Sleep(12000);
            final = _driver.FindElement(By.Id("submitDetail"));
            final.Click();
            
            
        }
        

        [Test]
        public void F_CreateProjectFromProposal()
        {
            var jse = (IJavaScriptExecutor) _driver;

            const string script =
                "window.scrollTo(0, document.body.scrollHeight)";
            var element = _driver.FindElement(By.Id("sucheProp"));
            element.Clear();
            element.SendKeys("Testprojekt");

            Thread.Sleep(2000);
            element = _driver.FindElement(By.Id("createProj"));
            element.Click();
            _driver.ExecuteJavaScript("window.scrollTo(0, document.body.scrollHeight)");

            jse.ExecuteScript(script);

            Thread.Sleep(3000);

            element = _driver.FindElement(By.Id("createBtn"));
            element.Click();
            Thread.Sleep(3000);
            Assert.IsTrue(_driver.Url.Contains("/Projekt"));
            _driver.Close();
        }
    }
}