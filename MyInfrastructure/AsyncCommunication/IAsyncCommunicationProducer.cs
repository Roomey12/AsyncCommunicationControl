﻿using MyInfrastructure.Models;

namespace MyInfrastructure.AsyncCommunication;

public interface IAsyncCommunicationProducer
{
    Task SendAndSubmitMessageAsync<TMessageContent>(TMessageContent sendMessage, string queue);

    Task SendAndSubmitCustomMessageAsync<TMessageContent>(TMessageContent sendMessage, MyMessage submitMessage, string queue);
    Task SendAndUpdateMessageAsync(MyMessage message);
}