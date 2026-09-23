using Entra.EventHandlers.Abstractions.Errors;

namespace Entra.EventHandlers.Abstractions.Extensions;

public static class ExceptionExtensions
{
    public static bool IsEntraException(this Exception ex) => ex is EntraException;

    public static string ToEntraErrorCode(this Exception ex) =>
        ex switch
        {
            EntraValidationException => EntraErrorCodes.ValidationError,
            EntraDeserializationException => EntraErrorCodes.DeserializationError,
            EntraHandlerNotFoundException => EntraErrorCodes.HandlerNotFound,
            EntraSecurityException => EntraErrorCodes.SecurityError,
            _ => EntraErrorCodes.UnhandledException
        };
}
