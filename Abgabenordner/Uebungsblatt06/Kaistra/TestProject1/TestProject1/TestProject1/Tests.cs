using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TestProject1
{
    public class Program
    {
        [Test]
        public static void test2Creation()
        {
            IWebDriver webDriver = new ChromeDriver();
            webDriver.Navigate().GoToUrl("https://localhost:5001");

            var linkTuEtwas = webDriver.FindElement(By.LinkText("tu etwas"));
            linkTuEtwas.Click();
            Thread.Sleep(2000);
            var linkAdd = webDriver.FindElement(By.LinkText("Add"));
            linkAdd.Click();
            Thread.Sleep(2000);
            var eingabeSkill = webDriver.FindElement(By.Id("name"));
            eingabeSkill.SendKeys("testProgramm");
            var submitButton = webDriver.FindElement(By.LinkText("Submit"));
            submitButton.Click();
            submitButton.Click(); //will nicht aktivieren
            Thread.Sleep(2000);
            Assert.AreEqual(webDriver.Url, "https://localhost:5001/skill/edit");
            var succes = webDriver.FindElement(By.Name("testProgramm"));
            Console.WriteLine(succes.Text);
        }

        public static void GeneriereDokument()
        {
            IWebDriver webDriver = new ChromeDriver();
            webDriver.Navigate().GoToUrl("https://localhost:5001");

            Thread.Sleep(2000);
            var linkGeneriere = webDriver.FindElement(By.XPath("//button/Generiere Word DOC Kaistra"));
            linkGeneriere.Click();
        }
    }
}