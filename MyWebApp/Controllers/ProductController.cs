using Microsoft.AspNetCore.Mvc;
using MyDomain;
using MyInfrastructure;
using MyInfrastructure.AsyncCommunication;

namespace MyWebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IAsyncCommunicationProducer _asyncCommunicationProducer;
    private const string ProductsQueue = "products";
    
    public ProductController(IAsyncCommunicationProducer asyncCommunicationProducer)
    {
        _asyncCommunicationProducer = asyncCommunicationProducer;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        var myMessage = new MyMessage("createdFromProductController");

        var result = await _asyncCommunicationProducer.SendAndSubmitMessage(product, ProductsQueue);
        return Ok(result);
    }
}