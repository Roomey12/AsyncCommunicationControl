using AsyncCommunicationControl.Helpers;
using AsyncCommunicationControl.Models;
using AsyncCommunicationControl.Services.Interfaces;
using MyConsumer;
using MyConsumer.Consumer;
using MyInfrastructure.AsyncCommunication;
using MyInfrastructure.Models;

namespace MyRetrySystem;

public class RetryWorker : BackgroundService
{
    private readonly ILogger<RetryWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IAsyncCommunicationProducer _asyncCommunicationProducer;
    private readonly IEnumerable<RetryPolicy> _retryPolicies;

    public RetryWorker(ILogger<RetryWorker> logger,
                       IServiceProvider serviceProvider,
                       IAsyncCommunicationProducer asyncCommunicationProducer)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _asyncCommunicationProducer = asyncCommunicationProducer;
        _retryPolicies = new List<RetryPolicy> { ProductConsumer.RetryPolicy };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await ExecuteRetryAsync(_retryPolicies);
            await Task.Delay(5000, stoppingToken);
        }
    }

    private async Task ExecuteRetryAsync(IEnumerable<RetryPolicy> retryPolicies)
    {
        using var scope = _serviceProvider.CreateScope();
        var retryService = scope.ServiceProvider.GetRequiredService<IRetryService<MyMessage>>();
        var retryMessages = (await retryService.GetRetryMessagesAsync(retryPolicies)).ToList();
        foreach (var retryMessage in retryMessages)
        {
            _logger.LogInformation("Sending retry message: {retryMessageId} at {time}", retryMessage.Id, DateTimeOffset.Now);
            await _asyncCommunicationProducer.SendAndUpdateMessageAsync(retryMessage);
        }
    }
}