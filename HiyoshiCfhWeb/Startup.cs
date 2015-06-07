using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HiyoshiCfhWeb.Startup))]
namespace HiyoshiCfhWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
