using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;

using System.Data.OracleClient;
using System.Data.Odbc;
using System.Data.SqlClient;


namespace QaTip.Fitnesse.Demo.DoFixture
{
   public class DataAccess
    {
        public static string DbConnectionString { get; set; }
        public string connectionstringname { get; set; }
        protected static ArrayList ApplicationConnectionStrings;
        public static string username { get; set; }
        public static string password { get; set; }
        public static string datasource { get; set; }
        public static string status { get; set; }
        public static object sqlconnection { get; set; }

        public static string sqlservername { get; set; }

        public static string databasename { get; set; }

        OdbcConnection cmd = new OdbcConnection();
       // static OracleConnection con; 

         static DataAccess()
        { }


         protected static void SetDbConnectionStrings(string[] connStrings)
         {
           
            Configuration config = ConfigurationManager.OpenExeConfiguration(new Uri(Assembly.GetExecutingAssembly().EscapedCodeBase).AbsolutePath);
            string filePath = config.FilePath;
            Console.WriteLine("File path: {0}", filePath);

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


         public static DataTable ExecuteQuery(string commandText)
         {
             return ExecuteQuery(commandText, string.Empty);
         }

         public static DataTable ExecuteQuery(string commandText, string connectionString)
         {
             DataTable dataTable = new DataTable();

             using (var connection = new OracleConnection(string.IsNullOrEmpty(connectionString) ? DataAccess.DbConnectionString : connectionString))
             {
                 connection.Open();

                 using (var cmd = new OracleCommand())
                 {
                     cmd.Connection = connection;
                     cmd.CommandType = CommandType.Text;
                     cmd.CommandText = commandText;

                     using (var dr = cmd.ExecuteReader())
                     {
                         if (dr.HasRows)
                         {
                             dataTable.Load(dr, LoadOption.OverwriteChanges);
                         }
                     }
                 }
                 connection.Close();
             }

             return dataTable;
         }


         /// <summary>
         /// Gets a string representation of a single field data query 
         /// </summary>
         /// <param name="commandText">Sql query that should return a single field line of data</param>
         /// <param name="maxObjects"></param>
         /// <returns>String representation of the data field</returns>
         public static string GetSingleFieldData(string commandText)
         {
             return GetSingleFieldData(commandText, 8000);
         }

         /// <summary>
         /// Gets a string representation of a single field data query 
         /// </summary>
         /// <param name="commandText">Sql query that should return a single field line of data</param>
         /// <param name="maxObjects"></param>
         /// <returns>String representation of the data field</returns>
         public static string GetSingleFieldData(string commandText, int maxWaitMs)
         {
             //System.Diagnostics.Debugger.Launch();
             CoreHelpers.LogMessage("Using the sql query below to return an single data field value:");
             CoreHelpers.LogMessage(commandText);

             var maxWait = new TimeSpan(0, 0, 0, 0, maxWaitMs);
             var started = DateTime.Now;
             string fieldData = Constants.NOT_FOUND;

             using (var connection = new OracleConnection(DataAccess.DbConnectionString))
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
                             int recordsFound = 0;
                             while (dr.Read())
                             {
                                 recordsFound++;
                                 if (recordsFound > 1)
                                 {
                                     throw new ApplicationException("Found more than expected 1 record returned for query: " + commandText);
                                 }
                                 string typeName = dr.GetDataTypeName(0);
                                 fieldData = GetStringFromDataType(typeName, 0, dr);
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


         /// <summary>
         /// Gets a string representation of a single field data query 
         /// </summary>
         /// <param name="commandText">Sql query that should return a single field line of data</param>
         /// <param name="maxObjects"></param>
         /// <returns>String representation of the data field</returns>
        

         /// <summary>
         /// Gets a string representation of a single field data query 
         /// </summary>
         /// <param name="commandText">Sql query that should return a single field line of data</param>
         /// <param name="maxObjects"></param>
         /// <returns>String representation of the data field</returns>
        
         public static List<string> GetMultiRowSingleFieldData(string commandText)
         {
             return GetMultiRowSingleFieldData(commandText, true, 5000);
         }

         /// <summary>
         /// Gets a string List of rows of a single field data query 
         /// </summary>
         /// <param name="commandText">Sql query that should return a List of rows with a single field line of data</param>
         /// <param name="maxObjects"></param>
         /// <returns>String List representation of the data field</returns>
         public static List<string> GetMultiRowSingleFieldData(string commandText, bool logProgress, int maxWaitTime)
         {
             if (logProgress)
             {
                 CoreHelpers.LogMessage("Using the sql query below to return multiple rows of a single data field value:");
                 CoreHelpers.LogMessage(commandText);
             }

             var maxWait = new TimeSpan(0, 0, 0, 0, maxWaitTime);
             var started = DateTime.Now;
             List<string> fieldData = new List<string>();

             using (var connection = new OracleConnection(DataAccess.DbConnectionString))
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
                             int recordsFound = 0;
                             while (dr.Read())
                             {
                                 recordsFound++;
                                 string typeName = dr.GetDataTypeName(0);
                                 fieldData.Add(GetStringFromDataType(typeName, 0, dr));
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

         public static string GetStringFromDataType(string dataType, int index, OracleDataReader dr)
         {
             if (dr.IsDBNull(0))
             {
                 return null;
             }

             switch (dataType.ToLower())
             {
                 case "date":
                     return dr.GetDateTime(index).ToString();
                 case "decimal":
                     return dr.GetDecimal(index).ToString();
                 case "float":
                     return dr.GetFloat(index).ToString();
                 case "double":
                 case "single":
                     return dr.GetDouble(index).ToString();
                 case "int16":
                     return dr.GetInt16(index).ToString();
                 case "int32":
                     return dr.GetInt32(index).ToString();
                 case "int64":
                     return dr.GetInt64(index).ToString();
                
                 default:
                     return dr.GetString(index);
             }
         }

       //SQL
         public static string GetStringFromDataTypeSQL(string dataType, int index, SqlDataReader dr)
         {
             if (dr.IsDBNull(0))
             {
                 return null;
             }

             switch (dataType.ToLower())
             {
                 case "date":
                     return dr.GetDateTime(index).ToString();
                 case "decimal":
                     return dr.GetDecimal(index).ToString();
                 case "float":
                     return dr.GetFloat(index).ToString();
                 case "double":
                 case "single":
                     return dr.GetDouble(index).ToString();
                 case "int16":
                     return dr.GetInt16(index).ToString();
                 case "int32":
                     return dr.GetInt32(index).ToString();
                 case "int64":
                     return dr.GetInt64(index).ToString();

                 default:
                     return dr.GetString(index);
             }
         }



       /*

        public static void ConnectOracleDb()
         {
             con = new OracleConnection();
             con.ConnectionString = "User Id="+ " " +username + ";Password=" + password + ";Data Source= " + datasource + '"';
             con.Open();
             Console.WriteLine("Connected to Oracle" + con.ServerVersion);
         }

       


        public  static void Close()
         {
             con.Close();
             con.Dispose();
         }


        public static void connecttoOracleusingTNSnames()
        {

            var conn = new OdbcConnection();
            conn.ConnectionString =
                "Driver={Oracle ODBC Driver};" +
                "Dbq=" + datasource + ";" + '"' + // define in tsnames.ora
                "Uid=" + username + ";" + '"' +
                "Pwd=" + password + ";" + '"';
            conn.Open();
        }
        */
       public static object connectToSqlClient()
        {
           
           SqlConnection myConnection = new SqlConnection("Server=A5Q55S72\\SQLEXPRESS;Database=QATIP;Trusted_Connection=True;");
            myConnection.Open();
            status = myConnection.State.ToString();
            DataAccess.DbConnectionString = myConnection.ConnectionString;
            return DataAccess.DbConnectionString;
            //return myConnection;

        }
       


       public static string GetSingleFieldDataSql(string commandText, int maxWaitMs)
       {
           //System.Diagnostics.Debugger.Launch();
           CoreHelpers.LogMessage("Using the sql query below to return an single data field value:");
           CoreHelpers.LogMessage(commandText);

           var maxWait = new TimeSpan(0, 0, 0, 0, maxWaitMs);
           var started = DateTime.Now;
           string fieldData = Constants.NOT_FOUND;

           using (SqlConnection connection = new SqlConnection("Server=ACNU329C4BY\\SQLEXPRESS;Database=QATIP;Trusted_Connection=True;"))

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
                               fieldData = GetStringFromDataTypeSQL(typeName, 0, dr);
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

