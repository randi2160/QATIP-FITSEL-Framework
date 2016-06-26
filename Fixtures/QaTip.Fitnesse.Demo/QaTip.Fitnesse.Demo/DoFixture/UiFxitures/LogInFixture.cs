using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using fit;
using OpenQA.Selenium.Chrome;

namespace QaTip.Fitnesse.Demo.DoFixture.UiFxitures
{
    public class LogInFixture : ColumnFixture
    {
        private IWebDriver selDriver;
        private string _url = "http://google.com";
        private string _emailAddress = "auto@cmtest.com";
        private string _username;
        private string _password;
        public LogInFixture(IWebDriver selDriver)
        {
            this.selDriver = selDriver;
        }

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }

        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public void navigatetogoogle()
        {
            Navigate();
        }
        private void Navigate()
        {
            selDriver.Manage().Window.Maximize();
            selDriver.Navigate().GoToUrl(_url);
        }
    }
}
