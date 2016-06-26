using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fit;
using QaTip.Fitnesse.Demo.DoFixture.UiFxitures.DomainObject;

namespace QaTip.Fitnesse.Demo.DoFixture
{
    public class dbConnectionFixture:ColumnFixture
    {
        private  dbconnectionmap _connection = new dbconnectionmap();

        public override object GetTargetObject()
        {
            return _connection;
        }
    }

}
