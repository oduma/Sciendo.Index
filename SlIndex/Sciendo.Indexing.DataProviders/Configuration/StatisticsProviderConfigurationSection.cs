using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Music.DataProviders.Configuration
{
    public class StatisticsProviderConfigurationSection:ConfigurationSection
    {
        [ConfigurationProperty("currentStatisticsProvider", DefaultValue = "mock", IsRequired = true)]
        public string CurrentStatisticsProvider
        {
            get
            {
                return (string)this["currentStatisticsProvider"];
            }
            set
            {
                this["currentStatisticsProvider"] = value;
            }
        }
        public override string ToString()
        {
            return
                string.Format(
                    "\r\n\tCurrent Statistics Provider:{0}",
                    CurrentStatisticsProvider);
        }

    }
}
