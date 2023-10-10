namespace AsyncCommunicationControl.Models;

public class RetryPolicy
{
    public readonly List<ExecutionStatus> RetryStatuses;
    public readonly int MaxRetryAttempts;
    public readonly TimeSpan RetryInterval;
    public readonly string Queue;
    
    public RetryPolicy(List<ExecutionStatus> allowedStatuses, int maxRetryAttempts, TimeSpan retryInterval, string queue)
    {
        RetryStatuses = allowedStatuses ?? throw new ArgumentNullException(nameof(allowedStatuses));
        MaxRetryAttempts = maxRetryAttempts;
        RetryInterval = retryInterval;
        Queue = queue ?? throw new ArgumentNullException(nameof(queue));
    }
}