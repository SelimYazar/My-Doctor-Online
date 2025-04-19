using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(_152120191023_WebBasedTechnologies_Hw4.Startup))]

namespace _152120191023_WebBasedTechnologies_Hw4
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Uygulamanızı nasıl yapılandıracağınız hakkında daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkID=316888 adresini ziyaret edin
            // SignalR'ı başlat
            app.MapSignalR();
        }
    }
}
