using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DocumentProcessingService.app.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DocumentProcessingService.app
{
    public class DocumentProcessingWorker : BackgroundService
    {
        private readonly TimeSpan _actionInterval;
        private IFileOrchestratorService _fileOrchestrator;
        private readonly ILogger<DocumentProcessingWorker> _logger;

        public DocumentProcessingWorker(IFileOrchestratorService fileOrchestrator, ILogger<DocumentProcessingWorker> logger)
        {
            _fileOrchestrator = fileOrchestrator;
            _logger = logger;
            _actionInterval = TimeSpan.FromHours(1);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Task is starting.");
            stoppingToken.Register(() => _logger.LogInformation("Task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _fileOrchestrator.Handle();
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
