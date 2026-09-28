using Temporalio.Common;
using Temporalio.Workflows;
using Week9_ResilientOrdersApi.Activities;

namespace Week9_ResilientOrdersApi.Workflows;

// A "Workflow" in Temporal is durable orchestration logic: its code and
// progress are recorded by the Temporal server, so if this process crashes
// or the network drops partway through, the workflow resumes from where it
// left off on any available worker - it does not silently lose the order.
[Workflow]
public class OrderConfirmationWorkflow
{
    [WorkflowRun]
    public async Task RunAsync(OrderConfirmationInput input)
    {
        var activityOptions = new ActivityOptions
        {
            StartToCloseTimeout = TimeSpan.FromSeconds(30),
            RetryPolicy = new RetryPolicy
            {
                InitialInterval = TimeSpan.FromSeconds(2),
                BackoffCoefficient = 2,
                MaximumInterval = TimeSpan.FromSeconds(30),
                MaximumAttempts = 5, // Temporal retries the email send this many times before giving up.
            },
        };

        await Workflow.ExecuteActivityAsync(
            (EmailActivities activities) => activities.SendOrderConfirmationEmailAsync(input),
            activityOptions);
    }
}
