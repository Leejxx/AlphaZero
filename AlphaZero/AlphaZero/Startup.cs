using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AlphaZero.Startup))]
namespace AlphaZero
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
