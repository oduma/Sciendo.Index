using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Sciendo.Index.Web
{
    public class IndexingConfigurationSection : ConfigurationSection
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

    }
}
