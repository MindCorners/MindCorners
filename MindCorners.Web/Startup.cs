using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MindCorners.Web.Startup))]
namespace MindCorners.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
