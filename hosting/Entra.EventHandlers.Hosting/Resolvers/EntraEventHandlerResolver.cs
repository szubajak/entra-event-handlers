using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;

namespace Entra.EventHandlers.Hosting.Resolvers;

/// <summary>
/// Resolves the strongly typed <see cref="IEntraEventHandler{TEvent,TResponse}"/>
/// implementation for the specified event and response types.
/// </summary>
/// <typeparam name="TEvent">The concrete event type to resolve a handler for.</typeparam>
/// <typeparam name="TResponse">The expected response type produced by the handler.</typeparam>
/// <returns>
/// The registered <see cref="IEntraEventHandler{TEvent,TResponse}"/> implementation.
/// </returns>
/// <exception cref="EntraHandlerNotFoundException">
/// Thrown when no handler is registered for <typeparamref name="TEvent"/>.
/// </exception>
/// <exception cref="InvalidOperationException">
/// Thrown when the resolved handler does not implement
/// <see cref="IEntraEventHandler{TEvent,TResponse}"/>.
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
        var handler = Resolve(typeof(TEvent));

        if (handler is IEntraEventHandler<TEvent, TResponse> typed)
            return typed;

        throw new InvalidOperationException(
            $"Handler for event {typeof(TEvent).Name} does not implement IEntraEventHandler<{typeof(TEvent).Name}, {typeof(TResponse).Name}>");
    }

    private IEntraEventHandler Resolve(Type eventType) =>
        _handlers.FirstOrDefault(h =>
            h.GetType()
             .GetInterfaces()
             .Any(i =>
                 i.IsGenericType &&
                 i.GetGenericTypeDefinition() == typeof(IEntraEventHandler<,>) &&
                 i.GetGenericArguments()[0] == eventType))
        ?? throw new EntraHandlerNotFoundException(eventType);
}
