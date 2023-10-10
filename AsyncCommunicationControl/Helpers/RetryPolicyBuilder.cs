using AsyncCommunicationControl.Models;

namespace AsyncCommunicationControl.Helpers;

public class RetryPolicyBuilder
{
    public readonly List<ExecutionStatus> _defaultRetryStatuses = new ()
    {
        ExecutionStatus.ToBeExecuted, ExecutionStatus.ExecutedWithWarnings, ExecutionStatus.ExecutedWithErrors
    };
    private List<ExecutionStatus> _retryStatuses = new();
    private int _maxRetryAttempts = 5;
    private TimeSpan _retryInterval = TimeSpan.FromMinutes(2);
    private string _queue;

    /// <summary>
    /// If method not called - ExecutionStatus.ToBeExecuted, ExecutionStatus.ExecutedWithWarnings, ExecutionStatus.ExecutedWithErrors applied
    /// </summary>
    /// <param name="statuses"></param>
    /// <returns></returns>
    public RetryPolicyBuilder WithExecutionStatus(params ExecutionStatus[] statuses)
    {
        _retryStatuses.AddRange(statuses);
        return this;
    }

    public RetryPolicyBuilder WithMaxRetryAttempts(int maxAttempts)
    {
        _maxRetryAttempts = maxAttempts;
        return this;
    }

    public RetryPolicyBuilder WithRetryInterval(TimeSpan interval)
    {
        _retryInterval = interval;
        return this;
    }
    
    public RetryPolicyBuilder WithQueue(string queue)
    {
        _queue = queue;
        return this;
    }

    public RetryPolicy Build()
    {
        return new RetryPolicy(_retryStatuses.Count > 0
                ? _retryStatuses
                : _defaultRetryStatuses,
            _maxRetryAttempts,
            _retryInterval,
            _queue);
    }
}