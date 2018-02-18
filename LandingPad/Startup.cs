using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LandingPad.Startup))]
namespace LandingPad
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
