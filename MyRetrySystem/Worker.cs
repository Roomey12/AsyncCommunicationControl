using AsyncCommunicationControl;
using AsyncCommunicationControl.Models;
using AsyncCommunicationControl.Services;
using MyInfrastructure;
using Microsoft.EntityFrameworkCore;
using MyInfrastructure.AsyncCommunication;

namespace MyRetrySystem;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IRetryService<MyMessage> _retryService;
    private readonly IAsyncCommunicationProducer _asyncCommunicationProducer;

    public Worker(ILogger<Worker> logger, IRetryService<MyMessage> retryService, IAsyncCommunicationProducer asyncCommunicationProducer)
    {
        _logger = logger;
        _retryService = retryService;
        _asyncCommunicationProducer = asyncCommunicationProducer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(10000, stoppingToken);
        }
    }

    // проблема что на каждую очередь нужен свой ретрай
    public async Task ExecuteAsync(RetryPolicy retryPolicy)
    {
        var retryMessages = await _retryService.GetRetryMessagesAsync(retryPolicy, retryPolicy.Queue).ToListAsync();
        foreach (var retryMessage in retryMessages)
        {
            retryMessage.TotalRetryAttempts++;
            await _asyncCommunicationProducer.SendAndSubmitMessageAsync(retryMessage, retryPolicy.Queue);
        }
    }
}