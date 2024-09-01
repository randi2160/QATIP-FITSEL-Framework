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
   public  class searchmap:ColumnFixture
    {
       public IWebDriver selDriver;
       public string searchQuery { get; set; }
       //public string productTitle { get; set; }
       public string productDescription
       {

           get;
           set;
       }
      // public string author { get; set; }
      // public string producttype { get; set; }
       //public string productPrice { get; set; }
       public string searchbox
       {
           get;set;
       }

       public  string productTitle
       {
           get
           {
               //i dont like the idea of using Xpath since its very fragile. However while doing this demo tutorial,
               //i find that Barnes and Noble.com site they way the DOM appears its hard to get the exact tile by usind ID. 
               return selDriver.FindElement(By.XPath(".//*[@id='prodSummary']/h1")).Text;
               //return selDriver.FindElement(By.Id("prodSummary")).Text;
           }
          
       }

       public string author
       {
           get
           {

               return selDriver.FindElement(By.XPath(".//*[@id='prodSummary']/span/a")).Text;
           }

       }
       public  string producttype
       {
           get
           {
               //.//*[@id='prodPromo']/h2
               return selDriver.FindElement(By.XPath(".//*[@id='prodPromo']/h2")).Text;
           }

       }
       public  string productPrice
       {
           get
           {
               //.//*[@id='prodInfoContainer']/form[1]/p/span[1]
               //return selDriver.FindElement(By.XPath(".//*[@id='prodInfoContainer']/form[1]/p/span[1]")).Text;
               return selDriver.FindElement(By.ClassName("price")).Text;
           }

       }

       public string SearchBoxExist()
       {
           if (selDriver.FindElement(By.Name("_D:/com/bn/endeca/base/SearchFormHandler.searchErrorURL")).Enabled)
           {
              return searchbox = "YES";
           }
           else
           {
             return  searchbox = "NOT FOUND";
           }
       
       }


        public void executeSearch()
        {
            selDriver.FindElement(By.Id("searchBarBN")).SendKeys(searchQuery);
            selDriver.FindElement(By.Id("searchSubmit")).Click();

            // Setting the implicit wait timeout
            selDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            // Setting the page load timeout
            selDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(15);

            // Setting the script timeout
            selDriver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(20);
        }





    }
}
