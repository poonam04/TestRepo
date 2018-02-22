using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyApplication.Startup))]
namespace MyApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
