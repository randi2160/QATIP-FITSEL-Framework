using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fit;
using QaTip.Fitnesse.Demo.DoFixture;
using System.Collections;
using System.Data.OracleClient;
using System.Diagnostics;
using System.IO;
using System.Data.SqlClient;
using System.Data;


namespace QaTip.Fitnesse.Demo.DoFixture
{
    public class runsqlFixture:ColumnFixture
    {
        private int _affectedRowCount;
        private string _dbConnectionString = DataAccess.DbConnectionString;
        private string _configString;
        private ArrayList items = new ArrayList();

       
        public string ItemValue
        { get; set; }
        public string ItemIndex
        { get; set; }

        public string AllItems
        {
            get;
            set;
        }
        /// <summary>
        /// Supply raw sql text
        /// </summary>
        public string Sql
        { get; set; }

        /// <summary>
        /// Supply script name 
        /// </summary>
        public string SqlFile
        { get; set; }

        /// <summary>
        /// Supply raw sql text that is fetching potential records
        /// </summary>
        public string RecordSql
        { get; set; }

        /// <summary>
        /// Supply sql that is a query for building a list of data
        /// </summary>
        public string ListSql
        { get; set; }       

        /// <summary>
        /// Get record count for provided sql string 
        /// </summary>
        public string GetRecordCount
        {
            get
            {
                return _affectedRowCount.ToString();
            }
        }


        /// <summary>
        /// Get the item on the list based on the itemIndex property
        /// </summary>
        public string GetListItemValue
        {
            get
            {
                if (ItemIndex != null)
                {
                    if (items.Count == 0)
                    {
                        return "No items were returned by the provided query";
                    }

                    int index = Convert.ToInt16(ItemIndex);
                    if(items.Count < index)
                    {
                        return "The item list returned by the provided query is shorter than the requested index";
                    }

                    items.Sort();
                    return items[index - 1].ToString();
                }
                return null;
            }
        }

        public string GetReturnedSingleField
        {
            get
            {
                if (items.Count == 0)
                {
                    return "No items were returned by the provided query";
                }
                return items[0].ToString();
            }
        }

        /// <summary>
        /// Sets the DB connection string to ODS or RDM
        /// </summary>
        public string DbConnection
        {
            get
            {
                return _dbConnectionString;                                                
            }

            set
            {
                _dbConnectionString = value;
            }
        }

        public string SessionConfig
        {
            set
            {
                _configString = value;
            }
        }

        public void runsqlcommand()
        {
            DataAccess.GetSingleFieldDataSql(Sql, 30);
        }


        /// <summary>
        /// Runs a Sql command(s) and/or Sql script
        /// </summary>
        public override void Execute()
        {
            // First run any SqlScript
            if (SqlFile != null)
            {
                RunSqlFile();
            }

            // Then run and sql commands
            if (Sql != null)
            {
                RunSql();
            }

            // Run RecordSql to get a record count
            if (RecordSql != null)
            {
                RunRecordSql();
            }

            // Run RecordSql to get a record count, build AllItems, and populate items
            if (ListSql != null)
            {
                RunListSql();
            }
        }
       
        /// <summary>
        /// Runs a given Sql script via run_sql.bat
        /// </summary>
        private void RunSqlFile()
        {              
            // input is a sql file name
            string sqlFileName = Environment.CurrentDirectory + @"\FitNesseRoot\SqlFiles\" + SqlFile;
            if (!File.Exists(sqlFileName))
            {
                throw new ApplicationException(string.Format("Cannot find '{0}'", sqlFileName));
            }

            // Example script arguments: 
            string[] dbInfo = _dbConnectionString.Split(new char[] { ';' });
            string dataSource = dbInfo[0].Replace("Data Source=", string.Empty);
            string userId = dbInfo[2].Replace(" User ID=", string.Empty);
            string password = dbInfo[3].Replace(" Password=", string.Empty);
                       
            ProcessStartInfo startInfo = new ProcessStartInfo(Environment.CurrentDirectory + @"\FitNesseRoot\SqlFiles\run_sql.bat");
            startInfo.WorkingDirectory = Environment.CurrentDirectory + @"\FitNesseRoot\SqlFiles";
            startInfo.Arguments = string.Format("{0} {1} {2} {3}", userId, password, dataSource, SqlFile);
            
            Process sqlScriptRunnerProcess = new Process();
            sqlScriptRunnerProcess.StartInfo = startInfo;            

            CoreHelpers.LogMessage(string.Format("Running: run_sql.bat {0}", startInfo.Arguments));

            sqlScriptRunnerProcess.Start();
            sqlScriptRunnerProcess.WaitForExit(20000);
            sqlScriptRunnerProcess.Close();

            SqlFile = null;
        }

        /// <summary>
        /// Runs a given set of Sql.  Multiple commands must be separated with ';' 
        /// </summary>
        private void RunSql()
        {           
            string[] commands = Sql.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string command in commands)
            {
                CoreHelpers.LogMessage(string.Format("Executing: {0}", command));
                _affectedRowCount = OracleHelper.ExecuteNonQuery(_dbConnectionString, System.Data.CommandType.Text, command);
            }
            Sql = null;
        }

        /// <summary>
        /// Runs a given set of Sql for the purpose of getting the returned list
        /// </summary>
        private void RunListSql()
        {
            DateTime started = DateTime.Now;
            TimeSpan maxWait = new TimeSpan(0, 1, 0);
            int _returnedRowCount = 0;

            items = new ArrayList();
            AllItems = null;

            OracleConnection connection = new OracleConnection(_dbConnectionString);
            OracleDataReader reader;

            if (_configString != null)
            {
                OracleCommand oConfigCommand = new OracleCommand(_configString, connection);
                connection.Open();
                oConfigCommand.ExecuteNonQuery();
                OracleCommand oCommand = new OracleCommand(ListSql, connection);
                reader = oCommand.ExecuteReader();
            }
            else
            {
                OracleCommand oCommand = new OracleCommand(ListSql, connection);
                connection.Open();
                reader = oCommand.ExecuteReader();
            }
            try
            {
                while (reader.Read())
                {
                    _returnedRowCount++;

                    string sValue = DataAccess.GetStringFromDataType(reader.GetDataTypeName(0), 0, reader);
                    items.Add(sValue);

                    AllItems += sValue + " ";

                    // Want to make sure we don't get stuck here, so use a default timeout
                    if (DateTime.Now - started > maxWait)
                    {
                        connection.Close();
                        throw new ApplicationException(String.Format("Times out collecting records for this query: {0} after {1} seconds.", ListSql, maxWait.Seconds));
                    }

                }
                if (AllItems != null)
                {
                    // remove final space
                    AllItems = AllItems.Substring(0, AllItems.Length - 1);
                }
            }
            finally
            {
                reader.Close();
            }


            _affectedRowCount = _returnedRowCount;
            ListSql = null;

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

        /// <summary>
        /// Runs a given set of Sql for the purpose of determining how many records were returned 
        /// </summary>
        private void RunRecordSql()
        {
            _affectedRowCount = CoreHelpers.GetRecordCountForQuery(RecordSql, _dbConnectionString);               
        } 
        
    }
}
