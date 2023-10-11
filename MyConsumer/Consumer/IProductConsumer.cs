using MyDomain;
using MyDomain.Models;
using MyInfrastructure.Models;

namespace MyConsumer.Consumer;

public interface IProductConsumer
{
    Task ExecuteAsync(MyMessage message, Product product);
}