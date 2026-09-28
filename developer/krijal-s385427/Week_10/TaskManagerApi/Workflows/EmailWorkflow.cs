using Temporalio.Common;
using Temporalio.Workflows;
using TaskManagerApi.Models;

namespace TaskManagerApi.Workflows;

[Workflow]
public class EmailWorkflow
{
    [WorkflowRun]
    public async Task<string> RunAsync(
        EmailRequest request)
    {
        await Workflow.ExecuteActivityAsync(
            (EmailActivities activities) =>
                activities.SendEmailAsync(request),
            new ActivityOptions
            {
                StartToCloseTimeout = TimeSpan.FromSeconds(30),
                RetryPolicy = new RetryPolicy
                {
                    MaximumAttempts = 3
                }
            });

        return $"Email workflow completed for {request.Recipient}";
    }
}