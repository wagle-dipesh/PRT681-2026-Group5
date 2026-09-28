using Temporalio.Client;

namespace Week9_ResilientOrdersApi.Workflows;

// The API's minimal-api handlers need a connected TemporalClient to start
// workflows, but connecting takes a moment at startup. This small class lets
// the background worker service hand the connected client to the rest of
// the app once it's ready, instead of every caller connecting separately.
public class TemporalClientProvider
{
    private readonly TaskCompletionSource<ITemporalClient> _clientReady = new();

    public void SetClient(ITemporalClient client) => _clientReady.TrySetResult(client);

    public Task<ITemporalClient> GetClientAsync() => _clientReady.Task;
}
