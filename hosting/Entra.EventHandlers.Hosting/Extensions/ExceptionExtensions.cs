using Entra.EventHandlers.Abstractions.Errors;

namespace Entra.EventHandlers.Hosting.Extensions;

public static class ExceptionExtensions
{
    public static bool IsEntraException(this Exception ex) =>
        ex is EntraValidationException
        or EntraDeserializationException
        or EntraHandlerNotFoundException;

    public static string ToEntraErrorCode(this Exception ex) =>
        ex switch
        {
            EntraValidationException => EntraErrorCodes.ValidationError,
            EntraDeserializationException => EntraErrorCodes.DeserializationError,
            EntraHandlerNotFoundException => EntraErrorCodes.HandlerNotFound,
            _ => EntraErrorCodes.UnhandledException
        };
}
