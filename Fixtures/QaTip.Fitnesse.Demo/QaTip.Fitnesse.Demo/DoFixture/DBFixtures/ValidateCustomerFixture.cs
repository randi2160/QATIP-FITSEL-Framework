using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fit;
using QaTip.Fitnesse.Demo.DoFixture.DBFixtures.domainObject;

namespace QaTip.Fitnesse.Demo.DoFixture.DBFixtures
{
    public class ValidateCustomerFixture :RowFixture
    {
       
        private customer _customer = new customer();
        private List<customer> _CustomerData = new List<customer>();
        private int _maxRows = 50;
        private string _tablename = string.Empty;
        private string _messageForTable = "Please enter valid FROM-clause or where-clause to retrieve data.";
        public override Type GetTargetClass()
        {
            return typeof(customer);
        }


        public override object[] Query()
        {
           
            //Args[0] holds table name; 
            //Args[1] holds maximum number of records to display data in grid or leave it as blank to use default number 50 if not sure; 
            //Args[2] holds process log id or where clause string or file name
            string queryObject = String.Empty;
            try
            {
                if (!String.IsNullOrEmpty(Args[1]))
                {
                    _maxRows = (int)GetArgumentInput(1, typeof(int));
                }

                _tablename = (string)GetArgumentInput(0, typeof(string));

                switch (Args[0].Trim())
                {
                    case "Customers":
                        if (!String.IsNullOrEmpty(Args[0].Trim()))
                        {

                            _CustomerData = _customer.GetCustomerData(_maxRows, _tablename, Args[2].Trim());
                        }
                        else
                        {
                            _CustomerData = _customer.GetCustomerData(_maxRows, _tablename, "");
                        }
                        SetMessageForCategoryEdit(_messageForTable);
                        return _CustomerData.ToArray();
                    default:
                        SetMessageForCategoryEdit("Unknown argument value for this fixture " + Args[0]);
                        break;
                }
            }
            catch (Exception e)
            {
                CoreHelpers.LogMessage(string.Format("Cross Reference Edits Error: {0}", e.Message.ToString()));
            }
            return _CustomerData.ToArray();
        }


        private void SetMessageForCategoryEdit(string msg)
        {
            if (Args[2].Length == 0 || Args[2] == null)
            {
                _customer.Result = msg;
                _CustomerData.Add(_customer);
            }
        }

    }
}
