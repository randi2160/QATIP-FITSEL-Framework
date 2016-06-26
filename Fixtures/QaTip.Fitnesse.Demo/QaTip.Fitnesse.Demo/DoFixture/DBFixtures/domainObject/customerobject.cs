using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fit;
using QaTip.Fitnesse.Demo.DoFixture.UiFxitures.DomainObject;


namespace QaTip.Fitnesse.Demo.DoFixture.DBFixtures.domainObject
{
   public class customerobject:ColumnFixture
    {
        public string customerid { get; set; }
        public string CustomerName { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string country { get; set; }
        public string Result { get; set; }
        public string status { get; set; }
    }
}
