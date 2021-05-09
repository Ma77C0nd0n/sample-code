using DocumentProcessingService.app.Queries;
using DocumentProcessingService.app.Repositories;
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
                .AddSingleton<IFileOrchestratorService, FileOrchestratorService>()
                .AddTransient<IFileProcessingService, FileProcessingService>()
                .AddTransient<IFileDeletionService, FileDeletionService>();
            return services;
        }

        internal static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddTransient<ILookupStore, LookupStore>()
                .AddTransient<IFileShareQuery, FileShareQuery>()
                .AddTransient<IFileStreamReader, FileStreamReader>();
        }
    }
}
