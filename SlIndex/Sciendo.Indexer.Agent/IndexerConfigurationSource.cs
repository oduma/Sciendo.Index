using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

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

    }
}
