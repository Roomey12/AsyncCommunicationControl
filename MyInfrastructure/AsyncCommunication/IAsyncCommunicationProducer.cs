namespace MyInfrastructure.AsyncCommunication;

public interface IAsyncCommunicationProducer
{
    Task<int> SendAndSubmitMessage<T>(T sendMessage, string queue);

    Task<int> SendAndSubmitCustomMessage<T>(T sendMessage, MyMessage submitMessage, string queue);
}