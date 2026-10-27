using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.AspNetCore.Routing;
using Entra.EventHandlers.Hosting.Orchestrators;

namespace Entra.EventHandlers.AspNetCore.Endpoints;

public sealed class EntraEventRouterEndpoint(
    ILogger<EntraEventRouterEndpoint> logger,
    IHttpAdapter httpAdapter,
    IEntraEventOrchestrator orchestrator
) : EntraEventRouterEndpointBase(logger, httpAdapter, orchestrator)
{
    public override void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("router", InvokeAsync);
    }
}
