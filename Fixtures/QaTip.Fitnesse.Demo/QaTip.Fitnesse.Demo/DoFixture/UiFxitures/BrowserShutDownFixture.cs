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
using System.Diagnostics;
using System.IO;

namespace QaTip.Fitnesse.Demo.DoFixture.UiFxitures
{
    public class BrowserShutDownFixture:ColumnFixture
    {
        private IWebDriver selDriver;

        public BrowserShutDownFixture(IWebDriver selDriver)
        {
            this.selDriver = selDriver;
        }

        public void ClearCache()
        {
            CoreHelpers.LogMessage("Clearing Cache...");
            //http://automationoverflow.blogspot.in/2013/07/clean-session-in-internet-explorer.html

            if (selDriver.GetType() == typeof(OpenQA.Selenium.IE.InternetExplorerDriver))
            {
                ProcessStartInfo psInfo = new ProcessStartInfo();
                psInfo.FileName = Path.Combine(Environment.SystemDirectory, "RunDll32.exe");
                psInfo.Arguments = "InetCpl.cpl,ClearMyTracksByProcess 2";
                psInfo.CreateNoWindow = true;
                psInfo.UseShellExecute = false;
                psInfo.RedirectStandardError = true;
                psInfo.RedirectStandardOutput = true;
                Process p = new Process { StartInfo = psInfo };
                p.Start();
                p.WaitForExit(10000);
            }
            else
            {
                //TBD
            }
        }

        public void ClearCookies()
        {
            CoreHelpers.LogMessage("Clearing Cookies...");
            selDriver.Manage().Cookies.DeleteAllCookies();
        }

        public void CloseBrowser()
        {
            CoreHelpers.LogMessage("Closing Browser...");
            selDriver.Close();
            selDriver.Dispose();
            selDriver = null;
        }
    }
}
