namespace Entra.EventHandlers.Abstractions.Errors;

public sealed class EntraHandlerNotFoundException(Type eventType)
    : Exception($"No handler registered for event type '{eventType.Name}'.")
{
}
