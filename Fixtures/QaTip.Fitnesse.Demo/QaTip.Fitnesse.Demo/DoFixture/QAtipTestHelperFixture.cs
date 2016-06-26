using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;


namespace QaTip.Fitnesse.Demo.DoFixture
{
   public  class QAtipTestHelperFixture:testhelperFixture
    {
        public string QATIPInstallRoot { get; set; }

        public string qaDbConnectionString
        {
            get
            {
                XDocument webConfig = CoreHelpers.GetConfigFileAsXDoc(Path.Combine(QATIPInstallRoot, "Web.config"));
                return webConfig.Element("configuration").Element("connectionStrings").Element("add").Attribute("connectionString").Value;
            }

            set
            {
                XDocument webConfig = CoreHelpers.GetConfigFileAsXDoc(Path.Combine(QATIPInstallRoot, "Web.config"));
                webConfig.Element("configuration").Element("connectionStrings").Element("add").Attribute("connectionString").Value = value;
                webConfig.Save(Path.Combine(QATIPInstallRoot, "Web.config"));
            }
        }

        


    }
}
