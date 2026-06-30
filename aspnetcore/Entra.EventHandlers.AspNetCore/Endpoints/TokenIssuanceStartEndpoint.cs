using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.AspNetCore.Abstractions;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.Hosting.Resolvers;

namespace Entra.EventHandlers.AspNetCore.Endpoints;

public sealed class TokenIssuanceStartEndpoint(
    ILogger<TokenIssuanceStartEndpoint> logger,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter,
    IEntraEventHandlerResolver resolver)
    : EntraTypedEndpointBase<TokenIssuanceStartEvent, TokenIssuanceStartResponse>(
        logger, requestAdapter, responseAdapter, resolver)
{
    public override void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("tokenissuancestart", InvokeAsync);
    }
}