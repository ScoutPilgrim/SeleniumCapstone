using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras;
using System.Linq;
using System.Net;
using System.Threading;
using System.Diagnostics;

namespace SeleniumPokemon
{
    [TestClass]
    public class PokedexPage
    {
        [TestMethod]
        public void Should_Nav_To_Pokedex()
        {            
            using (var chromeDriver = new ChromeDriver())
            {
                //Initialize the string that will be the main page to nav to
                string pokedexPage = "https://www.pokemon.com/us/pokedex/";
                //Maximize the testing window
                chromeDriver.Manage().Window.Maximize();
                //Navigate to PokeDex Page
                chromeDriver.Navigate().GoToUrl(pokedexPage);
                //Assert that the page was properly navigated to
                Assert.AreEqual(chromeDriver.Url, pokedexPage);
            }
        }
        [TestMethod]
        public void Print_Out_Links_Status()
        {
            using (var chromeDriver = new ChromeDriver())
            {
                //Setup Pokedex Page var
                string pokedexPage = "https://www.pokemon.com/us/pokedex";
                //Setup Home Page var
                string homePage = "https://www.pokemon.com/us/";
                //Init HttpReq
                HttpWebRequest request = null;
                //Maximize Window
                chromeDriver.Manage().Window.Maximize();
                //Navigate to Home Page
                chromeDriver.Navigate().GoToUrl(pokedexPage);
                //Add a wait to make sure the page is completely loaded
                new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(12)).Until(
                    d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
                //Init and populate a list of Page elements with the <a> tag
                List<IWebElement> pageLinks = chromeDriver.FindElements(By.XPath("//a")).ToList();
                List<IWebElement> cleanList = new List<IWebElement>();
                //cleaning up the list of links
                System.Console.WriteLine("**LIST OF BAD LINKS**");
                foreach (IWebElement element in pageLinks)
                {
                    if ((String.IsNullOrEmpty(element.GetAttribute("href"))) || !element.GetAttribute("href").StartsWith(homePage))
                    {
                        System.Console.WriteLine($"Link with NAME: {element.Text} is either nonexistant or leads to a foreign domain");
                        continue;
                    }
                    else
                    {
                        cleanList.Add(element);
                    }
                }
                System.Console.WriteLine("**LIST OF FOUND LINKS**");
                //Iterate through List and check the URL
                foreach (IWebElement element in cleanList)
                {
                    //Make sure we are not linking emails or links that have no text in them
                    if (!(element.Text.Contains("email") || element.Text == ""))
                    {
                        request = (HttpWebRequest)WebRequest.Create(element.GetAttribute("href"));
                        try
                        {
                            var response = (HttpWebResponse)request.GetResponse();
                            System.Console.WriteLine($"Link with NAME: {element.Text} and URL: {element.GetAttribute("href")} has a status of {response.StatusCode}");
                        }
                        catch (WebException myException)
                        {
                            var errorResponse = (HttpWebResponse)myException.Response;
                        }
                    }
                }
            }
        }
    }
}
