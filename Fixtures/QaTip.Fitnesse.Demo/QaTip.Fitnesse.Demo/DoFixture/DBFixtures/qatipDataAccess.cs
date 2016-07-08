using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QaTip.Fitnesse.Demo.DoFixture.UiFxitures.DomainObject;

namespace QaTip.Fitnesse.Demo.DoFixture.DBFixtures
{
    public class qatipDataAccess:DataAccess
    {
        //public static new string connectionstringname { get; set; }

        public static void SetQATIPDataAccessConnectionStrings()
        {
           
            // For tests that need to access the main Content Manager db, expect FitNesseContentManagerDbConString to be defined in machine.config
            SetDbConnectionStrings(new string[] { "FitNesseqatipDbConString" });

            if (ApplicationConnectionStrings.Count < 1)
            {
                // Don't want to fail on this just yet as not all may require DB access
                CoreHelpers.LogMessage(string.Format("Did not find connection strings defined for the ContentManager application!"));
                DataAccess.status = "Did not find connection strings defined for the ContentManager application!";

            }
            else
            {
                DbConnectionString = ApplicationConnectionStrings[0].ToString();
                CoreHelpers.LogMessage(string.Format("DbConnectionString = {0}", DbConnectionString));
            }
        }
    }
}
