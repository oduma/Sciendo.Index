using System.Configuration;

namespace Sciendo.Music.Real.Procesors.Configuration
{
    public class AgentConfigurationSection:ConfigurationSection
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
        public AgentConfigurationSource Music
        {
            get
            {
                return (AgentConfigurationSource)this["music"];
            }
            set
            {
                this["music"] = value;
            }
        }
        [ConfigurationProperty("lyrics")]
        public AgentConfigurationSource Lyrics
        {
            get
            {
                return (AgentConfigurationSource)this["lyrics"];
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
