using DocumentProcessingService.app.Models;
using DocumentProcessingService.app.Queries;
using DocumentProcessingService.app.Repositories;
using DocumentProcessingService.app.Services;
using DocumentProcessingService.app.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentProcessingService.app.Infrastructure.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services
                .AddScoped<IFileOrchestratorService, FileOrchestratorService>()
                .AddScoped<IFileProcessingService, FileProcessingService>()
                .AddScoped<IFileDeletionRepository, FileDeletionRepository>();
            return services;
        }

        internal static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddDbContext<DocumentContext>(opt => opt.UseInMemoryDatabase("DocumentDatabase"))
                .AddMemoryCache()
                .AddScoped<ILookupStore, LookupStore>()
                .AddScoped<IFileShareQuery, FileShareQuery>()
                .AddScoped<IFileStreamReader, FileStreamReader>();
        }
    }
}
