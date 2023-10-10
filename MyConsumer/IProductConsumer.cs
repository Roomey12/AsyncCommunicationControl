using MyDomain;
using MyInfrastructure;
using MyInfrastructure.Models;

namespace MyConsumer;

public interface IProductConsumer
{
    Task ExecuteAsync(MyMessage message, Product product);
}