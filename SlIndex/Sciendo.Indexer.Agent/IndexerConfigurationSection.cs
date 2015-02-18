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

        public override string ToString()
        {
            return string.Format("SolrConnectionString: {0}\r\nMusic:\r\n{1}\r\nLyrics:\r\n{2}.", SolrConnectionString, Music.ToString(),
                Lyrics.ToString());
        }
    }
}
