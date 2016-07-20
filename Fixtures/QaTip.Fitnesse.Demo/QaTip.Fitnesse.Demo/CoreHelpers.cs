using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Xml.Linq;
using System.ServiceProcess;
using QaTip.Fitnesse.Demo.DoFixture;

namespace QaTip.Fitnesse.Demo
{
    public class CoreHelpers
    {
        public static IWebElement WaitForElementToExist(IWebDriver driver, By elementFindCriteria, int maxWaitTimeSeconds, bool manditory)
        {
            DateTime start = DateTime.Now;
            IWebElement element = null;

            while (DateTime.Now - start < new TimeSpan(0, 0, maxWaitTimeSeconds))
            {
                try
                {
                    element = driver.FindElement(elementFindCriteria);
                    break;
                }
                catch (NoSuchElementException)
                {
                    // Element does exist yet
                }

                System.Threading.Thread.Sleep(200);
            }

            if (element == null && manditory)
            {
                throw new ApplicationException("Web element never appeared!");
            }

            return element;
        }
        public static string servicename { get; set; }

        public static string GetWebPageTitle(string url)
        {
            // Create a request to the url
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;

            // If the request wasn’t an HTTP request (like a file), ignore it
            if (request == null) return null;

            // Use the user’s credentials
            request.UseDefaultCredentials = true;

            // Obtain a response from the server, if there was an error, return nothing
            HttpWebResponse response = null;
            try { response = request.GetResponse() as HttpWebResponse; }
            catch (WebException) { return null; }

            // Regular expression for an HTML title
            string regex = @"(?<=<title.*>)([\s\S]*)(?=</title>)";

            // If the correct HTML header exists for HTML text, continue
            if (new List<string>(response.Headers.AllKeys).Contains("Content-Type"))
                if (response.Headers["Content-Type"].StartsWith("text/html"))
                {
                    // Download the page
                    WebClient web = new WebClient();
                    web.UseDefaultCredentials = true;
                    string page = web.DownloadString(url);

                    // Extract the title
                    Regex ex = new Regex(regex, RegexOptions.IgnoreCase);
                    return ex.Match(page).Value.Trim();
                }

            // Not a valid HTML page
            return null;
        }


        public static XDocument GetConfigFileAsXDoc(string configFilePath)
        {

            FileStream fs = new FileStream(configFilePath, FileMode.Open, FileAccess.Read);


            StreamReader sr = new StreamReader(fs);

            string inXml = sr.ReadToEnd();

            // Adding Replace to fix Issue with UI config file that contained the listed value below which causes issues when parsing xdocument

            inXml = inXml.Replace("&trade;", "")

            .Replace("&reg;", "")

            .Replace("&copy;", "");

            sr.Close();

            fs.Close();

            return XDocument.Parse(inXml);

        }



        //Lets add wait time here 

        public static string Wait(int WaitSeconds)
        {
            int sleepMs = WaitSeconds == null ? 0 : (WaitSeconds) * 1000;
            System.Threading.Thread.Sleep(sleepMs);
            return "Slept " + sleepMs / 1000 + " seconds";
        }

