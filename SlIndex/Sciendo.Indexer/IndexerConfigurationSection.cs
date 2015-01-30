using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Sciendo.Indexer
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

        [ConfigurationProperty("music")]
        public IndexerConfigurationSource Music
        {
            get
            {
                return (IndexerConfigurationSource)this["music"];
            }
            set
            {
                this["music"] = value;
            }
        }
        [ConfigurationProperty("lyrics")]
        public IndexerConfigurationSource Lyrics
        {
            get
            {
                return (IndexerConfigurationSource)this["lyrics"];
            }
            set
            {
                this["lyrics"] = value;
            }
        }

    }
}
