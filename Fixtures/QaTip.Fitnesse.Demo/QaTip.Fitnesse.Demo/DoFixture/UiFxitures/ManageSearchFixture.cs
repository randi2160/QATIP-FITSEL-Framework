using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using fit;
using OpenQA.Selenium.Chrome;
using QaTip.Fitnesse.Demo.DoFixture.UiFxitures.DomainObject;

namespace QaTip.Fitnesse.Demo.DoFixture.UiFxitures
{
    public class ManageSearchFixture:ColumnFixture
    {
        private searchmap _SearchMap = new searchmap();

        //Creating a constructor that sets selDriver to the instance of the navigationmap class now called _pageread

        public ManageSearchFixture(IWebDriver selDriver)
        {
            // Setting up Implicit wait
            selDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            // Setting up Page load timeout
            selDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(15);

            // Setting up Script timeout
            selDriver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(20);

            // Assigning the driver to the SearchMap object
            _SearchMap.selDriver = selDriver;
        }


        public override object GetTargetObject()
       {
           return _SearchMap;
       }
    }
}
