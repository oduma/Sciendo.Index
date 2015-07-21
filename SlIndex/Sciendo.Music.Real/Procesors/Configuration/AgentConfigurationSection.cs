using System.Configuration;

namespace Sciendo.Music.Real.Procesors.Configuration
{
    public class AgentConfigurationSection:ConfigurationSection
    {
        [ConfigurationProperty("solrConnectionString", DefaultValue = "", IsRequired = true)]
        public string SolrConnectionString
        {
            get
            {
                return (string)this["solrConnectionString"];
            }
            set
            {
                this["solrConnectionString"] = value;
            }
        }

        [ConfigurationProperty("music")]
        public AgentConfigurationSource Music
        {
            get
            {
                return (AgentConfigurationSource)this["music"];
            }
            set
            {
                this["music"] = value;
            }
        }
        [ConfigurationProperty("lyrics")]
        public AgentConfigurationSource Lyrics
        {
            get
            {
                return (AgentConfigurationSource)this["lyrics"];
            }
            set
            {
                this["lyrics"] = value;
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
            return string.Format("SolrConnectionString: {0} Current Processing Implementation:{1}\r\n\tCurrent monitoring implementation:{2}\r\nMusic:\r\n{3}\r\nLyrics:\r\n{4}.", SolrConnectionString, CurrentProcessingComponentKey, CurrentMonitoringComponentKey, Music.ToString(),
                Lyrics.ToString());
        }
    }
}
