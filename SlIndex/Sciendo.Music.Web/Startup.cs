using Microsoft.Owin;
using Owin;
using Sciendo.Music.Web;

[assembly: OwinStartup(typeof(Startup))]

namespace Sciendo.Music.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}