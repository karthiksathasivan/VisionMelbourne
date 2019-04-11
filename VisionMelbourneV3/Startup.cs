using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VisionMelbourneV3.Startup))]
namespace VisionMelbourneV3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
