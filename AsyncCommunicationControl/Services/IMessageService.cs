﻿using AsyncCommunicationControl.Entities;
using AsyncCommunicationControl.Models;

namespace AsyncCommunicationControl.Services;

public interface IMessageService<TCustomMessage> where TCustomMessage : Message, new()
{
    Task<int> SubmitMessageAsync(TCustomMessage message);

    Task<TCustomMessage> CreateAndSubmitMessageAsync<TMessageContent>(TMessageContent content, ExecutionStatus status = ExecutionStatus.ToBeExecuted);
}