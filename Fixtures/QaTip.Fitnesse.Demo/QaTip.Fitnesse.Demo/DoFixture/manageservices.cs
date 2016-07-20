using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fit;

namespace QaTip.Fitnesse.Demo.DoFixture
{
   public class manageservices:ColumnFixture
    {
        private string _output;

        public manageservices(string integrationType)
        {
        }

        /// <summary>
        /// Service name.  For use in Service related operations
        /// </summary>
        public string ServiceName
        { get; set; }

        /// <summary>
        /// Service Restart.  For use in Service related operations
        /// </summary>
        public string ServiceRestart
        { get; set; }

        /// <summary>
        /// Generic holder for any detail oriented info that can be communicated
        /// </summary>
        public string Output
        {
            get
            {
                return _output;
            }
        }
        public string EnsureServiceStarted()
        {
            return StartService();
        }

        // <summary>
        /// Starts a given service if it is not running
        /// Specify "engine" or "paid" to start the appropriate service for the given integration type
        /// or specify the specific name of a service, ie. TPP.TrackLoaderService
        /// </summary>
        public string StartService()
        {
            bool result = false;
            string currentOutput = string.Empty;

            ServiceName = CoreHelpers.GetServiceName(ServiceName);

            result = CoreHelpers.StartService(ServiceName, out currentOutput);
            _output += currentOutput;
            ServiceName = null;

            return result ? "SUCCESS" : "FAIL";
        }

        public string StopService()
        {
            bool result = false;
            string currentOutput = string.Empty;

            ServiceName = CoreHelpers.GetServiceName(ServiceName);

            result = CoreHelpers.StopService(ServiceName, out currentOutput);
            _output += currentOutput;
            ServiceName = null;

            return result ? "SUCCESS" : "FAIL";
        }

    }
}
