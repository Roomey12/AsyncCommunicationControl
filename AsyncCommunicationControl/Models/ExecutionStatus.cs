namespace AsyncCommunicationControl.Models;

public enum ExecutionStatus
{
    ToBeExecuted,
    SuccessfullyExecuted,
    ExecutedWithErrors,
    ExecutedWithWarnings,
    InRetryQueue
}