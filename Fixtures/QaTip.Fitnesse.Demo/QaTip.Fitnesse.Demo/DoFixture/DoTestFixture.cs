﻿using System;
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

namespace QaTip.Fitnesse.Demo
{
    public class DoTestFixture
    {
       
        private IWebDriver selDriver;
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
            qatipDataAccess.SetQATIPDataAccessConnectionStrings();
        }
        #region manage config file
        public ManageConfigFixture manageconfigs()
        {
            return new ManageConfigFixture();
        }

        

        #endregion

        #region Generic validation fixture
        public genericvalidationfixture validate()
        {
            return new genericvalidationfixture();
        }
        #endregion


        public ValidateCustomerFixture validatecustomer()
        {
           
            return new ValidateCustomerFixture();
        }

        //Browser Clean Up
        public BrowserShutDownFixture CleanUpAndCloseBrowser()
        {
            IWebDriver selDriverLocal = selDriver;
            selDriver = null;
            return new BrowserShutDownFixture(selDriverLocal);
        }
        //Lets create a Generic function to capture the page title of any given url we navigate to. This way we dont have to hard code what we are expecting

        public ManageNavigationFixture NavigateTo()
        {
            
            if (selDriver == null)
            {
                selDriver = CoreHelpers.InitializeSeleniumBrowser();
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

        //Sample login fixture for UI
        public LogInFixture UILogIn()
        {
            if (selDriver == null)
            {
                selDriver = CoreHelpers.InitializeSeleniumBrowser();
               // selDriver.Manage().Window.Maximize();
            }

            return new LogInFixture(selDriver);
        }
    }
}
