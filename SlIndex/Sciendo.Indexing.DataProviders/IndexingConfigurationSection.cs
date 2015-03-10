using System.Configuration;

namespace Sciendo.Indexing.DataProviders
{
    public class IndexingConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("currentDataProvider", DefaultValue = "mock", IsRequired = true)]
        public string CurrentDataProvider
        {
            get
            {
                return (string)this["currentDataProvider"];
            }
            set
            {
                this["currentDataProvider"] = value;
            }
        }

    }
}
