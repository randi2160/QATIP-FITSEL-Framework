using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using fit;
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace QaTip.Fitnesse.Demo.DoFixture.DomainObject
{
    public class ManageConfigMap:ColumnFixture
    {
        /// Field used for holding various values to use when updating config file data
        /// </summary>
        public string _configValue;
        private string _Attribute;

        public string Attribute
        {
            get { return _Attribute; }
            set { _Attribute = value; }
        }

        public ManageConfigMap()
        { }

        public string ConfigFileName
        {
            get { return _configValue; ; }
            set { _configValue = value; }
        }


        public string ConfigValue
        { get; set; }

        public string TargetValue { get; set; }
        public object ServiceName { get; private set; }



        #region

        /// <summary>
        /// gets config settings from appsettings section
        /// </summary>
        /// <returns></returns>
        public string GetConfigSetting()
        {

            string ConfigPath = ConfigFileName;
            return _getConfigAppSetting(ConfigPath);
        }



        /// <summary>
        /// seta config settings in appsettings section
        /// </summary>
        public void SetConfigSetting()
        {
            string ConfigPath = ConfigFileName;
            _setConfigAppSetting(ConfigPath);
        }
        /// <summary>
        /// Writes the contents of TargetValue into the file specified by ConfigFileName
        /// </summary>
        public void SetConfigFile()
        {
            File.WriteAllText(ConfigFileName, TargetValue);
        }
        /// <summary>
        /// Finds the xml node to update via XPATH and Writes the contents of TargetValue into the file specified by ConfigFileName
        /// </summary>
        public void SetXPATHConfigValueToTargetValue()
        {
            var doc = new XmlDocument();
            doc.Load(ConfigFileName);
            var nav = doc.CreateNavigator();
            var expr = nav.Compile(ConfigValue);
            var node = nav.SelectSingleNode(expr);
            node.SetValue(TargetValue);
            doc.Save(ConfigFileName);
        }

        /// <summary>
        /// sets config settings in protectedConfig section
        /// </summary>
        public void SetConfigForProtectedConfigSection()
        {
            string ConfigPath = ConfigFileName;
            _setProtectedConfigSection(ConfigPath);
        }

        /// <summary>
        /// gets config settings from protectedConfig section
        /// </summary>
        public string GetConfigForProtectedConfigSection()
        {
            string ConfigPath = ConfigFileName;
            return _getProtectedConfigSection(ConfigPath);
        }


        /// <summary>
        /// set config settings in connection section
        /// </summary>
        public void SetConnectionSetting()
        {
            string ConfigPath = ConfigFileName;
            _setConfigConnectionSetting(ConfigPath);
        }

        /// <summary>
        /// set config settings in connection section
        /// </summary>
        public string GetConnectionSetting()
        {
            return _getConfigConnectionSetting(ConfigFileName);
        }

        /// <summary>
        /// gets log file location from log4net section
        /// </summary>
        /// <returns></returns>
        public string GetConfigSettingForLog4netSection()
        {
            string ConfigPath = ConfigFileName;
            return _getConfigAppSettingLog4Net(ConfigPath);
        }

        /// <summary>
        /// sets log file location in log4net section
        /// </summary>
        public void SetConfigSettingForLog4netSection()
        {
            string ConfigPath = ConfigFileName;
            _setConfigAppSettinglog4net(ConfigPath);
        }

        private string _getConfigAppSettingLog4Net(string configPath)
        {
            XmlDocument xmlDocument = new XmlDocument();
            using (var fs = new FileStream(configPath, FileMode.Open))
            {
                xmlDocument.Load(fs);
            }

            XmlNode node = xmlDocument.SelectSingleNode("/configuration/log4net/appender/file");
            string logPathString = node.Attributes["value"].Value;

            if (!String.IsNullOrWhiteSpace(logPathString))
            {
                return logPathString;
            }
            else
            {
                return string.Format("Could not find '{0}' AppSetting in {1}", ConfigValue, configPath);
            }
        }

        private string _getConfigAppSetting(string configPath)
        {
            XmlDocument xmlDocument = new XmlDocument();
            using (var fs = new FileStream(configPath, FileMode.Open))
            {
                xmlDocument.Load(fs);
            }

            XmlNodeList nodes = xmlDocument.SelectNodes("/configuration/appSettings/add");
            foreach (XmlNode node in nodes)
            {
                if (node.Attributes["key"].Value.ToLower() == ConfigValue.ToLower())
                {
                    return node.Attributes["value"].Value;
                }
            }

            return string.Format("Could not find '{0}' AppSetting in {1}", ConfigValue, configPath);
        }

        private void _setConfigAppSettinglog4net(string configPath)
        {
            XmlDocument xmlDocument = new XmlDocument();
            using (var fs = new FileStream(configPath, FileMode.Open))
            {
                xmlDocument.Load(fs);
            }
            XmlNode targetNode = xmlDocument.SelectSingleNode("/configuration/log4net/appender/file");

            if (targetNode != null)
            {
                targetNode.Attributes["value"].Value = TargetValue == null ? string.Empty : TargetValue;
                using (FileStream fs = new FileStream(configPath, FileMode.Create, FileAccess.Write))
                {
                    xmlDocument.Save(fs);
                }
            }
            else
            {
                throw new ApplicationException(string.Format("Could not find the specified app setting of '{0}' in {1}", ConfigValue, configPath));
            }
        }

        private void _setConfigAppSetting(string configPath)
        {
            //ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            //configFileMap.ExeConfigFilename = configPath;
            //Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            //config.AppSettings.Settings[ConfigValue].Value = TargetValue;
            //config.Save(); 
            XmlDocument xmlDocument = new XmlDocument();

            using (FileStream fs = new FileStream(configPath, FileMode.Open))
            {
                xmlDocument.Load(fs);
            }

            XmlNodeList nodes = xmlDocument.SelectNodes("/configuration/appSettings/add");

            XmlNode targetNode = null;

            foreach (XmlNode node in nodes)
            {
                if (node.Attributes["key"].Value.ToLower() == ConfigValue.ToLower())
                {
                    targetNode = node;
                }
            }
            if (targetNode != null)
            {
                targetNode.Attributes["value"].Value = TargetValue == null ? string.Empty : TargetValue;
                using (FileStream fs = new FileStream(configPath, FileMode.Create, FileAccess.ReadWrite))
                {
                    xmlDocument.Save(fs);
                }
            }
            else
            {
                throw new ApplicationException(string.Format("Could not find the specified app setting of '{0}' in {1}", ConfigValue, configPath));
            }

        }

        private void _setConfigConnectionSetting(string configPath)
        {
            XmlDocument xmlDocument = new XmlDocument();
            using (FileStream fs = new FileStream(configPath, FileMode.Open))
            {
                xmlDocument.Load(fs);
            }

            XmlNodeList nodes = xmlDocument.SelectNodes("/configuration/connectionStrings/add");

            XmlNode targetNode = null;

            foreach (XmlNode node in nodes)
            {
                if (node.Attributes["name"].Value.ToLower() == ConfigValue.ToLower())
                {
                    targetNode = node;
                }
            }
            if (targetNode != null)
            {
                targetNode.Attributes["connectionString"].Value = TargetValue == null ? string.Empty : TargetValue;
                using (FileStream fs = new FileStream(configPath, FileMode.Create, FileAccess.Write))
                {
                    xmlDocument.Save(fs);
                }
            }
            else
            {
                throw new ApplicationException(
                    string.Format("Could not find the specified app setting of '{0}' in {1}", ConfigValue,
                        configPath));
            }

        }

        private string _getConfigConnectionSetting(string configPath)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlNode targetNode = null;

            using (var fs = new FileStream(configPath, FileMode.Open))
            {
                xmlDocument.Load(fs);

                XmlNodeList nodes = xmlDocument.SelectNodes("/configuration/connectionStrings/add");


                foreach (XmlNode node in nodes)
                {
                    if (node.Attributes["name"].Value.ToLower() == ConfigValue.ToLower())
                    {
                        targetNode = node;
                    }
                }
            }
            if (targetNode != null)
            {
                return targetNode.Attributes["connectionString"].Value;
            }

            throw new ApplicationException(string.Format("Could not find the specified app setting of '{0}' in {1}", ConfigValue, configPath));
        }


        private void _setProtectedConfigSection(string configPath)
        {
            XmlDocument xmlDocument = new XmlDocument();
            using (var fs = new FileStream(configPath, FileMode.Open))
            {
                xmlDocument.Load(fs);
            }

            XmlNodeList nodes = xmlDocument.SelectNodes("/configuration/ProtectedConfigSection/appSettings/add");

            XmlNode targetNode = null;

            foreach (XmlNode node in nodes)
            {
                if (node.Attributes["key"].Value.ToLower() == ConfigValue.ToLower())
                {
                    targetNode = node;
                }

            }
            if (targetNode != null)
            {
                targetNode.Attributes["value"].Value = TargetValue == null ? string.Empty : TargetValue;
                using (FileStream fs = new FileStream(configPath, FileMode.Create, FileAccess.Write))
                {
                    xmlDocument.Save(fs);
                }
            }
            else
            {
                throw new ApplicationException(string.Format("Could not find the specified app setting of '{0}' in {1}", ConfigValue, configPath));
            }
        }

        private string _getProtectedConfigSection(string configPath)
        {
            XmlDocument xmlDocument = new XmlDocument();
            using (var fs = new FileStream(configPath, FileMode.Open))
            {
                xmlDocument.Load(fs);
            }

            XmlNodeList nodes = xmlDocument.SelectNodes("/configuration/ProtectedConfigSection/appSettings/add");
            foreach (XmlNode node in nodes)
            {
                if (node.Attributes["key"].Value.ToLower() == ConfigValue.ToLower())
                {
                    return node.Attributes["value"].Value;
                }
            }

            return string.Format("Could not find '{0}' AppSetting in {1}", ConfigValue, configPath);
        }

        #endregion

        
    }
}
