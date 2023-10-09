using MyDomain;
using MyInfrastructure;

namespace MyConsumer;

public interface IProductConsumer
{
    Task ExecuteAsync(MyMessage message, Product product);
}