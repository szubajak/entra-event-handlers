using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AspNetCore.Abstractions;
using Entra.EventHandlers.AspNetCore.Adapters;

namespace Entra.EventHandlers.AspNetCore.Base;

public abstract class TokenIssuanceStartEndpointBase(
    ILogger<TokenIssuanceStartEndpointBase> logger,
    ITokenIssuanceStartHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter) : EntraEndpointBase(logger, requestAdapter, responseAdapter)
{
    private readonly ITokenIssuanceStartHandler _handler = handler;

    protected override async Task Execute(HttpContext httpContext)
    {
        var evt = await RequestAdapter.ReadEvent<TokenIssuanceStartEvent>(httpContext);
        var response = await _handler.Handle(evt, httpContext.RequestAborted);
        await ResponseAdapter.WriteOk(httpContext, response);
    }
}