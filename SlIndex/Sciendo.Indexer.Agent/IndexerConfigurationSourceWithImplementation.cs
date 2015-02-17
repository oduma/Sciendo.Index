using System.Configuration;

namespace Sciendo.Indexer.Agent
{
    public class IndexerConfigurationSourceWithImplementation:IndexerConfigurationSourceBase
    {
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

    }
}
