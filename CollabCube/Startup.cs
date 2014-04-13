using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CollabCube.Startup))]
namespace CollabCube
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
