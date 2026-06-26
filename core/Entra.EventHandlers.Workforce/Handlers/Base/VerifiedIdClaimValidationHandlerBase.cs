using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Workforce.Builders;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Entra.EventHandlers.Workforce.Handlers.Base;

/// <summary>
/// Provides a base implementation of <see cref="IVerifiedIdClaimValidationHandler"/>
/// that applies shared processing behavior such as structured logging,
/// correlation scoping, execution timing, and exception handling.
/// Derived classes should override <see cref="HandleCoreAsync"/> to implement
/// event‑specific business logic.
/// This base class also validates incoming events according to the Entra
/// protocol contract before invoking handler logic.
/// </summary>
public abstract class VerifiedIdClaimValidationHandlerBase(ILogger logger) : IVerifiedIdClaimValidationHandler
{
    protected ILogger Logger { get; } = logger;

    /// <summary>
    /// Handles the VerifiedIdClaimValidation event by performing protocol‑level
    /// validation, establishing a correlation logging scope, measuring execution
    /// duration, and applying consistent exception handling.
    /// </summary>
    /// <remarks>
    /// If an unhandled exception occurs, the handler returns a response
    /// indicating failed claim validation. This ensures that account recovery
    /// does not proceed when the validation logic cannot complete safely.
    /// </remarks>
    public async Task<VerifiedIdClaimValidationResponse> HandleAsync(VerifiedIdClaimValidationEvent request, CancellationToken cancellationToken = default)
    {
        using var scope = Logger.BeginScope(new Dictionary<string, object?>
        {
            ["CorrelationId"] = request.CorrelationId,
            ["EventType"] = request.Type,
            ["EventName"] = request.GetType().Name
        });

        var sw = Stopwatch.StartNew();

        Logger.LogInformation("Handling event");

        try
        {
            request.Validate();

            var response = await HandleCoreAsync(request, cancellationToken);

            sw.Stop();

            var actionType = response.Data.Actions.FirstOrDefault()?.OdataType ?? "None";

            Logger.LogInformation(
                "Successfully handled event. DurationMs={Duration}, Action={ActionType}",
                sw.ElapsedMilliseconds,
                actionType);

            return response!;
        }
        catch (Exception ex)
        {
            sw.Stop();

            Logger.LogError(
                ex,
                "Unhandled exception. DurationMs={Duration}",
                sw.ElapsedMilliseconds);

            return EntraWorkforceEventResponses
                .VerifiedIdClaimValidation()
                .Failed([])
                .Build();
        }
    }

    /// <summary>
    /// Contains the event‑specific business logic for handling the
    /// VerifiedIdClaimValidation event. Implementations should override
    /// this method instead of <see cref="HandleAsync"/>.
    /// </summary>
    protected abstract Task<VerifiedIdClaimValidationResponse> HandleCoreAsync(VerifiedIdClaimValidationEvent request, CancellationToken cancellationToken);
}
