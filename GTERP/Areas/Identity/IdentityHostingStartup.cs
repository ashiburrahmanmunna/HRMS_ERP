using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(GTERP.Areas.Identity.IdentityHostingStartup))]
namespace GTERP.Areas.Identity
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