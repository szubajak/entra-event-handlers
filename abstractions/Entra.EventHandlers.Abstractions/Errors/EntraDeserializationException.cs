namespace Entra.EventHandlers.Abstractions.Errors;

public sealed class EntraDeserializationException : Exception
{
    public EntraDeserializationException(string message)
        : base(message)
    {
    }

    public EntraDeserializationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
