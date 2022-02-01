using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CPDPortalMVC.Startup))]
namespace CPDPortalMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
