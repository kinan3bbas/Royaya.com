using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RoyayaControlPanel.com.Startup))]
namespace RoyayaControlPanel.com
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
