using Temporalio.Common;
using Temporalio.Workflows;
using Week9_ResilientOrdersApi.Activities;

namespace Week9_ResilientOrdersApi.Workflows;

// runs the order confirmation email as a Temporal workflow so it can be retried on failure
[Workflow]
public class OrderConfirmationWorkflow
{
    [WorkflowRun]
    public async Task RunAsync(OrderConfirmationInput input)
    {
        var activityOptions = new ActivityOptions
        {
            StartToCloseTimeout = TimeSpan.FromSeconds(30),
            RetryPolicy = new RetryPolicy { MaximumAttempts = 5 }, // retry up to 5 times before giving up
        };

        await Workflow.ExecuteActivityAsync(
            (EmailActivities activities) => activities.SendOrderConfirmationEmailAsync(input),
            activityOptions);
    }
}
