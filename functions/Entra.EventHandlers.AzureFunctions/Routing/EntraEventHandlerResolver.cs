using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Interfaces;

namespace Entra.EventHandlers.AzureFunctions.Routing;

/// <summary>
/// Resolves the correct <see cref="IEntraEventHandler"/> implementation
/// for a given event type at runtime.
/// </summary>
public interface IEntraEventHandlerResolver
{
    IEntraEventHandler Resolve(Type eventType);
}

/// <inheritdoc />
public sealed class EntraEventHandlerResolver(IEnumerable<IEntraEventHandler> handlers) : IEntraEventHandlerResolver
{
    private readonly IEnumerable<IEntraEventHandler> _handlers = handlers;

    public IEntraEventHandler Resolve(Type eventType) =>
        _handlers.FirstOrDefault(h =>
            h.GetType()
             .GetInterfaces()
             .Any(i =>
                 i.IsGenericType &&
                 i.GetGenericTypeDefinition() == typeof(IEntraEventHandler<,>) &&
                 i.GetGenericArguments()[0] == eventType))
        ?? throw new EntraHandlerNotFoundException(eventType);
}
