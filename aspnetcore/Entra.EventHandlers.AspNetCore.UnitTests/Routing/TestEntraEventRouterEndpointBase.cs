using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.AspNetCore.Routing;
using Entra.EventHandlers.Hosting.Orchestrators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AspNetCore.UnitTests.Routing;

public sealed class TestEntraEventRouterEndpointBase(ILogger logger, IHttpAdapter httpAdapter, IEntraEventOrchestrator orchestrator)
    : EntraEventRouterEndpointBase(logger, httpAdapter, orchestrator)
{
    protected override string Route => "test-route";

    public Task Invoke(HttpContext ctx) => InvokeAsync(ctx);
}
