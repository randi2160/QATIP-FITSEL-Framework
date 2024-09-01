using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QaTip.Fitnesse.Demo;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using QaTip.Fitnesse.Demo.DoFixture.UiFxitures;
using System.Net;
using System.Text.RegularExpressions;
using QaTip.Fitnesse.Demo.DoFixture;
using QaTip.Fitnesse.Demo.DoFixture.DBFixtures;
using System.IO;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
//using WebDriverManager.DriverConfigs.Impl;
//using WebDriverManager;

namespace QaTip.Fitnesse.Demo
{
    public class DoTestFixture
    {
        //private IWebDriver selDriver;
        protected IWebDriver selDriver { get;  set; }

        public string connectionstringname { get; set; }
        public string filename { get; private set; }

        public DemoTestSetupFixture helloworld()
        {
            return new DemoTestSetupFixture();
        }

        public QAtipTestHelperFixture testhelper()
        {
            return new QAtipTestHelperFixture();
        }

        public DoTestFixture()
        {
           // System.Diagnostics.Debugger.Launch();
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 |
                                                 System.Net.SecurityProtocolType.Tls13;

            try
            {
                System.Diagnostics.Debugger.Launch();
                new DriverManager().SetUpDriver(new ChromeConfig());
                // Set up the ChromeDriver using WebDriverManager
               
             
                // Initialize the WebDriver
                selDriver = new ChromeDriver();
            }
            catch (AggregateException ex)
            {
                Console.WriteLine("Failed to set up the WebDriver. Please check your network settings and try again.");
                Console.WriteLine(ex.Message);
                // You may want to handle the exception further or provide fallback logic here
            }
        }

        #region Manage Config File
        public ManageConfigFixture manageconfigs()
        {
            return new ManageConfigFixture();
        }
        #endregion

        #region Generic Validation Fixture
        public genericvalidationfixture validate()
        {
            return new genericvalidationfixture();
        }
        #endregion

        public ValidateCustomerFixture validatecustomer()
        {
            return new ValidateCustomerFixture();
        }

        // Browser Clean Up
        public BrowserShutDownFixture CleanUpAndCloseBrowser()
        {
            IWebDriver selDriverLocal = selDriver;
            selDriver = null;
            return new BrowserShutDownFixture(selDriverLocal);
        }

        // Function to capture the page title of any given URL
        public ManageNavigationFixture NavigateTo()
        {
            if (selDriver == null)
            {
                selDriver = CoreHelpers.InitializeSeleniumBrowser("chrome"); // Specify browser type here
                // selDriver.Manage().Window.Maximize();
            }
            return new ManageNavigationFixture(selDriver);
        }

        // Sample Connecting to Database
        public runsqlFixture connect()
        {
            return new runsqlFixture();
        }

        public runsqlFixture RunSqlCommands()
        {
            return new runsqlFixture();
        }

        public dbConnectionFixture ConnectTodb()
        {
            return new dbConnectionFixture();
        }

        public ManageSearchFixture PerformSearch()
        {
            return new ManageSearchFixture(selDriver);
        }

        public ManageSearchFixture GetSearchResults()
        {
            return new ManageSearchFixture(selDriver);
        }

        public ManageFilesAndDirectoriesFixture ManageFilesAndDirectories()
        {
            return new ManageFilesAndDirectoriesFixture();
        }

        // Sample login fixture for UI
        public LogInFixture UILogIn()
        {
            if (selDriver == null)
            {
                selDriver = CoreHelpers.InitializeSeleniumBrowser("chrome"); // Specify browser type here
                // selDriver.Manage().Window.Maximize();
            }

            return new LogInFixture(selDriver);
        }
    }
}
