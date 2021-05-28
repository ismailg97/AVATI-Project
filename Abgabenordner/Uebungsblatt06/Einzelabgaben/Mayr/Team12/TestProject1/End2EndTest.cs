using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using Team12.Data;

namespace TestProject1
{
    public class End2EndTest
    {
        private IWebDriver _firefox;

        [SetUp]
        public void Setup() {
            _firefox = new FirefoxDriver();
        }

        [Test]
        public void TestEditSkill() {
            _firefox.Navigate().GoToUrl("http://localhost:5000/skill/edit");
            System.Threading.Thread.Sleep(750);

            IWebElement input = _firefox.FindElement(By.Id("name"));
            input.SendKeys("New SoftSkill");
            SelectElement select = new SelectElement(_firefox.FindElement(By.Id("category")));
            select.SelectByText("Soft-Skill");
            _firefox.FindElement(By.Id("sub")).Click();
            Assert.IsNotNull(SkillNameConventionAttribut.ErrorMessage);
        }
    }
}