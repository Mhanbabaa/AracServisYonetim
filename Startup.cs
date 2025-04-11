using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartup(typeof(AracServisYonetim.Startup))]
namespace AracServisYonetim
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // OWIN yapılandırma kodları buraya gelecek
            ConfigureAuth(app);
        }
    }
} 