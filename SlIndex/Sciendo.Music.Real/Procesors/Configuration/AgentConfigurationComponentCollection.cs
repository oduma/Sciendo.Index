using System.Configuration;

namespace Sciendo.Music.Real.Procesors.Configuration
{
    [ConfigurationCollection(typeof(AgentConfigurationComponent))]
    public class AgentConfigurationComponentCollection:ConfigurationElementCollection
    {
        public AgentConfigurationComponent this[int index ]
        {
            get { return (AgentConfigurationComponent) BaseGet(index); }
            set
            {
                if(BaseGet(index)!=null)
                    BaseRemoveAt(index);
                BaseAdd(index,value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new AgentConfigurationComponent();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AgentConfigurationComponent) element).Key;
        }
    }
}
