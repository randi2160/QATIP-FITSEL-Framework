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
           //We can setup Implicit wait etc.. will talk about this later
            selDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5)).SetPageLoadTimeout(TimeSpan.FromSeconds(15));
            selDriver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(20));
            _SearchMap.selDriver = selDriver;
            
        }

       public override object GetTargetObject()
       {
           return _SearchMap;
       }
    }
}
