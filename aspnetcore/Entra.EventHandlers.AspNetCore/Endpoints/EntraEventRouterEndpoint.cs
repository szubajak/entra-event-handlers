using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.AspNetCore.Routing;
using Entra.EventHandlers.Hosting.Orchestrators;

namespace Entra.EventHandlers.AspNetCore.Endpoints;

public sealed class EntraEventRouterEndpoint(
    ILogger<EntraEventRouterEndpoint> logger,
    IEntraEventOrchestrator orchestrator,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter) : EntraEventRouterEndpointBase(logger, orchestrator, requestAdapter, responseAdapter)
{
    public override void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("router", InvokeAsync);
    }
}