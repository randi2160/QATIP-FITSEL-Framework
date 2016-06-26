using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fit;
using QaTip.Fitnesse.Demo.DoFixture.UiFxitures.DomainObject;
using QaTip.Fitnesse.Demo.DoFixture.DBFixtures.domainObject;


namespace QaTip.Fitnesse.Demo.DoFixture.DBFixtures
{
    public class ValiateCustomerInfoTableFixture:ColumnFixture
    {
        private customerobject _customerobj = new customerobject();


        public override object GetTargetObject()
        {
            return _customerobj;
        }
    }
}
