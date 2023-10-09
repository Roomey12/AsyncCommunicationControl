namespace MyInfrastructure.AsyncCommunication;

public interface IAsyncCommunicationProducer
{
    Task SendAndSubmitMessage<TMessageContent>(TMessageContent sendMessage, string queue);

    Task SendAndSubmitCustomMessage<TMessageContent>(TMessageContent sendMessage, MyMessage submitMessage, string queue);
}