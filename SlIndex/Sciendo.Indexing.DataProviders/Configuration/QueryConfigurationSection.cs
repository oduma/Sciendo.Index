using System.Configuration;

namespace Sciendo.Music.DataProviders.Configuration
{
    public class QueryConfigurationSection:ConfigurationSection
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

        [ConfigurationProperty("solrConnectionString", DefaultValue = "", IsRequired = false)]
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
        [ConfigurationProperty("pageSize", DefaultValue = 25, IsRequired = false)]
        public int PageSize
        {
            get
            {
                return (int)this["pageSize"];
            }
            set
            {
                this["pageSize"] = value;
            }
        }

    }
}