using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;

namespace Entra.EventHandlers.Hosting.Resolvers;

/// <summary>
/// Resolves the strongly typed <see cref="IEntraEventHandler{TEvent,TResponse}"/>
/// implementation registered for the specified event and response types.
/// </summary>
/// <typeparam name="TEvent">
/// The concrete event type for which a handler should be resolved.
/// </typeparam>
/// <typeparam name="TResponse">
/// The concrete response type produced by the handler.
/// </typeparam>
/// <returns>
/// The registered <see cref="IEntraEventHandler{TEvent,TResponse}"/> implementation.
/// </returns>
/// <exception cref="EntraHandlerNotFoundException">
/// Thrown when no handler implementing
/// <see cref="IEntraEventHandler{TEvent,TResponse}"/> is registered.
/// </exception>
public interface IEntraEventHandlerResolver
{
    IEntraEventHandler<TEvent, TResponse> Resolve<TEvent, TResponse>()
        where TEvent : EntraEvent
        where TResponse : EntraEventResponse;
}

/// <inheritdoc />
public sealed class EntraEventHandlerResolver(IEnumerable<IEntraEventHandler> handlers) : IEntraEventHandlerResolver
{
    private readonly IEnumerable<IEntraEventHandler> _handlers = handlers;

    public IEntraEventHandler<TEvent, TResponse> Resolve<TEvent, TResponse>()
        where TEvent : EntraEvent
        where TResponse : EntraEventResponse
    {
        var handler = _handlers
            .OfType<IEntraEventHandler<TEvent, TResponse>>()
            .FirstOrDefault();

        return handler ?? throw new EntraHandlerNotFoundException(typeof(TEvent));
    }
}
