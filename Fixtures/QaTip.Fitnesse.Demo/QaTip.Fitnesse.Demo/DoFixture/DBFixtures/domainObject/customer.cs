using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using QaTip.Fitnesse.Demo.DoFixture.UiFxitures.DomainObject;


namespace QaTip.Fitnesse.Demo.DoFixture.DBFixtures.domainObject
{
    public class customer:BaseDomainObject
    {
        public new string customerid { get; set; }
        public new string CustomerName { get; set; }
        public new string ContactName { get; set; }
        public new string Address { get; set; }
        public new string City { get; set; }
        public  string Salary { get; set; }
        public new string PostalCode { get; set; }
        public new string country { get; set; }
        public new string Result { get; set; }
        public new string status { get; set; }
        //Need empty constructor for BaseDomainObject.GetDbObjects()
        //public customer {}

   

        public customer()
        {

        }
        #region Public Copy
        /// <CrossReferenceDefiniton>
        /// Build Copy Method return a copy of an object without any proxy objects associated with it
        /// </CrossReferenceDefiniton>        
        ///
        public new void CopyTo(customer copyCustomer)
        {
            copyCustomer.customerid = this.customerid;
            copyCustomer.CustomerName = this.CustomerName;
            copyCustomer.ContactName = this.ContactName;
            copyCustomer.Address = this.Address;
            copyCustomer.City = this.City;
            copyCustomer.PostalCode = this.PostalCode;
            copyCustomer.country = this.country;
            copyCustomer.Salary = this.Salary;
        }



        #endregion
        //move copy over here:
        protected override void PopulatesqlFromReader(SqlDataReader sqlDataReader)
        {
            for (int i = 0; i < sqlDataReader.FieldCount; i++)
            {
                if (sqlDataReader.IsDBNull(i))
                {
                    continue;
                }
                switch (sqlDataReader.GetName(i))
                {
                    case "customerid":
                        customerid = sqlDataReader.GetSqlValue(i).ToString();
                        break;
                    case "CustomerName":
                        CustomerName = sqlDataReader.GetString(i);
                        break;
                    case "ContactName":
                        ContactName = sqlDataReader.GetString(i);
                        break;
                    case "ADDRESS":
                        Address = sqlDataReader.GetString(i);
                        break;
                    case "City":
                        City = sqlDataReader.GetString(i);
                        break;
                    case "PostalCode":
                        PostalCode = sqlDataReader.GetString(i);
                        break;
                    case "Country":
                        country = sqlDataReader.GetString(i);
                        break;
                    case "SALARY":
                        Salary = sqlDataReader.GetSqlValue(i).ToString();
                        break;

                    default:
                        throw new ApplicationException(string.Format("Encountered a Database field that is not recognized: {0}", sqlDataReader.GetName(i)));
                }
            }
        }



        public List<customer> GetCustomerData(int maxRows, string tableName, string whereClause)
        {
            DataAccess.connectToSqlClient();
            string connectionstring = DataAccess.DbConnectionString;
            string commandText = string.Format(Constants.SELECT_FROM_TABLE_WITH_WHERECLAUSE, tableName, whereClause);
            CoreHelpers.LogMessage(string.Format("Customers Sql query: {0}", commandText));
            return GetCrossReferenceRecords(maxRows, commandText);
        }

       


        





        private List<customer> GetCrossReferenceRecords(int maxRows, string commandText)
        {
        
            List<customer> records = new List<customer>();
            try
            {
                List<BaseDomainObject> results = GetsqlDbObjects(commandText, maxRows);
                foreach (var a in results)
                {
                    customer ce = a as customer;
                   /*
                    ce.Result = Constants.SUCCESS;
                    ce.Address = a.Address;
                    ce.City = a.City;
                   ce.CustomerName = a.CustomerName;
                    ce.ContactName = a.ContactName;
                   ce.PostalCode = a.PostalCode;
                     */
                    
                    records.Add(ce);
                }
            }
            catch (Exception e)
            {
                customer ce = new customer();
                ce.Result = Constants.NOT_FOUND;
                records.Add(ce);
            }
            return records;
        }


    }
}
