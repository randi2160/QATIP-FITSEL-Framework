using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using fit;
using OpenQA.Selenium.Chrome;
using QaTip.Fitnesse.Demo.DoFixture.UiFxitures.DomainObject;
using QaTip.Fitnesse.Demo.DoFixture.DBFixtures;
using System.Configuration;

namespace QaTip.Fitnesse.Demo.DoFixture
{
   public  class testhelperFixture:ColumnFixture
    {
        /// <summary>
        /// FileName.  Used in BuildFile
        /// </summary>
        public string FileName
        { get; set; }

        /// <summary>
        /// FileContent.  Used in BuildFile
        /// </summary>
        public string FileContent
        { get; set; }

        /// <summary>
        /// WaitSeconds.  Used in Wait()
        /// Number of seconds to sleep the thread
        /// </summary>
        public string WaitSeconds
        { get; set; }

        /// <summary>
        /// EnvironmentalVariable.  Used in GetEnvironmentalVariable and SetEnvironmentalVariable().        
        /// </summary>
        public string EnvironmentVariable
        { get; set; }

        /// <summary>
        /// EnvironmentalVariableValue.  Used in SetEnvironmentalVariable().        
        /// </summary>
        public string EnvironmentVariableValue
        { get; set; }

        /// <summary>
        /// FitNesse connection string
        /// </summary>
        /// 
       
        public string FitNesseDbConnectionString
        {
            get
            {
                return DataAccess.DbConnectionString;
            }

            set
            {
                
                DataAccess.DbConnectionString = value;
                CoreHelpers.LogMessage(string.Format("Changing FitNesse connection string to: {0}", value));
            }
        }



        public string GetFitNesseDbConnectionString
        {
            get
            {
                return qatipDataAccess.DbConnectionString;
            }

            set
            {
                DataAccess.DbConnectionString = value;
                CoreHelpers.LogMessage(string.Format("Changing FitNesse connection string to: {0}", value));
            }
        }

        /// <summary>
        /// Sleeps the test thread the given number of seconds as determined by WaitSeconds        
        /// </summary>
        public string Wait()
        {
            int sleepMs = WaitSeconds == null ? 0 : Int32.Parse(WaitSeconds) * 1000;
            System.Threading.Thread.Sleep(sleepMs);
            return "Slept " + sleepMs / 1000 + " seconds";
        }

        public void runquery()
        {
            
           
           
        }

        /// <summary>
        /// Gets the value of a System Environment variable as defined by EnvironmentVariable        
        /// </summary>
        public string GetSystemEnvironmentVariable
        {
            get
            {
                if (EnvironmentVariable == null)
                {
                    throw new ApplicationException("Need to specify the EnvironmentalVariable property to execute GetEnvironmentalVariable");
                }

                CoreHelpers.LogMessage(string.Format("Getting the {0} environment variable", EnvironmentVariable));
                return Environment.GetEnvironmentVariable(EnvironmentVariable, EnvironmentVariableTarget.Machine);
            }
        }

        /// <summary>
        /// Sets the value of a System Environment variable as defined by EnvironmentVariable, EnvironmentVariableValue        
        /// </summary>
        public void SetSystemEnvironmentVariable()
        {
            if (EnvironmentVariable == null)
            {
                throw new ApplicationException("Need to specify the EnvironmentVariable property to execute SetEnvironmentVariable");
            }

            if (EnvironmentVariableValue == null)
            {
                throw new ApplicationException("Need to specify the EnvironmentVariableValue property to execute SetEnvironmentVariable");
            }

            CoreHelpers.LogMessage(string.Format("Setting the {0} environment variable to {1}", EnvironmentVariable, EnvironmentVariableValue));
            System.Environment.SetEnvironmentVariable(EnvironmentVariable, EnvironmentVariableValue, EnvironmentVariableTarget.Machine);
        }

        /// <summary>
        /// Builds a text file with content as specified by FileContent and FileName      
        /// </summary>
        public void BuildFile()
        {
            if (FileName == null)
            {
                throw new ApplicationException("Need to specify the FileName property to execute BuildFile");
            }

            if (FileContent == null)
            {
                throw new ApplicationException("Need to specify the FileContent property to execute BuildFile");
            }

            FileStream fileStream = null;
            if (File.Exists(FileName))
            {
                fileStream = new FileStream(FileName, FileMode.Append, FileAccess.Write);
            }
            else
            {
                fileStream = new FileStream(FileName, FileMode.Create, FileAccess.Write);
            }

            StreamWriter sw = new StreamWriter(fileStream);
            sw.WriteLine(FileContent);
            sw.Close();
            fileStream.Close();
        }

        /// <summary>
        /// Restarts iis      
        /// </summary>
        public void ResetIIS()
        {
            Process iisreset = new Process();
            iisreset.StartInfo.FileName = "iisreset.exe";
            iisreset.StartInfo.Arguments = "localhost";
            iisreset.Start();
            iisreset.WaitForExit(120000);
        }


        // Reading Machine Config files

        // Access the machine configuration file using mapping.
        // The function uses the OpenMappedMachineConfiguration 
        // method to access the machine configuration. 
        public static void MapMachineConfiguration()
        {
          
            // Get the machine.config file.
            Configuration machineConfig =
              ConfigurationManager.OpenMachineConfiguration();
            // Get the machine.config file path.
            ConfigurationFileMap configFile =
              new ConfigurationFileMap(machineConfig.FilePath);

            // Map the application configuration file to the machine 
            // configuration file.
            Configuration config =
              ConfigurationManager.OpenMappedMachineConfiguration(
                configFile);

            // Get the AppSettings section.
            AppSettingsSection appSettingSection =
              (AppSettingsSection)config.GetSection("appSettings");
            appSettingSection.SectionInformation.AllowExeDefinition =
                ConfigurationAllowExeDefinition.MachineToRoamingUser;


            // Display connection String
            
           

            // Display the configuration file sections.
            ConfigurationSectionCollection sections =
              config.Sections;

            Console.WriteLine();
            Console.WriteLine("Using OpenMappedMachineConfiguration.");
            Console.WriteLine("Sections in machine.config:");

            // Get the sections in the machine.config.
            foreach (ConfigurationSection section in sections)
            {
                string name = section.SectionInformation.Name;
                Console.WriteLine("Name: {0}", name);
            }

        }

    }
}
