using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Threading;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.Support.UI;
using Team12.Data;

namespace TestProject
{
    [TestFixture(typeof(ChromeDriver))]
    [TestFixture(typeof(FirefoxDriver))]
    class UiTestDeleteSkill<T> where T : IWebDriver, new()
    {
        private IWebDriver _driver;

        [SetUp]
        public void Setup()
        {
            _driver = new T();
            _driver.Navigate().GoToUrl("https://localhost:5001/");
            var skilllink = _driver.FindElement(By.LinkText("Skills"));
            skilllink.Click();
            Thread.Sleep(2000);

            var createbtn = _driver.FindElement(By.Id("Create"));
            createbtn.Click();
            Thread.Sleep(2000);
            
            var skillname = _driver.FindElement(By.Id("skillname"));
            skillname.SendKeys("TestDelete");
            
            IWebElement skillkat = _driver.FindElement(By.Id("skillkategorie"));
            SelectElement selectkat = new SelectElement(skillkat);
            selectkat.SelectByText("Softskill");
            
            var submitbtn = _driver.FindElement(By.Id("submit"));
            submitbtn.Click();
            Thread.Sleep(2000);
        }

        [Test]

        public void DeleteSkillTest() {

            var updatebtn = _driver.FindElement(By.Id("Delete-TestDelete"));
            updatebtn.Click();
            Thread.Sleep(2000);
            
            IWebElement tableElement = _driver.FindElement(By.XPath("//table"));
            IList<IWebElement> tableRow = tableElement.FindElements(By.TagName("tr"));
            IList<IWebElement> rowTD;
            bool success = false;

            foreach (IWebElement row in tableRow)
            {
                rowTD = row.FindElements(By.TagName("td"));
                if (rowTD.Count() >= 2)
                {
                    if (rowTD[0].Text.Equals("TestDelete") && rowTD[1].Text.Equals(Skilltype.Softskill.ToString()))
                        success = true;
                }
            }
            Assert.IsFalse(success);
        }
    }
}