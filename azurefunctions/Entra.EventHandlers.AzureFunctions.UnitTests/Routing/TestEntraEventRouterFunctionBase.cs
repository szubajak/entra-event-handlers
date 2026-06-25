using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.AzureFunctions.Routing;
using Entra.EventHandlers.Hosting.Orchestrators;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AzureFunctions.UnitTests.Routing;

public sealed class TestEntraEventRouterFunctionBase(
    ILogger logger,
    IEntraEventOrchestrator orchestrator,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter)
    : EntraEventRouterFunctionBase(logger, orchestrator, requestAdapter, responseAdapter)
{
    public Task<HttpResponseData> RunAsync(HttpRequestData req) =>
        InvokeAsync(req);
}
