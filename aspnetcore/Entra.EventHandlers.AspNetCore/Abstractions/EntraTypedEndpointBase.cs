using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.Hosting.Resolvers;

namespace Entra.EventHandlers.AspNetCore.Abstractions;

public abstract class EntraTypedEndpointBase<TEvent, TResponse>(
    ILogger logger,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter,
    IEntraEventHandlerResolver resolver) : EntraEndpointBase(logger, requestAdapter, responseAdapter)
    where TEvent : EntraEvent
    where TResponse : EntraEventResponse
{
    protected IEntraEventHandlerResolver Resolver { get; } = resolver;

    protected sealed override async Task ExecuteAsync(HttpContext httpContext)
    {
        var evt = await RequestAdapter.ReadEventAsync<TEvent>(httpContext);
        var handler = Resolver.Resolve<TEvent, TResponse>();
        var result = await handler.HandleAsync(evt, httpContext.RequestAborted);

        if (result.HasException)
        {
            await OnExceptionAsync(result.Exception!, httpContext);
        }

        await ResponseAdapter.WriteOkAsync(httpContext, result.Response);
    }
}