using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(异步操作.Startup))]
namespace 异步操作
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
