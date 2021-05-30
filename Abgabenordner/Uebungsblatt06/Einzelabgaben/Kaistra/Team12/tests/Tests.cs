using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace TestProject1
{
    public class Program
    {
        [Test]
        public static void Test2CreationChrome()
        {
            IWebDriver webDriver = new ChromeDriver();

            webDriver.Navigate().GoToUrl("https://localhost:5001");

            Thread.Sleep(2000);
            var linkTuEtwas = webDriver.FindElement(By.Id("blep"));
            linkTuEtwas.Click();
            Thread.Sleep(2000);
            var linkAdd = webDriver.FindElement(By.Id("ADDIERE"));
            linkAdd.Click();
            Thread.Sleep(2000);
            var eingabeSkill = webDriver.FindElement(By.Id("name"));
            eingabeSkill.SendKeys("testProgramm");
            var submitButton = webDriver.FindElement(By.Id("Submit"));
            submitButton.Click();
        }
        
        [Test]
        public static void Test2CreationFirefox()
        {
            FirefoxOptions firefoxOptions = new FirefoxOptions();
            firefoxOptions.AcceptInsecureCertificates = true;

            IWebDriver webDriver = new FirefoxDriver(firefoxOptions);
            webDriver.Navigate().GoToUrl("https://localhost:5001");

            Thread.Sleep(2000);
            var linkTuEtwas = webDriver.FindElement(By.Id("blep"));
            linkTuEtwas.Click();
            Thread.Sleep(2000);
            var linkAdd = webDriver.FindElement(By.Id("ADDIERE"));
            linkAdd.Click();
            Thread.Sleep(2000);
            var eingabeSkill = webDriver.FindElement(By.Id("name"));
            eingabeSkill.SendKeys("testProgramm");
            var submitButton = webDriver.FindElement(By.Id("Submit"));
            submitButton.Click();
        }
        
        [Test]
        public static void GeneriereDokumentChrome()
        {
            IWebDriver webDriver = new ChromeDriver();
            webDriver.Navigate().GoToUrl("https://localhost:5001");

            Thread.Sleep(2000);
            var linkGeneriere = webDriver.FindElement(By.Id("download Kaistra"));
            linkGeneriere.Click();
        }
        [Test]
        public static void GeneriereDokumentFirefox()
        {
            FirefoxOptions firefoxOptions = new FirefoxOptions();
            firefoxOptions.AcceptInsecureCertificates = true;

            IWebDriver webDriver = new FirefoxDriver(firefoxOptions);
            webDriver.Navigate().GoToUrl("https://localhost:5001");

            Thread.Sleep(2000);
            var linkGeneriere = webDriver.FindElement(By.Id("download Kaistra"));
            linkGeneriere.Click();
        }
    }
}