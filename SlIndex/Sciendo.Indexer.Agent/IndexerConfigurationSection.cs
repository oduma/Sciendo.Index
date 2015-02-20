using System.Configuration;

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

        [ConfigurationProperty("packagesRetainerLimt", DefaultValue = 100, IsRequired = false)]
        public int PackagesRetainerLimit
        {
            get
            {
                return (int)this["packagesRetainerLimt"];
            }
            set
            {
                this["packagesRetainerLimt"] = value;
            }
        }

        public override string ToString()
        {
            return string.Format("SolrConnectionString: {0} Packages Retainer Limit: {1}\r\nMusic:\r\n{2}\r\nLyrics:\r\n{3}.", SolrConnectionString, PackagesRetainerLimit,Music.ToString(),
                Lyrics.ToString());
        }
    }
}
