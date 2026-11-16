using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.AzureFunctions.Routing;
using Entra.EventHandlers.Hosting.Orchestrators;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace AzureFunctionsSample.Net11.Functions;

public sealed class EntraEventRouterFunction(
    ILogger<EntraEventRouterFunction> logger,
    IHttpAdapter httpAdapter,
    IEntraEventOrchestrator orchestrator
) : EntraEventRouterFunctionBase(logger, httpAdapter, orchestrator)
{
    [Function("Router")]
    public Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "router")] HttpRequestData req) =>
        InvokeAsync(req);
}
