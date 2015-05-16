using System.Configuration;

namespace Sciendo.Music.DataProviders.Configuration
{
    public class IndexDataProviderConfigurationSection : ConfigurationSection
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
        public override string ToString()
        {
            return
                string.Format(
                    "\r\n\tCurrent Data Provider:{0}\r\n",
                    CurrentDataProvider);
        }

    }
}
