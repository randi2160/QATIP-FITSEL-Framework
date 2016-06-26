using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fit;
using System.Data.Odbc;
//using System.Data.OracleClient;
using System.Data;
using Oracle.DataAccess;
using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;


namespace QaTip.Fitnesse.Demo.DoFixture.UiFxitures.DomainObject
{
    public class dbconnectionmap:ColumnFixture
    {
        public static string username { get; set; }
        public static string password { get; set; }
        public static string datasource { get; set; }
        public string command { get; set; }
        public string status { get; set; }
        public string results { get; set; }
        public object sqlconnection { get; set; }
       public static int maxWaitMs { get; set; }
        public static string sqlservername { get; set; }

        public static string databasename { get; set; }
        protected static ArrayList ApplicationConnectionStrings;
        OdbcConnection cmd = new OdbcConnection();
        OracleConnection con;

        public  string ConnectOracleDb()
        {
            con = new OracleConnection();
            con.ConnectionString = "Data Source=" + datasource + "; Persist Security Info=True; User ID=" + username +";" +"Password=" + password ;
            con.Open();
            Console.WriteLine("Connected to Oracle" + con.ServerVersion);
            DataAccess.DbConnectionString = con.ConnectionString;
            return DataAccess.DbConnectionString;
        }

        public  void Close()
        {
            con.Close();
            con.Dispose();
        }


        //set db connection string
        protected static void SetDbConnectionStrings(string[] connStrings)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(new Uri(System.Reflection.Assembly.GetExecutingAssembly().EscapedCodeBase).AbsolutePath);

            if (config == null)
            {
                throw new ApplicationException("config was null! There must be a problem finding Machine.config");
            }

            if (config.ConnectionStrings == null)
            {
                throw new ApplicationException("config.ConnectionStrings was null!  The current Machine.config does not specify any connection strings.");
            }

            ApplicationConnectionStrings = new ArrayList();

            foreach (string connString in connStrings)
            {
                var connectionStringSettings = config.ConnectionStrings.ConnectionStrings[connString];
                if (connectionStringSettings != null)
                {
                    ApplicationConnectionStrings.Add(connectionStringSettings.ConnectionString);
                    CoreHelpers.LogMessage(string.Format("Found ConnectionString: {0}", connString));
                }
            }
        }


        public void connectToSQLclient()
        {
            // DataAccess.connectToSqlClient();
            //SqlConnection conn = new SqlConnection(sqlservername + ";Database=" + databasename + "; Trusted_Connection=True;");

            SqlConnection conn = new SqlConnection("Server=A5Q55S72\\SQLEXPRESS;Database=QATIP;Trusted_Connection=True;");
           SqlDataReader rdr = null;
           try
           {
               // 2. Open the connection
               conn.Open();
               status = conn.State.ToString();
               // 3. Pass the connection to a command object
               SqlCommand cmd = new SqlCommand("select * from Customers", conn);

               //
               // 4. Use the connection
               //

               // get query results
               rdr = cmd.ExecuteReader();

               // print the CustomerID of each record
               while (rdr.Read())
               {
                   int count = rdr.FieldCount;
                   for (int i = 0; i < count; i++ )
                   {
                       Console.WriteLine(rdr[i]);
                       results = rdr[i].ToString();
                   }
                       
               }
           }
           finally
           {
               // close the reader
               if (rdr != null)
               {
                   rdr.Close();
               }

               // 5. Close the connection
               if (conn != null)
               {
                   conn.Close();
               }
           }        
           
        }

        

        public static string GetSingleFieldDataSql(string commandText)
        {
           
            CoreHelpers.LogMessage("Using the sql query below to return an single data field value:");
            CoreHelpers.LogMessage(commandText);

            var maxWait = new TimeSpan(0, 0, 0, 0, maxWaitMs);
            var started = DateTime.Now;
            string fieldData = Constants.NOT_FOUND;

            using (SqlConnection connection = new SqlConnection("Server=A5Q55S72\\SQLEXPRESS;Database=QATIP;Trusted_Connection=True;"))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = commandText;

                    while (true)
                    {
                        using (var dr = cmd.ExecuteReader())
                        {
                            int recordsFound = 0;
                            while (dr.Read())
                            {
                                recordsFound++;
                                if (recordsFound > 1)
                                {
                                    throw new ApplicationException("Found more than expected 1 record returned for query: " + commandText);
                                }
                                string typeName = dr.GetDataTypeName(0);
                                fieldData = DataAccess.GetStringFromDataTypeSQL(typeName, 0, dr);
                            }

                            if (recordsFound > 0)
                            {
                                break;
                            }

                            if (recordsFound == 0 && DateTime.Now - started > maxWait)
                            {
                                break;
                            }

                            System.Threading.Thread.Sleep(500);
                        }
                    }
                }
                connection.Close();
            }
            return fieldData;
        }


       
       

    }
}
