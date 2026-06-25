using Entra.EventHandlers.AzureFunctions.Abstractions;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AzureFunctions.UnitTests.Abstractions;

public sealed class TestEntraFunctionBase(
    ILogger logger,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter)
    : EntraFunctionBase(logger, requestAdapter, responseAdapter)
{
    public Func<HttpRequestData, Task<HttpResponseData>>? ExecuteDelegate { get; set; }

    protected override Task<HttpResponseData> ExecuteAsync(HttpRequestData req)
    {
        if (ExecuteDelegate is null)
            throw new InvalidOperationException("ExecuteDelegate must be set in tests.");

        return ExecuteDelegate(req);
    }

    public Task<HttpResponseData> Invoke(HttpRequestData req) => InvokeAsync(req);
}
