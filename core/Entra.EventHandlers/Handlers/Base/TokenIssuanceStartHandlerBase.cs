using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Entra.EventHandlers.Handlers.Base;

/// <summary>
/// Provides a base implementation of <see cref="ITokenIssuanceStartHandler"/>
/// that applies shared processing behavior such as structured logging,
/// correlation scoping, execution timing, and exception handling.
/// Derived classes should override <see cref="HandleCoreAsync"/> to implement
/// event‑specific business logic.
/// This base class also validates incoming events according to the Entra
/// protocol contract before invoking handler logic.
/// </summary>
public abstract class TokenIssuanceStartHandlerBase(ILogger logger) : ITokenIssuanceStartHandler
{
    protected ILogger Logger { get; } = logger;

    /// <remarks>
    /// This method performs protocol‑level validation (including <c>@odata.type</c>
    /// verification), establishes a logging scope with correlation identifiers,
    /// measures execution duration, and applies consistent exception handling.
    /// In case of failure, a valid response containing an empty claim set is
    /// returned to ensure token issuance continues without interruption.
    /// </remarks>
    public async Task<TokenIssuanceStartResponse> HandleAsync(TokenIssuanceStartEvent request, CancellationToken cancellationToken)
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

            var actionTypes = response.Data.Actions.Any()
                ? string.Join(",", response.Data.Actions.Select(a => a.OdataType))
                : "None";

            Logger.LogInformation(
                "Successfully handled event. DurationMs={Duration}, Actions={ActionTypes}",
                sw.ElapsedMilliseconds,
                actionTypes);

            return response!;
        }
        catch (Exception ex)
        {
            sw.Stop();

            Logger.LogError(
                ex,
                "Unhandled exception. DurationMs={Duration}",
                sw.ElapsedMilliseconds);

            return EntraEventResponses
                .TokenIssuanceStart()
                .ProvideClaimsForToken([])
                .Build();
        }
    }

    /// <summary>
    /// Contains the event‑specific business logic for handling the
    /// TokenIssuanceStart event. Implementations should override
    /// this method instead of <see cref="HandleAsync"/>.
    /// </summary>
    protected abstract Task<TokenIssuanceStartResponse> HandleCoreAsync(TokenIssuanceStartEvent request, CancellationToken cancellationToken);
}
