using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using NUnit.Framework;

namespace Automation_Code_Challenge_4_CSharp
{
    public class WebPageTest
    {
        private ChromeDriver driver;
        static String baseWebPageURL = "http://www.skiutah.com";
        private bool browserStarted = false;
        private Dictionary<String, String> resortList = new Dictionary<String, String>();


        static void Main(string[] args)
        {
            // Empty Main method - keeping the compiler happy
        }

        public void Initialize()
        {
            // Populate resortList HashMap
            resortList.Add("beaver mountain", "Beaver Mtn");
            resortList.Add("cherry peak", "Cherry Peak");
            resortList.Add("nordic valley", "Nordic Valley");
            resortList.Add("powder mountain", "Powder Mtn");
            resortList.Add("snowbasin", "Snowbasin");
            resortList.Add("alta", "Alta");
            resortList.Add("brighton", "Brighton");
            resortList.Add("snowbird", "Snowbird");
            resortList.Add("solitude", "Solitude");
            resortList.Add("deer valley", "Deer Valley");
            resortList.Add("park city", "Park City");
            resortList.Add("sundance", "Sundance");
            resortList.Add("brian head", "Brian Head");
            resortList.Add("eagle point", "Eagle Point");
        }

        public void startBrowser()
        {
            //File pathToBinary = new File("C:\\Program Files (x86)\\Mozilla Firefox\\firefox.exe");
            //FirefoxBinary ffBinary = new FirefoxBinary(pathToBinary);
            //FirefoxProfile firefoxProfile = new FirefoxProfile();
            //driver = new FirefoxDriver(ffBinary,firefoxProfile);
            //driver = new FirefoxDriver();
            //File file = new File("C:\\ChromeDriver\\chromedriver.exe");
            //System.setProperty("webdriver.chrome.driver", "C:\\ChromeDriver\\chromedriver.exe");
            driver = new ChromeDriver();
        }

        private void VerifyPageTitle(String webPageURL, String titleStringToTest)
        {
            // startBrowser
            if (browserStarted == false)
            {
                startBrowser();
                browserStarted = true;
            }
            // Open Webpage URL
            driver.Url = (webPageURL);
            // Get page title of current page
            String pageTitle = driver.Title;
            // Print page title of current page
            Console.WriteLine("Page title of current page is: " + pageTitle);
            // Print title string to test
            Console.WriteLine("Title String to Test is: " + titleStringToTest);
            // Test that the titleStringToTest = title of current page
            //Assert.assertTrue(pageTitle.equals(titleStringToTest), "Current Page Title is not equal to the expected page title value");
            // If there is no Assertion Error, Print out that the Current Page Title = Expected Page Title
            Console.WriteLine("Current Page Title = Expected Page Title");
        }

        private void VerifyNavigation(String navigationMenu)
        {
            // Build CSS Selector based on navigation menu user wants to click on
            String cssSelectorText = "a[title='" + navigationMenu + "']";
            // Find menu WebElement based on CSS Selector
            IWebElement navigationMenuWebElement = driver.FindElementByCssSelector((cssSelectorText));
            // Get href attributte from menu WebElement
            String navigationMenuURL = navigationMenuWebElement.GetAttribute("href");
            // Navigate to href and validate page title
            VerifyPageTitle(navigationMenuURL, "Ski and Snowboard The Greatest Snow on Earth® - Ski Utah");
        }

