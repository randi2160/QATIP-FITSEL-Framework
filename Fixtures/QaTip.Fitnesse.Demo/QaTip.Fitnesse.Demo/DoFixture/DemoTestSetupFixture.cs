﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fit;
using fitlibrary;
using fitSharp;


namespace QaTip.Fitnesse.Demo
{
   public class DemoTestSetupFixture:ColumnFixture
    {
       public string myname { get; set; }

       public string SayHello()
       {
         if(myname!=null)
         {

             return "Hello" + myname;
         }
         else
         {
             return "I dont know your name, Hello";
         }
       }
    }
}
