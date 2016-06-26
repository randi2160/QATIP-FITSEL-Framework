using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using fit;
using OpenQA.Selenium.Chrome;

namespace QaTip.Fitnesse.Demo.DoFixture.UiFxitures.DomainObject
{
    
    public class navigationmap:ColumnFixture
    {
       

        public string url
        {
            get;
            set;
        }
        public string NavigationSuccessful { get; set; }
        public IWebDriver selDriver;
        public string getpagetitle { get; set; }
        public string KeywordContainedInTitle { get; set; }
        public void NavigateToUrl()
        {

            selDriver.Manage().Window.Maximize();
            selDriver.Navigate().GoToUrl(url);

            try
            {
                
                // Now this part here is technically hard coded for specifically Bn.com, since we know that the application we are workign with....
                //However here is a way to bascially dynamically check for any url you use without having to go and hard code for various different URL etc.
                getpagetitle = CoreHelpers.GetWebPageTitle(url);  // This function helps to resolve that. 
                /*
                 here is two approach i am thinking about:
                 * 1. you can allow user to check specifically for what they know will contain in the page title or
                 * 2. just generincally check to see if the lenght is greater than 0 and assume that it there.
                 */
                  System.Threading.Thread.Sleep(10000);
                //if (selDriver.FindElement(By.Id("searchForm")).Displayed == true)
                if (getpagetitle.Contains(KeywordContainedInTitle))
                {
                    NavigationSuccessful = "Success";
                }
                else
                {
                    NavigationSuccessful = "Unsuccessful could not find {0}: " + KeywordContainedInTitle + "The actual page Title i found is" + getpagetitle;
                }
            }
            catch (Exception e)
            {
                NavigationSuccessful = "Unsuccessful" + "" + e.ToString();

            }

        }

      
    }
}
