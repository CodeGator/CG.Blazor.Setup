using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace CG.Blazor.Setup.QuickStart
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Run the host as a setup observer.
            await HostHelper.RunAsSetupObserverAsync(
                () => CreateHostBuilder(args)
                );
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
