using System.Configuration;

namespace Sciendo.Music.Agent
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

        [ConfigurationProperty("currentProcessingImplementation", DefaultValue = "", IsRequired = true)]
        public string CurrentProcessingImplementation
        {
            get
            {
                return (string)this["currentProcessingImplementation"];
            }
            set
            {
                this["currentProcessingImplementation"] = value;
            }
        }

        [ConfigurationProperty("currentMonitoringImplementation", DefaultValue = "", IsRequired = true)]
        public string CurrentMonitoringImplementation
        {
            get
            {
                return (string)this["currentMonitoringImplementation"];
            }
            set
            {
                this["currentProcessingImplementation"] = value;
            }
        }

        public override string ToString()
        {
            return string.Format("\tSource Directory: {0}\r\n\tSearch pattern:{1}\r\n\tCurrent Processing Implementation:{2}\r\n\tCurrent monitoring implementation:{3}",
                SourceDirectory, SearchPattern, CurrentProcessingImplementation, CurrentMonitoringImplementation);
        }
    }
}
