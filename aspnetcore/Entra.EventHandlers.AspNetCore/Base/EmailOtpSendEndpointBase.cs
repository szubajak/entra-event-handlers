using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.AspNetCore.Abstractions;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.Hosting.Resolvers;

namespace Entra.EventHandlers.AspNetCore.Base;

public abstract class EmailOtpSendEndpointBase(
    ILogger logger,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter,
    IEntraEventHandlerResolver resolver) : EntraSingleEndpointBase(logger, requestAdapter, responseAdapter, resolver)
{
    protected sealed override async Task ExecuteAsync(HttpContext httpContext)
    {
        var evt = await RequestAdapter.ReadEventAsync<EmailOtpSendEvent>(httpContext);
        var handler = Resolver.Resolve<EmailOtpSendEvent, EmailOtpSendResponse>();
        var response = await handler.HandleAsync(evt, httpContext.RequestAborted);
        await ResponseAdapter.WriteOkAsync(httpContext, response);
    }
}