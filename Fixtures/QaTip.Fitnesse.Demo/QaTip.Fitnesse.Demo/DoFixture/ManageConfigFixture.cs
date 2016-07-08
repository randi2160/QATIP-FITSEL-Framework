using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fit;
using QaTip.Fitnesse.Demo.DoFixture.DomainObject;

namespace QaTip.Fitnesse.Demo.DoFixture
{
  public  class ManageConfigFixture: ColumnFixture
    {
        private ManageConfigMap _manageConfig = new ManageConfigMap();
        public override object GetTargetObject()
        {
            return _manageConfig;
        }
    }
}
