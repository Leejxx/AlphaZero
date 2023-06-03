using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(roomRental.Startup))]
namespace roomRental
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
