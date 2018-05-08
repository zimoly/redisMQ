using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(rabbitMQ.Startup))]
namespace rabbitMQ
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
