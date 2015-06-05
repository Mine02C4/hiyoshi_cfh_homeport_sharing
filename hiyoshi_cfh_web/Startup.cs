using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(hiyoshi_cfh_web.Startup))]
namespace hiyoshi_cfh_web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
