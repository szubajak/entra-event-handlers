namespace Entra.EventHandlers.Abstractions.Errors;

public sealed class EntraDeserializationException(string message)
    : Exception(message)
{
}
