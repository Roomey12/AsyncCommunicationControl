using AsyncCommunicationControl.Models;
using AsyncCommunicationControl.Services;
using MyInfrastructure;
using Microsoft.EntityFrameworkCore;

namespace MyRetrySystem;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IMessageService<MyMessage> _messageService;

    public Worker(ILogger<Worker> logger, IMessageService<MyMessage> messageService)
    {
        _logger = logger;
        _messageService = messageService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }

    public async Task ExecuteAsync(ExecutionStatus executionStatus, string queue)
    {
        var retryMessages = await _messageService.GetMessagesByStatusAndQueue(executionStatus, queue).ToListAsync();
        //взять консьюмера, получить условия - например где у месейджей статус ExecutedWithErrors и интервал
        //
    }
}