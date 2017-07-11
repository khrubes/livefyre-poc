using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(livefyre_test.Startup))]
namespace livefyre_test
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
