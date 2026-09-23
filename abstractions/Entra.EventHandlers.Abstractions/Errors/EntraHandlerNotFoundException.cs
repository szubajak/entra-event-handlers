namespace Entra.EventHandlers.Abstractions.Errors;

public sealed class EntraHandlerNotFoundException(Type eventType)
    : EntraException($"No handler registered for event type '{eventType.Name}'.")
{
    public Type EventType { get; } = eventType;
}
