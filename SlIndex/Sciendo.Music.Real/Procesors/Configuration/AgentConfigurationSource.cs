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

        public override string ToString()
        {
            return string.Format("\tSource Directory: {0}\r\n\tSearch pattern:{1}",
                SourceDirectory, SearchPattern);
        }
    }
}
