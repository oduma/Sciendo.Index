using System.Configuration;

namespace Sciendo.Music.Real.Procesors.Configuration
{
    public class AgentConfigurationSource:ConfigurationElement
    {
        [ConfigurationProperty("sourceDirectory")]
        public string SourceDirectory 
        {
            get
            {
                return (string)this["sourceDirectory"];
            }
            set
            {
                this["sourceDirectory"] = value;
            }
        }

        [ConfigurationProperty("searchPattern")]
        public string SearchPattern
        {
            get
            {
                return (string)this["searchPattern"];
            }
            set
            {
                this["searchPattern"] = value;
            }
        }

        [ConfigurationProperty("currentProcessingComponentKey", DefaultValue = "", IsRequired = true)]
        public string CurrentProcessingComponentKey
        {
            get
            {
                return (string)this["currentProcessingComponentKey"];
            }
            set
            {
                this["currentProcessingComponentKey"] = value;
            }
        }

        [ConfigurationProperty("currentMonitoringComponentKey", DefaultValue = "", IsRequired = true)]
        public string CurrentMonitoringComponentKey
        {
            get
            {
                return (string)this["currentMonitoringComponentKey"];
            }
            set
            {
                this["currentMonitoringComponentKey"] = value;
            }
        }

        public override string ToString()
        {
            return string.Format("\tSource Directory: {0}\r\n\tSearch pattern:{1}\r\n\tCurrent Processing Implementation:{2}\r\n\tCurrent monitoring implementation:{3}",
                SourceDirectory, SearchPattern, CurrentProcessingComponentKey, CurrentMonitoringComponentKey);
        }
    }
}
