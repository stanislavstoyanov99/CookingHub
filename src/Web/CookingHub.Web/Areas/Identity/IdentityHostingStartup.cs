using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(CookingHub.Web.Areas.Identity.IdentityHostingStartup))]

namespace CookingHub.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
