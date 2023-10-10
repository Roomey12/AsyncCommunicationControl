using AsyncCommunicationControl.Helpers;
using AsyncCommunicationControl.Models;
using AsyncCommunicationControl.Services.Interfaces;
using MyInfrastructure.AsyncCommunication;
using MyInfrastructure.Models;

namespace MyRetrySystem;

public class RetryWorker : BackgroundService
{
    private readonly ILogger<RetryWorker> _logger;
    private readonly IRetryService<MyMessage> _retryService;
    private readonly IAsyncCommunicationProducer _asyncCommunicationProducer;
    private readonly IEnumerable<RetryPolicy> _retryPolicies;

    public RetryWorker(ILogger<RetryWorker> logger, IRetryService<MyMessage> retryService, IAsyncCommunicationProducer asyncCommunicationProducer)
    {
        _logger = logger;
        _retryService = retryService;
        _asyncCommunicationProducer = asyncCommunicationProducer;
        _retryPolicies = InitRetryPolicies();
    }

    private IEnumerable<RetryPolicy> InitRetryPolicies()
    {
        var retryPolicies = new List<RetryPolicy>();
        var productRetryPolicy = new RetryPolicyBuilder()
            .WithQueue("product")
            .WithExecutionStatus(ExecutionStatus.ExecutedWithErrors)
            .WithMaxRetryAttempts(3)
            .WithRetryInterval(TimeSpan.FromMinutes(1))
            .Build();
        retryPolicies.Add(productRetryPolicy);
        return retryPolicies;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await ExecuteRetryAsync(_retryPolicies);
            await Task.Delay(10000, stoppingToken);
        }
    }

    private async Task ExecuteRetryAsync(IEnumerable<RetryPolicy> retryPolicies)
    {
        var retryMessages = await _retryService.GetRetryMessagesAsync(retryPolicies);
        foreach (var retryMessage in retryMessages)
        {
            retryMessage.TotalRetryAttempts++;
            _logger.LogInformation("Sending retry message: {retryMessageId} at {time}", retryMessage.Id, DateTimeOffset.Now);
            await _asyncCommunicationProducer.SendAndSubmitMessageAsync(retryMessage, retryMessage.Queue);
        }   
    }
}