using Temporalio.Client;
using Temporalio.Worker;
using Week9_ResilientOrdersApi.Activities;

namespace Week9_ResilientOrdersApi.Workflows;

// Runs for the lifetime of the app. It connects to the Temporal server,
// then starts a worker that listens on "orders-task-queue" for work to do.
// This is what actually executes OrderConfirmationWorkflow + EmailActivities
// whenever the API starts a new workflow.
public class TemporalWorkerService : BackgroundService
{
    public const string TaskQueue = "orders-task-queue";

    private readonly IConfiguration _config;
    private readonly EmailActivities _emailActivities;
    private readonly TemporalClientProvider _clientProvider;
    private readonly ILogger<TemporalWorkerService> _logger;

    public TemporalWorkerService(
        IConfiguration config,
        EmailActivities emailActivities,
        TemporalClientProvider clientProvider,
        ILogger<TemporalWorkerService> logger)
    {
        _config = config;
        _emailActivities = emailActivities;
        _clientProvider = clientProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var hostPort = _config["Temporal:HostPort"] ?? "localhost:7233";

        // Retry the initial connection - the Temporal server container might
        // still be starting up when this API container starts.
        ITemporalClient? client = null;
        while (client == null && !stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Connecting to Temporal server at {HostPort}...", hostPort);
                client = await TemporalClient.ConnectAsync(new TemporalClientConnectOptions { TargetHost = hostPort });
                _logger.LogInformation("Connected to Temporal server at {HostPort}", hostPort);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not connect to Temporal server at {HostPort} yet, retrying in 5s", hostPort);
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        if (client == null) return; // shutting down before we ever connected

        _clientProvider.SetClient(client);

        using var worker = new TemporalWorker(
            client,
            new TemporalWorkerOptions(taskQueue: TaskQueue)
                .AddAllActivities(_emailActivities)
                .AddWorkflow<OrderConfirmationWorkflow>());

        _logger.LogInformation("Temporal worker listening on task queue {TaskQueue}", TaskQueue);
        await worker.ExecuteAsync(stoppingToken);
    }
}
