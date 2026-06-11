using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.AspNetCore.Routing;
using Entra.EventHandlers.Hosting.Resolvers;

namespace Entra.EventHandlers.AspNetCore.Endpoints;

public sealed class EntraEventRouterEndpoint(
    ILogger<EntraEventRouterEndpoint> logger,
    IEntraEventHandlerResolver resolver,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter) : EntraEventRouterEndpointBase(logger, resolver, requestAdapter, responseAdapter)
{
    public override void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("router", InvokeAsync);
    }
}