using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.AzureFunctions.Routing;
using Entra.EventHandlers.Hosting.Orchestrators;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AzureFunctions.UnitTests.Routing;

public sealed class TestEntraEventRouterFunctionBase(ILogger logger, IHttpAdapter httpAdapter, IEntraEventOrchestrator orchestrator)
    : EntraEventRouterFunctionBase(logger, httpAdapter, orchestrator)
{
    public bool ExceptionCalled { get; private set; }

    protected override Task OnExceptionAsync(Exception ex)
    {
        ExceptionCalled = true;
        return Task.CompletedTask;
    }

    public Task<HttpResponseData> RunAsync(HttpRequestData req) => InvokeAsync(req);
}
