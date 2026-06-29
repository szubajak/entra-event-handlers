using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.AspNetCore.Abstractions;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.Hosting.Resolvers;

namespace Entra.EventHandlers.AspNetCore.Base;

public abstract class TokenIssuanceStartEndpointBase(
    ILogger logger,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter,
    IEntraEventHandlerResolver resolver) : EntraSingleEndpointBase(logger, requestAdapter, responseAdapter, resolver)
{
    protected sealed override async Task ExecuteAsync(HttpContext httpContext)
    {
        var evt = await RequestAdapter.ReadEventAsync<TokenIssuanceStartEvent>(httpContext);
        var handler = Resolver.Resolve<TokenIssuanceStartEvent, TokenIssuanceStartResponse>();
        var response = await handler.HandleAsync(evt, httpContext.RequestAborted);
        await ResponseAdapter.WriteOkAsync(httpContext, response);
    }
}