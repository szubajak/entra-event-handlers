namespace Entra.EventHandlers.Abstractions.Errors;

public abstract class EntraException : Exception
{
    protected EntraException(string message) : base(message) { }

    protected EntraException(string message, Exception innerException) : base(message, innerException) { }
}
