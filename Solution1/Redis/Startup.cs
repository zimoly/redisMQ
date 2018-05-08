using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Redis.Startup))]
namespace Redis
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
