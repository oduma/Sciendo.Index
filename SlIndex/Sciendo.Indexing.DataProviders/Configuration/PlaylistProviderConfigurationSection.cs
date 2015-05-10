using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Music.DataProviders.Configuration
{
    public class PlaylistProviderConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("currentPlaylistProvider", DefaultValue = "mock", IsRequired = true)]
        public string CurrentPlaylistProvider
        {
            get { return (string) this["currentPlaylistProvider"]; }
            set { this["currentPlaylistProvider"] = value; }
        }
        [ConfigurationProperty("lastFmBaseApiUrl", DefaultValue = "http://ws.audioscrobbler.com/2.0/", IsRequired = true)]
        public string LastFmBaseApiUrl
        {
            get
            {
                return (string)this["lastFmBaseApiUrl"];
            }
            set
            {
                this["lastFmBaseApiUrl"] = value;
            }
        }

        [ConfigurationProperty("lastFmApiKey", DefaultValue = "990a564011017129df00d7f3f3f1f2fa", IsRequired = true)]
        public string LastFmApiKey
        {
            get
            {
                return (string)this["lastFmApiKey"];
            }
            set
            {
                this["lastFmApiKey"] = value;
            }
        }

        [ConfigurationProperty("lastFmUser", DefaultValue = "scentmaster", IsRequired = true)]
        public string LastFmUser
        {
            get
            {
                return (string)this["lastFmUser"];
            }
            set
            {
                this["lastFmUser"] = value;
            }
        }

    }
}