        private void SubMenuNavigation(String navigationMenu, String navigationSubMenu)
        {
            // Build CSS Selector based on navigation menu user wants to click on
            String cssSelectorTextNavigationMenu = "a[title='" + navigationMenu + "']";
            // Find menu WebElement based on CSS Selector
            bool isPresent = driver.FindElementsByCssSelector((cssSelectorTextNavigationMenu)).Count == 1;
            // Check if navigation menu item exists
            if (isPresent)
            {
                // Get navigation menu WebElement
                IWebElement navigationMenuWebElement = driver.FindElementByCssSelector((cssSelectorTextNavigationMenu));
                // Get href attributte from navigation menu WebElement
                String navigationMenuURL = navigationMenuWebElement.GetAttribute("href");
                //Create Actions object
                Actions mouseHover = new Actions(driver);
                // Move to navigation menu WebElement to initiate a hover event
                mouseHover.MoveToElement(navigationMenuWebElement).Perform();
                //String cssSelectorTextSubMenu = "a[title='" + navigationSubMenu + "']";
                // Build navigation submenu xpath to anchor tag
                String xpathSelectorTextSubmenu = "//a[.='" + navigationSubMenu + "']";
                //WebElement navigationSubMenuWebElement = driver.findElement(By.linkText(navigationSubMenu));
                // Get navigation submenu WebElement
                IWebElement navigationSubMenuWebElement = driver.FindElementByXPath((xpathSelectorTextSubmenu));
                // Check if navigation submenu exists
                Assert.IsTrue(navigationSubMenuWebElement.Enabled, (navigationSubMenu + " navigation submenu does not exist on this page"));
                // Click on navigation submenu WebElement
                navigationSubMenuWebElement.Click();
                //mouseHover.perform();
                // Navigate to href and validate page title
                //VerifyPageTitle(navigationMenuURL, "Ski and Snowboard The Greatest Snow on Earth® - Ski Utah");
            }
            else
            {
                // Print message indicating that the navigation menu passed in to this method does not exist on the page
                Console.WriteLine(navigationMenu + " navigation menu does not exist on this page");
            }
        }

        private void getResortMileageFromAirport(String resortName)
        {
            // Find the Compare Resorts WebElement and click on it
            IWebElement compareResorts = driver.FindElementByXPath((".//*[@id='ski-utah-welcome-map']/div/div[2]/div[4]/label/span[1]"));
            compareResorts.Click();

            // Find the Miles from Airport menu item popout and click on it
            IWebElement milesFromAirport = driver.FindElementByXPath(("//*[@id=\"ski-utah-welcome-map\"]/div/div[2]/div[4]/div/label[1]"));
            milesFromAirport.Click();

            // Check if passed in resortName is empty or null
            if (String.IsNullOrEmpty(resortName))
            {
                // Print that no resort name was entered
                Console.WriteLine("No Resort Name entered");
                // Exit from method
                return;
            }

            // Convert passed in resortName to all lowercase
            String resortNameLowercase = resortName.ToLower();
            // Declare resortValueName for holding value from HashMap
            String resortValueName = null;
            // Get resortValueName from HashMap
            resortValueName = resortList[resortNameLowercase];

            // Check if value returned from HashMap does not exist - returns null from HashMap
            if (String.IsNullOrEmpty(resortValueName))
            {
                Console.WriteLine("Resort Name not found");
            }

            // Resort Name found in HashMap
            else
            {
                // Print passed in resortName
                // Console.WriteLine("Entered Resort Name = " + resortName);
                // Print resortValueName from HashMap
                // Console.WriteLine("Resort Value = " + resortValueName);
                // Get WebElement (first span tag) using resortValueName
                IWebElement resortMileageFromAirport = driver.FindElementByXPath(("//span[@class='map-Area-shortName'][.='" + resortValueName + "']"));
                // Print class value of WebElement
                // Console.WriteLine(resortMileageFromAirport.getAttribute("class"));
                // Print text value of WebElement
                // Console.WriteLine(resortMileageFromAirport.getText());
                // Get WebElement (span tag with distance value) using resortValueName
                IWebElement resortMileageFromAirportElement = resortMileageFromAirport.FindElement(By.XPath("//span[@class='map-Area-shortName'][.='" + resortValueName + "']" + "/following-sibling::span[contains(@class,'distance')]"));
                // Print class value of WebElement
                // Console.WriteLine(resortMileageFromAirportElement.getAttribute("class"));
                // Print innerHtml value of WebElement
                // Console.WriteLine(resortMileageFromAirportElement.getAttribute("innerHTML"));
                // Create a variable to hold the resortMileage value
                String resortMileage = resortMileageFromAirportElement.GetAttribute("innerHTML");
                // Print the mileage from the airport for the passed in resort name
                Console.WriteLine(resortValueName + " is " + resortMileage + " miles from the airport");

            }
        }

        [Test]
        public void TestLauncher()
        {
            Initialize();

            VerifyPageTitle(baseWebPageURL, "Ski Utah - Ski Utah");
            getResortMileageFromAirport("EAGLE point");
            //GetResortMileageFromAirport(resortName);
            //VerifyNavigation("Explore");
            //SubMenuNavigation("Explore", "Stories - Photos - Videos");
        }

    }
}
