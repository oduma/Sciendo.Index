using System.Configuration;

namespace Sciendo.Music.Real.Procesors.Configuration
{
    public class AgentConfigurationComponent:ConfigurationElement
    {
        [ConfigurationProperty("key", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string Key
        {
            get { return (string) this["key"]; }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("assemblyFilter", DefaultValue = "", IsRequired = true)]
        public string AssemblyFilter
        {
            get { return (string)this["assemblyFilter"]; }
            set { this["assemblyFilter"] = value; }
        }
        

    }
}
