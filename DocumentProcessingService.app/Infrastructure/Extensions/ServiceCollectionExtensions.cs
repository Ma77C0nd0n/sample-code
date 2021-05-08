using DocumentProcessingService.app.Services;
using DocumentProcessingService.app.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentProcessingService.app.Infrastructure.Extensions
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
