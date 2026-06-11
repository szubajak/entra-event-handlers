using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.AspNetCore.Base;

namespace Entra.EventHandlers.AspNetCore.Endpoints;

public sealed class TokenIssuanceStartEndpoint(
    ILogger<TokenIssuanceStartEndpoint> logger,
    ITokenIssuanceStartHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter) : TokenIssuanceStartEndpointBase(logger, handler, requestAdapter, responseAdapter)
{
    public override void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("tokenissuancestart", InvokeAsync);
    }
}