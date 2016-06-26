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
   public class ManageNavigationFixture:ColumnFixture
    {
       private navigationmap _pageread = new navigationmap();
       
       //Creating a constructor that sets selDriver to the instance of the navigationmap class now called _pageread

       public ManageNavigationFixture(IWebDriver selDriver)
        {
           //We can setup Implicit wait etc.. will talk about this later
            selDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5)).SetPageLoadTimeout(TimeSpan.FromSeconds(10));
            selDriver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(20));
           _pageread.selDriver = selDriver;
        }

       public override object GetTargetObject()
       {
           return _pageread;
       }
         
    }
}
