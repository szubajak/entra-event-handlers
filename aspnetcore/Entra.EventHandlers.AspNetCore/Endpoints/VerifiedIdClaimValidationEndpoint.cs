using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.AspNetCore.Base;

namespace Entra.EventHandlers.AspNetCore.Endpoints;

public sealed class VerifiedIdClaimValidationEndpoint(
    ILogger<TokenIssuanceStartEndpoint> logger,
    IVerifiedIdClaimValidationHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter) : VerifiedIdClaimValidationEndpointBase(logger, handler, requestAdapter, responseAdapter)
{
    public override void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("verifiedidclaimvalidation", InvokeAsync);
    }
}