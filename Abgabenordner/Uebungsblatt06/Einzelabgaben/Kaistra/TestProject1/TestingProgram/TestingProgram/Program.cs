using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TestingProgram
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GeneriereDokument();
        }

        [Test]
        public static void test2Creation()
        {
            IWebDriver webDriver = new ChromeDriver();
            webDriver.Navigate().GoToUrl("https://localhost:5001");

            var linkTuEtwas = webDriver.FindElement(By.Id("blep"));
            linkTuEtwas.Click();
            Thread.Sleep(1500);
            var linkAdd = webDriver.FindElement(By.Id("ADDIERE"));
            linkAdd.Click();
            Thread.Sleep(1500);
            var eingabeSkill = webDriver.FindElement(By.Id("name"));
            eingabeSkill.SendKeys("testProgramm");
            var submitButton = webDriver.FindElement(By.Id("Submit"));
            submitButton.Click();
            submitButton.Click(); //will nicht aktivieren
            Thread.Sleep(1500);
            Assert.AreEqual(webDriver.Url, "https://localhost:5001/skill/index");
            
        }

        public static void GeneriereDokument()
        {
            IWebDriver webDriver = new ChromeDriver();
            webDriver.Navigate().GoToUrl("https://localhost:5001");

            Thread.Sleep(2000);
            var linkGeneriere = webDriver.FindElement(By.Id("download Kaistra"));
            linkGeneriere.Click();
        }
    }
}