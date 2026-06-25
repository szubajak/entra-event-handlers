using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.AspNetCore.Routing;
using Entra.EventHandlers.Hosting.Orchestrators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.AspNetCore.UnitTests.Routing;

public sealed class TestEntraEventRouterEndpointBase(
    ILogger logger,
    IEntraEventOrchestrator orchestrator,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter)
    : EntraEventRouterEndpointBase(logger, orchestrator, requestAdapter, responseAdapter)
{
    public Task Invoke(HttpContext ctx) => InvokeAsync(ctx);

    public override void Map(IEndpointRouteBuilder endpoints) =>
        throw new NotSupportedException("Not needed for tests.");
}
