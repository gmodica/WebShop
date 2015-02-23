using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebShop.Startup))]
namespace WebShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
