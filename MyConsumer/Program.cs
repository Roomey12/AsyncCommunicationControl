using System.Text.Json;
using MyDomain;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("sas");
var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.QueueDeclare("products", exclusive: false, autoDelete: false);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) =>
{
    try
    {
        var body = eventArgs.Body.ToArray();
        var product = JsonSerializer.Deserialize<Product>(body);
        Console.WriteLine($"Product: {product.Name}|{product.Price}");
    }
    catch(Exception ex)
    {
        //Logger
        //channel.BasicNack(eventArgs.DeliveryTag, false, true);
    }
};
channel.BasicConsume(queue: "products", autoAck: true, consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();