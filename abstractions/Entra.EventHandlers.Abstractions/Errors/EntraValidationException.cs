namespace Entra.EventHandlers.Abstractions.Errors;

public sealed class EntraValidationException(string message) : EntraException(message) { }
