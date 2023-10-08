using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace MyInfrastructure.RabbitMQ;

public class RabbitMQProducer : IMessageProducer
{
    public void SendMessage<T>(T message, string queue)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue, exclusive: false, autoDelete: false);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: "", routingKey: queue, body: body);
    }
}