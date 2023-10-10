using AsyncCommunicationControl.Models;

namespace AsyncCommunicationControl;

public class RetryPolicy
{
    public readonly List<ExecutionStatus> RetryStatuses;
    public readonly int MaxRetryAttempts;
    public readonly TimeSpan RetryInterval;
    public string Queue;
    
    public RetryPolicy(List<ExecutionStatus> allowedStatuses, int maxRetryAttempts, TimeSpan retryInterval)
    {
        RetryStatuses = allowedStatuses ?? throw new ArgumentNullException(nameof(allowedStatuses));
        MaxRetryAttempts = maxRetryAttempts;
        RetryInterval = retryInterval;
    }
}