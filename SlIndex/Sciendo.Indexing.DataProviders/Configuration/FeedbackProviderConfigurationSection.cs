using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Music.DataProviders.Configuration
{
    public class FeedbackProviderConfigurationSection:ConfigurationSection
    {
        [ConfigurationProperty("feedbackUrl", IsRequired = true)]
        public string FeedbackUrl
        {
            get
            {
                return (string)this["feedbackUrl"];
            }
            set
            {
                this["feedbackUrl"] = value;
            }
        }
        public override string ToString()
        {
            return
                string.Format(
                    "\r\n\tCurrent Feedback Provider:Feedback Url:{0}",
                    FeedbackUrl);
        }


    }
}
