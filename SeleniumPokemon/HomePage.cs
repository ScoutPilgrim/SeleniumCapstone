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
    public class HomePage
    {
        [TestMethod]
        public void Should_Nav_To_Home()
        {
            using (var chromeDriver = new ChromeDriver())
            {
                //Maximizes Test Window
                chromeDriver.Manage().Window.Maximize(); 
                //Navigates to Home of Pokemon Website
                chromeDriver.Navigate().GoToUrl("https://www.pokemon.com/us/");
                //Sets a wait before assert that correct URL has been reached
                chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
                //test asserts that home URL has been reached
                Assert.AreEqual(chromeDriver.Url, "https://www.pokemon.com/us/");
                
            }
        }
        [TestMethod]
        public void Should_Find_Search_Bar()
        {
            using (var chromeDriver = new ChromeDriver())
            {
                //Maximizes Test Window
                chromeDriver.Manage().Window.Maximize();
                //Navigate
                chromeDriver.Navigate().GoToUrl("https://www.pokemon.com/us/");
                //Set Timer To Give time for page loading
                var wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(15));
                //Dismiss Cookies
                var cookieDismiss = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='cookie-dismisser']")));
                cookieDismiss.Click();
                //click Nav Bar Search icon to pop-up search Modal
                var searchButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//a[@class='icon icon_search']")));
                searchButton.Click();
                //enter text into seacrh bar modal
                var searchBarModal = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//input[@id='site-search-widget-term']")));
                searchBarModal.SendKeys("grimer");
                //find and click actual search button
                var searchButtonGo = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//input[@id='site-search-widget-submit']")));
                searchButtonGo.Click();
                //Assert to complete test
                Assert.AreEqual(chromeDriver.Url, "https://www.pokemon.com/us/search/#/results/grimer/1/");
            }
        }
        [TestMethod]
        public void Pokemon_Company_Footer_Links() {
            using (var chromeDriver = new ChromeDriver())
            {
                //Maximize Test Window
                chromeDriver.Manage().Window.Maximize();
                //Navigate
                chromeDriver.Navigate().GoToUrl("https://www.pokemon.com/us/");
                //Set Timer for page loading
                var wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(15));
                //Dismiss Cookies
                var cookieDismiss = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='cookie-dismisser']")));
                cookieDismiss.Click();
                //Test Pokemon News Footer
                var whatsNewFooter = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//a[contains(text(),\"What's New\")]")));
                whatsNewFooter.Click();
                //Assert footer link is properly navigating
                Assert.AreEqual(chromeDriver.Url, "https://www.pokemon.com/us/pokemon-news/");
                //Navigate Back Home to test other links
                chromeDriver.Navigate().GoToUrl("https://www.pokemon.com/us/");
                //Test Parent's Guide
                var parentsGuideFooter = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//a[contains(text(), 'Pokémon Parents Guide')]")));
                parentsGuideFooter.Click();
                //Assert footer link is properly navigating
                Assert.AreEqual(chromeDriver.Url, "https://www.pokemon.com/us/parents-guide/");
                //Navigate Back Home
                chromeDriver.Navigate().GoToUrl("https://www.pokemon.com/us/");
                //Test Customer Service
                var customerServiceFooter = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//a[contains(text(), 'Customer Service')]")));
                customerServiceFooter.Click();
                //Assert footer link is properly navigating
                Assert.AreEqual(chromeDriver.Url, "https://support.pokemon.com/hc/en-us");
                //Navigate Back Home
                chromeDriver.Navigate().GoToUrl("https://www.pokemon.com/us/");
                //Test About Our Company
                var aboutUsFooter = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//a[contains(text(), 'About Our Company')]")));
                aboutUsFooter.Click();
                //Assert footer link is properly navigating
                Assert.AreEqual(chromeDriver.Url, "https://www.pokemon.com/us/about-pokemon/");
                //Navigate Back Home
                chromeDriver.Navigate().GoToUrl("https://www.pokemon.com/us/");
                //Test Pokemon Careers - This opens new tab NEED to address this
                var pokemonCareersFooter = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//a[contains(text(), 'Pokémon Careers')]")));
                pokemonCareersFooter.Click();
                //Pause the thread to allow time for new tab to open
                Thread.Sleep(1500);
                //Handle new tab being made
                var myTabs = new ReadOnlyCollection<string>(chromeDriver.WindowHandles);
                chromeDriver.SwitchTo().Window(myTabs[1]);
                //Assert footer link is properly navigating
                Assert.AreEqual(chromeDriver.Url, "https://chj.tbe.taleo.net/chj04/ats/careers/jobSearch.jsp?act=redirectCws&cws=1&org=POKEMON");
                //Switch back to original tab
                chromeDriver.Close();
                chromeDriver.SwitchTo().Window(myTabs[0]);
                //Navigate Back Home
                chromeDriver.Navigate().GoToUrl("https://www.pokemon.com/us/");
                //Test Region Select
                var regionSelectFooter = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//a[contains(text(),'Select a Country/Region')]")));
                regionSelectFooter.Click();
                //Assert that footer link is properly navigating
                Assert.AreEqual(chromeDriver.Url, "https://www.pokemon.com/us/country/");
            }
        }
        [TestMethod]
        public void Print_Out_Links_Status()
        {
            using (var chromeDriver = new ChromeDriver())
            {
                //Setup Home Page var
                string homePage = "https://www.pokemon.com/us/";
                //Init HttpReq
                HttpWebRequest request = null;
                //Maximize Window
                chromeDriver.Manage().Window.Maximize();
                //Navigate to Home Page
                chromeDriver.Navigate().GoToUrl(homePage);
                //Add a wait to make sure the page is completely loaded
                new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(12)).Until(
                    d => ((IJavaScriptExecutor) d).ExecuteScript("return document.readyState").Equals("complete"));
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
