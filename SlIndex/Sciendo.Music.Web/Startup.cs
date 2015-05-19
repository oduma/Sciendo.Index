using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using Sciendo.Music.Web;
using System;

[assembly: OwinStartup(typeof(Startup))]

namespace Sciendo.Music.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.Configuration.ConnectionTimeout = new TimeSpan(0, 45, 0);
            
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
 
        }
    }
}