using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Sciendo.Indexer.Agent
{
    public class IndexerConfigurationSection:ConfigurationSection
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

        [ConfigurationProperty("currentSender", DefaultValue = "solrSender", IsRequired = false)]
        public string CurrentSender
        {
            get
            {
                return (string)this["currentSender"];
            }
            set
            {
                this["currentSender"] = value;
            }
        }
        [ConfigurationProperty("music")]
        public IndexerConfigurationSourceBase Music
        {
            get
            {
                return (IndexerConfigurationSourceBase)this["music"];
            }
            set
            {
                this["music"] = value;
            }
        }
        [ConfigurationProperty("lyrics")]
        public IndexerConfigurationSourceWithImplementation Lyrics
        {
            get
            {
                return (IndexerConfigurationSourceWithImplementation)this["lyrics"];
            }
            set
            {
                this["lyrics"] = value;
            }
        }

    }
}
