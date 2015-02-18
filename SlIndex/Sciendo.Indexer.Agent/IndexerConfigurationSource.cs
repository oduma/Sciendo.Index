using System.Configuration;

namespace Sciendo.Indexer.Agent
{
    public class IndexerConfigurationSource:ConfigurationElement
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

        [ConfigurationProperty("currentImplementation", DefaultValue = "", IsRequired = false)]
        public string CurrentImplementation
        {
            get
            {
                return (string)this["currentImplementation"];
            }
            set
            {
                this["currentImplementation"] = value;
            }
        }

        public override string ToString()
        {
            return string.Format("\tSource Directory: {0}\r\n\tSearch pattern:{1}\r\n\tCurrent Implementation:{2}",
                SourceDirectory, SearchPattern, CurrentImplementation);
        }
    }
}
