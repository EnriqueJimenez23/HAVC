using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CsWeb.Startup))]
namespace CsWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
