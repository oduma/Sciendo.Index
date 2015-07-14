using Microsoft.AspNet.SignalR;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Cors;

namespace Sciendo.Music.Agent
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);

                var hubConfiguration = new HubConfiguration
                {
                    EnableDetailedErrors = true,
                    EnableJSONP = true
                };

                map.RunSignalR(hubConfiguration);
            });
        } 
    }
}
