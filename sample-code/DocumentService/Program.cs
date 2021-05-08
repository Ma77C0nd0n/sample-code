using DocumentService.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace DocumentService
{
    public class Program
    {
        /* Steps:
         * For each file in a given directory
         * Read contents - map to model, validate
         * Pass mapping properties to lookup store, persist
         * Delete file
         */
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = new HostBuilder();
            hostBuilder
                .ConfigureAppConfiguration((hostContext, configureAppBuilder) =>
                {
                    configureAppBuilder.AddJsonFile("appsettings.json", optional: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .RegisterServices()
                        .AddStores();
                })
                .ConfigureHostConfiguration(configureHost =>
                {
                    configureHost.AddEnvironmentVariables();
                });

            return hostBuilder;
        }
    }
}