using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fit;
using System.Data.OracleClient;
using System.Data;
using System.Threading;
using System.Data.SqlClient;
using QaTip.Fitnesse.Demo.DoFixture.DBFixtures.domainObject;


namespace QaTip.Fitnesse.Demo.DoFixture
{
   public class BaseDomainObject
    {
        /// <summary>
        /// Copy the reader row data into the object fields
        /// </summary>
        /// <param name="oracleDataReader">hot Oracle data reader with object data</param>
        protected virtual void PopulateFromReader(OracleDataReader oracleDataReader)
        {
        }


        public string customerid { get; set; }
        public string CustomerName { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string country { get; set; }
        public string Result { get; set; }
        public string status { get; set; }

        public void CopyTo(customer copyCustomer)
        {
            copyCustomer.customerid = this.customerid;
            copyCustomer.CustomerName = this.CustomerName;
            copyCustomer.ContactName = this.ContactName;
            copyCustomer.Address = this.Address;
            copyCustomer.City = this.City;
            copyCustomer.PostalCode = this.PostalCode;
            copyCustomer.country = this.country;
        }




        
         protected virtual void PopulatesqlFromReader(SqlDataReader SQLDataReader)
         {

         }
         
        public BaseDomainObject GetDbObject(string commandText)
        {
            return GetDbObjects(commandText, 1)[0];
        }

       //SQL
        public BaseDomainObject GetsqlDbObject(string commandText)
        {
            return GetsqlDbObjects(commandText, 1)[0];
        }

        /// <summary>
        /// Get object data from the database and load up the object.
        /// </summary>
        /// <param name="commandText">SQL SELECT statement to get data</param>
        /// <param name="connectionString">Specifies the connection string to use.  Used for specifying ODs o r RDM query location.</param>
        /// <returns>a List of the objects that matched the query.</returns>
        public BaseDomainObject GetDbObject(string commandText, string connectionString)
        {
            return GetDbObjects(commandText, 1, new TimeSpan(0, 0, 8), connectionString)[0];
        }

        public BaseDomainObject GetsqlDbObject(string commandText, string connectionString)
        {
            return GetsqlDbObjects(commandText, 1, new TimeSpan(0, 0, 8), connectionString)[0];
        }

        /// <summary>
        /// Get object data from the database and load up the object.
        /// </summary>
        /// <param name="commandText">SQL SELECT statement to get data</param>
        /// <param name="maxObjects">The maximum number of objects expected</param>
        /// <returns>a List of the objects that matched the query.</returns>
        public List<BaseDomainObject> GetDbObjects(string commandText, int maxObjects)
        {
            int maxWaitSeconds = 8;
            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["GetDbObjectsMaxWaitSec"]))
            {
                maxWaitSeconds = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["GetDbObjectsMaxWaitSec"]);
            }

            return GetDbObjects(commandText, maxObjects, new TimeSpan(0, 0, maxWaitSeconds));
        }


       //SQL
        public List<BaseDomainObject> GetsqlDbObjects(string commandText, int maxObjects)
        {
            int maxWaitSeconds = 8;
            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["GetDbObjectsMaxWaitSec"]))
            {
                maxWaitSeconds = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["GetDbObjectsMaxWaitSec"]);
            }

            return GetsqlDbObjects(commandText, maxObjects, new TimeSpan(0, 0, maxWaitSeconds));
        }

        /// <summary>
        /// Get object data from the database and load up the object.
        /// </summary>
        /// <param name="commandText">SQL SELECT statement to get data</param>
        /// <param name="maxObjects">The maximum number of objects expected</param>
        /// <returns>a List of the objects that matched the query.</returns>
        public List<BaseDomainObject> GetDbObjects(string commandText, int maxObjects, TimeSpan maxWait)
        {
            return GetDbObjects(commandText, maxObjects, maxWait, DataAccess.DbConnectionString);
        }


       //sql

        public List<BaseDomainObject> GetsqlDbObjects(string commandText, int maxObjects, TimeSpan maxWait)
        {
            return GetsqlDbObjects(commandText, maxObjects, maxWait, DataAccess.DbConnectionString);
        }
        /// <summary>
        /// Get object data from the database and load up the object.
        /// </summary>
        /// <param name="commandText">SQL SELECT statement to get data</param>
        /// <param name="maxObjects">The maximum number of objects expected</param>
        /// <returns>a List of the objects that matched the query.</returns>
        ///  public List<BaseDomainObject> GetDbObjects(string commandText, int maxObjects, TimeSpan maxWait)
        public List<BaseDomainObject> GetDbObjects(string commandText, int maxObjects, TimeSpan maxWait, string connectionString)
        {
            var started = DateTime.Now;
            var objectList = new List<BaseDomainObject>();

            using (var connection = new OracleConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new OracleCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = commandText;

                    while (true)
                    {
                        using (var dr = cmd.ExecuteReader())
                        {
                            var recordsFound = 0;
                            while (dr.Read())
                            {
                                CoreHelpers.LogMessage(string.Format("{0} Query returned results.", DateTime.Now.ToString()));
                                recordsFound++;
                                if (recordsFound > maxObjects)
                                {
                                    throw new ApplicationException("Found more than expected" + maxObjects + " records return for query: " + commandText);
                                }
                                var domObject = (BaseDomainObject)this.GetType().Assembly.CreateInstance(this.GetType().FullName);

                                domObject.PopulateFromReader(dr);
                                objectList.Add(domObject);
                            }
                            if (recordsFound == 0 && DateTime.Now - started > maxWait)
                            {
                                connection.Close();

                                throw new ApplicationException(String.Format("No records found in db matching {0} afer {1} seconds.", commandText, maxWait.Seconds));
                            }
                            if (recordsFound > 0)
                                break;

                            Thread.Sleep(500);
                        }
                    }
                }
                connection.Close();
            }
            return objectList;
        }


        /*

         //move copy over here:
          protected void PopulateFromReader(SqlDataReader sqlDataReader)
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
                      case "Address":
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

                      default:
                          throw new ApplicationException(string.Format("Encountered a Database field that is not recognized: {0}", sqlDataReader.GetName(i)));
                  }
              }
          }*/


       

        //sql
        public List<BaseDomainObject> GetsqlDbObjects(string commandText, int maxObjects, TimeSpan maxWait, string connectionString)
        {
            var started = DateTime.Now;
            var objectList = new List<BaseDomainObject>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = commandText;

                    while (true)
                    {
                        using (var dr = cmd.ExecuteReader())
                        {
                            var recordsFound = 0;
                            while (dr.Read())
                            {
                                CoreHelpers.LogMessage(string.Format("{0} Query returned results.", DateTime.Now.ToString()));
                                recordsFound++;
                                if (recordsFound > maxObjects)
                                {
                                    throw new ApplicationException("Found more than expected" + maxObjects + " records return for query: " + commandText);
                                }
                                var domObject = (BaseDomainObject)this.GetType().Assembly.CreateInstance(this.GetType().FullName);

                                domObject.PopulatesqlFromReader(dr);
                                objectList.Add(domObject);
                            }
                            if (recordsFound == 0)
                            {
                                connection.Close();

                                throw new ApplicationException(String.Format("No records found in db matching {0} afer {1} seconds.", commandText));
                            }
                            if (recordsFound > 0)
                                break;

                            Thread.Sleep(500);
                        }
                    }
                }
                connection.Close();
            }
            return objectList;
        }



    }
}
