using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TopMarket.Startup))]
namespace TopMarket
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
