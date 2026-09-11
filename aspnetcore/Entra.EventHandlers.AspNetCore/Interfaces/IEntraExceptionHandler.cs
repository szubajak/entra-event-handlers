namespace Entra.EventHandlers.AspNetCore.Interfaces;

/// <summary>
/// Defines a custom exception handler for Entra event processing in ASP.NET Core.
/// Implementations may override the default logging, telemetry, or diagnostic
/// behavior performed by the Entra endpoint infrastructure.
/// </summary>
/// <remarks>
/// This handler is invoked whenever an exception occurs while processing an Entra
/// event through the ASP.NET Core hosting pipeline. It allows applications to
/// implement cross‑cutting concerns such as structured logging, metrics, or
/// external notifications.
///
/// The <paramref name="isEntraException"/> flag indicates whether the exception
/// represents an expected Entra domain error (for example, validation or
/// deserialization failures) or an unexpected infrastructure/runtime failure.
///
/// <b>Important:</b> This handler does <i>not</i> control the HTTP response sent
/// back to the client. The Entra endpoint infrastructure always generates and
/// writes the final error response. Implementations should therefore avoid
/// writing to the response stream or modifying HTTP status codes.
/// </remarks>
public interface IEntraExceptionHandler
{
    /// <summary>
    /// Handles an exception thrown during Entra event processing.
    /// </summary>
    /// <param name="ex">The exception that occurred.</param>
    /// <param name="context">The current HTTP context.</param>
    /// <param name="isEntraException">
    /// Indicates whether the exception is an expected Entra domain exception.
    /// </param>
    Task HandleAsync(Exception ex, HttpContext context, bool isEntraException);
}
