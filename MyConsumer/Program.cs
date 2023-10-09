﻿using System.Text.Json;
using AsyncCommunicationControl.Extensions;
using AsyncCommunicationControl.Services;
using Microsoft.Extensions.DependencyInjection;
using MyDomain;
using MyInfrastructure;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MyConsumer;

class Program
{
    static void Main()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IProductConsumer, ProductConsumer>()
            .AddAsyncCommunicationControl<MyMessage>("cs")
            .BuildServiceProvider();
        Run(serviceProvider);
    }

    static void Run(IServiceProvider serviceProvider)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare("products", exclusive: false, autoDelete: false);
        
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, eventArgs) =>
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                var message = JsonSerializer.Deserialize<MyMessage>(body);
                var product = JsonSerializer.Deserialize<Product>(message.Content);
                var productConsumer = serviceProvider.GetRequiredService<IProductConsumer>();
                productConsumer.ExecuteAsync(message, product);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        };
        channel.BasicConsume(queue: "products", autoAck: true, consumer: consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}