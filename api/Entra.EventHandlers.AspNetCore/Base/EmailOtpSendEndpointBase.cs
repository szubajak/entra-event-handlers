using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AspNetCore.Abstractions;
using Entra.EventHandlers.AspNetCore.Adapters;
using Microsoft.AspNetCore.Http;

namespace Entra.EventHandlers.AspNetCore.Base;

public abstract class EmailOtpSendEndpointBase(
    IEmailOtpSendHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter)
    : EntraEndpointBase(requestAdapter, responseAdapter)
{
    private readonly IEmailOtpSendHandler _handler = handler;

    protected override async Task Invoke(HttpContext httpContext)
    {
        var evt = await RequestAdapter.ReadEvent<EmailOtpSendEvent>(httpContext);
        var response = await _handler.Handle(evt, httpContext.RequestAborted);
        await ResponseAdapter.WriteOk(httpContext, response);
    }
}