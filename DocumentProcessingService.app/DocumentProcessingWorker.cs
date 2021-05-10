using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DocumentProcessingService.app.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DocumentProcessingService.app
{
    public class DocumentProcessingWorker : BackgroundService
    {
        public IServiceProvider Services { get; }

        private readonly TimeSpan _actionInterval;
        private readonly ILogger<DocumentProcessingWorker> _logger;
        private const int INTERVAL_HOURS = 1;
        
        public DocumentProcessingWorker(IServiceProvider services, ILogger<DocumentProcessingWorker> logger)
        {
            Services = services;
            _logger = logger;
            _actionInterval = TimeSpan.FromHours(INTERVAL_HOURS);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Task is starting.");
            stoppingToken.Register(() => _logger.LogInformation("Task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = Services.CreateScope())
                    {
                        var fileOrchestratorService = scope.ServiceProvider.GetRequiredService<IFileOrchestratorService>();
                        await fileOrchestratorService.DoWork();
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Something went wrong! Exiting unhealthy background thread!");
                    break;
                }

                _logger.LogInformation($"Task will sleep for {_actionInterval.Hours} hours");
                await Task.Delay(_actionInterval, stoppingToken);
            }
        }
    }
}
