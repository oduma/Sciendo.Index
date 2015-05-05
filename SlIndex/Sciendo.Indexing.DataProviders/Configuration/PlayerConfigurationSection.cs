using System.Configuration;

namespace Sciendo.Music.DataProviders.Configuration
{
    public class PlayerConfigurationSection:ConfigurationSection
    {
        [ConfigurationProperty("currentPlayerProcess", DefaultValue = "mock", IsRequired = true)]
        public string CurrentPlayerProcess
        {
            get
            {
                return (string)this["currentPlayerProcess"];
            }
            set
            {
                this["currentPlayerProcess"] = value;
            }
        }

        [ConfigurationProperty("playerProcessIdentifier", DefaultValue = "", IsRequired = false)]
        public string PlayerProcessIdentifier
        {
            get
            {
                return (string)this["playerProcessIdentifier"];
            }
            set
            {
                this["playerProcessIdentifier"] = value;
            }
        }
    }
}
