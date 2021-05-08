using DocumentService.Services;
using DocumentService.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentService.Infrastructure.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services
                .AddTransient<IFileReadingService, FileReadingService>()
                .AddTransient<IFileDeletionService, FileDeletionService>();
            return services;
        }

        internal static IServiceCollection AddStores(this IServiceCollection services)
        {
            services.AddTransient<ILookupStore, LookupStore>();
            return services;
        }
    }
}
