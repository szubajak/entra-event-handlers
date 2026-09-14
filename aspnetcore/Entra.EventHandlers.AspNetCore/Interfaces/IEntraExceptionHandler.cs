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
/// </remarks>
public interface IEntraExceptionHandler
{
    /// <summary>
    /// Handles an exception thrown during Entra event processing.
    /// </summary>
    /// <param name="ex">The exception that occurred.</param>
    Task HandleAsync(Exception ex);
}
