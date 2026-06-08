using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.AspNetCore.Base;

namespace Entra.EventHandlers.AspNetCore.Endpoints;

public sealed class EmailOtpSendEndpoint(
    ILogger<EmailOtpSendEndpoint> logger,
    IEmailOtpSendHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter) : EmailOtpSendEndpointBase(logger, handler, requestAdapter, responseAdapter)
{
    public override void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("emailotpsend", Invoke);
    }
}