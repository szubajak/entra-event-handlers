using Entra.EventHandlers.AzureFunctions.Abstractions;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AzureFunctions.UnitTests.Abstractions;

public sealed class TestEntraFunctionBase(
    ILogger logger,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter)
    : EntraFunctionBase(logger, requestAdapter, responseAdapter)
{
    public Func<HttpRequestData, FunctionContext, Task<HttpResponseData>>? ExecuteDelegate { get; set; }

    protected override Task<HttpResponseData> ExecuteAsync(HttpRequestData req, FunctionContext context)
    {
        if (ExecuteDelegate is null)
            throw new InvalidOperationException("ExecuteDelegate must be set in tests.");

        return ExecuteDelegate(req, context);
    }

    public Task<HttpResponseData> Invoke(HttpRequestData req, FunctionContext context) => InvokeAsync(req, context);
}
