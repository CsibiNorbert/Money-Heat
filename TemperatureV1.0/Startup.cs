using Microsoft.Owin;
using Owin;
using TemperatureV1._0;

[assembly: OwinStartup(typeof(Startup))]

namespace TemperatureV1._0
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}