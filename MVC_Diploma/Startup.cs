using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC_Diploma.Startup))]
namespace MVC_Diploma
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
