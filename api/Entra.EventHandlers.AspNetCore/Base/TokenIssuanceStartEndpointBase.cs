using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AspNetCore.Adapters;
using Microsoft.AspNetCore.Http;

namespace Entra.EventHandlers.AspNetCore.Base;

public abstract class TokenIssuanceStartEndpointBase(
    ITokenIssuanceStartHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter)
{
    private readonly ITokenIssuanceStartHandler _handler = handler;
    private readonly IRequestAdapter _requestAdapter = requestAdapter;
    private readonly IResponseAdapter _responseAdapter = responseAdapter;

    public async Task Invoke(HttpContext httpContext)
    {
        var evt = await _requestAdapter.ReadEvent<TokenIssuanceStartEvent>(httpContext);
        var response = await _handler.Handle(evt, httpContext.RequestAborted);
        await _responseAdapter.WriteOk(httpContext, response);
    }
}