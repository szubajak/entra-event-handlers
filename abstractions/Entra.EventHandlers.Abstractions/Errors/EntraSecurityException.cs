namespace Entra.EventHandlers.Abstractions.Errors;

public sealed class EntraSecurityException : EntraException
{
    public EntraSecurityException(string message) : base(message) { }

    public EntraSecurityException(string message, Exception innerException) : base(message, innerException) { }
}
