using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fit;
using System.IO;
using System.Diagnostics;
using QaTip.Fitnesse.Demo.DoFixture.UiFxitures.DomainObject;

namespace QaTip.Fitnesse.Demo.DoFixture
{
   public class ManageFilesAndDirectoriesFixture:ColumnFixture
    {
       private manageFilesandDirectoriesMap _pageread = new manageFilesandDirectoriesMap();

       public override object GetTargetObject()
       {
           return _pageread;
       }
    }
}
