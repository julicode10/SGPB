                using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SGPB.Web.Data;

namespace SGPB.Web
{
        public class Program
        {
                public static void Main(string[] args)
                {
                        IWebHost host = CreateWebHostBuilder(args).Build();
                        RunSeeding(host);
                        host.Run();
                }
                private static void RunSeeding(IWebHost host)
                {
                        IServiceScopeFactory scopeFactory =
                       host.Services.GetService<IServiceScopeFactory>();
                        using (IServiceScope scope = scopeFactory.CreateScope())
                        {
                                SeedDb? service = scope.ServiceProvider.GetService<SeedDb>();
                                service.SeedAsync().Wait();
                        }
                }
                public static IWebHostBuilder CreateWebHostBuilder(string[] args)
                {
                        return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
                }
        }
}