        //database 
        // Get Record count for query
        public static int GetRecordCountForQuery(string command, string _dbConnectionString)
        {
            DateTime started = DateTime.Now;
            TimeSpan maxWait = new TimeSpan(0, 1, 0);
            int _returnedRowCount = 0;

            LogMessage("Executing query for row count:");
            LogMessage(command);

            using (OracleConnection connection = new OracleConnection(_dbConnectionString))
            {
                OracleCommand oCommand = new OracleCommand(command, connection);
                connection.Open();
                OracleDataReader reader = oCommand.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        _returnedRowCount++;
                        // Want to make sure we don't get stuck here, so use a default timeout
                        if (DateTime.Now - started > maxWait)
                        {
                            connection.Close();
                            throw new ApplicationException(String.Format("Times out collecting records for this query: {0} after {1} seconds.", command, maxWait.Seconds));
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return _returnedRowCount;
        }



        public static void WaitForElementToLeave(IWebElement container, By elementFindCriteria, int maxWaitTimeSeconds)
        {
            DateTime start = DateTime.Now;
            try
            {
                IWebElement el = container.FindElement(elementFindCriteria);
                while (el != null && DateTime.Now - start < new TimeSpan(0, 0, maxWaitTimeSeconds))
                {
                    el = container.FindElement(elementFindCriteria);

                    // If the element can be found, but is not displayed, return
                    if (!el.Displayed)
                        return;

                    System.Threading.Thread.Sleep(200);
                }
            }
            catch (NoSuchElementException)
            {
                // Element was not found or is gone, return
            }
        }

        public static IWebElement WaitForElementToExist(IWebDriver driver, By elementFindCriteria, int maxWaitTimeSeconds)
        {
            return WaitForElementToExist(driver, elementFindCriteria, maxWaitTimeSeconds, true);
        }

        public static void LogMessage(string message)
        {
            Console.WriteLine("{0}: {1}", DateTime.Now, message);
        }


        //get service name
        public static string GetServiceName(string serviceName)
        {
            if (serviceName == null)
            {
                throw new ApplicationException("Failed to supply a service name!");
            }

            return serviceName;
        }

        public static bool StartService(string serviceName, out string output)
        {
            output = "No Issues. ";
            bool result = false;

            if (serviceName == servicename)
            {
                // Make sure that the "IBM MQSeries" service has been started
                result = StartService(serviceName, out output);

                if (!result)
                {
                    return result;
                }
            }

            ServiceController sc = new ServiceController(serviceName);
            try
            {
                if (sc.Status != ServiceControllerStatus.Running)
                {
                    LogMessage(string.Format("Current Service state: {0}", sc.Status.ToString()));
                    LogMessage(string.Format("Starting {0}", serviceName));
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 120));
                    LogMessage(string.Format("Started {0}", serviceName));
                    output += string.Format("Started {0}. ", serviceName);
                }
                else
                {
                    LogMessage(string.Format("{0} is already running.", serviceName));
                    output += string.Format("{0} is already running. ", serviceName);
                }

                sc.Refresh();
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    result = true;
                }
                else
                {
                    output = string.Format("{0} status is not Running.  Max wait time for service start is {1}. ", serviceName, Constants.MAXSERVICEWAITTIME);
                }

                sc.Close();
            }
            catch (InvalidOperationException e)
            {
                output = e.Message;
                output += e.InnerException.Message;
                return false;
            }

            return result;
        }


        public static bool StopService(string serviceName, out string output)
        {
            output = "No Issues. ";
            bool result = false;

            ServiceController sc = new ServiceController(serviceName);

            try
            {
                if (sc.Status != ServiceControllerStatus.Stopped)
                {
                    LogMessage(string.Format("Stopping {0}", serviceName));
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, Constants.MAXSERVICEWAITTIME));
                    output += string.Format(" Stopped {0}. ", serviceName);
                }
                else
                {
                    output += string.Format(" {0} was already stopped. ", serviceName);
                }

                sc.Refresh();
                if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    result = true;
                }
                else
                {
                    output = string.Format("{0} status is not Stopped.  Max wait time for service stop is {1}. ", serviceName, Constants.MAXSERVICEWAITTIME);
                }

                sc.Close();
            }
            catch (InvalidOperationException e)
            {
                output = e.Message;
                output += e.InnerException.Message;
                return false;
            }

            return result;
        }


        public static IWebDriver InitializeSeleniumBrowser()
        {
            IWebDriver selDriver;
           // string targetedBrowser = System.Configuration.ConfigurationManager.AppSettings.Get("TargetedBrowser");
            string targetedBrowser = "ie";
            switch (targetedBrowser.ToLower())
            {
                case "chrome":
                    {
                        CoreHelpers.LogMessage("Executing test using Chrome browser.");
                        selDriver = new ChromeDriver();
                        break;
                    }
                case "ie":
                case "internet explorer":
                case "internetexplorer":
                default:
                    {
                        CoreHelpers.LogMessage("Executing test using Internet Explorer browser.");
                       // selDriver = new InternetExplorerDriver(@"C:\Training\Fitnesse\Fixtures\Fitnesse_Dlls\dependencies");
                        selDriver = new InternetExplorerDriver(new InternetExplorerOptions() { IntroduceInstabilityByIgnoringProtectedModeSettings = true, }); // articles states this is not neccessary if the zones are already set. it might introduce instability 
                        break;
                    }
            }
            return selDriver;
        }

    }
}
