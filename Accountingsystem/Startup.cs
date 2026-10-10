using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Accountingsystem.Startup))]
namespace Accountingsystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
