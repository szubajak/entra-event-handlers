using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AspNetCore.Abstractions;
using Entra.EventHandlers.AspNetCore.Adapters;

namespace Entra.EventHandlers.AspNetCore.Base;

public abstract class EmailOtpSendEndpointBase(
    ILogger logger,
    IEmailOtpSendHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter) : EntraEndpointBase(logger, requestAdapter, responseAdapter)
{
    private readonly IEmailOtpSendHandler _handler = handler;

    protected override async Task Execute(HttpContext httpContext)
    {
        var evt = await RequestAdapter.ReadEvent<EmailOtpSendEvent>(httpContext);
        var response = await _handler.Handle(evt, httpContext.RequestAborted);
        await ResponseAdapter.WriteOk(httpContext, response);
    }
}