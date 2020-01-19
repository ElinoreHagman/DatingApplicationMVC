using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC_Dating_Project.Startup))]
namespace MVC_Dating_Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
