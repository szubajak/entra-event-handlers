using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Extensions;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Abstractions.Results;
using Entra.EventHandlers.Builders;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Entra.EventHandlers.Handlers.Base;

/// <summary>
/// Provides a base implementation of <see cref="IAttributeCollectionStartHandler"/>
/// that applies shared processing behavior such as structured logging,
/// correlation scoping, execution timing, and exception handling.
/// Derived classes should override <see cref="HandleCoreAsync"/> to implement
/// event‑specific business logic.
/// This base class also validates incoming events according to the Entra
/// protocol contract before invoking handler logic.
/// </summary>
public abstract class AttributeCollectionStartHandlerBase(ILogger logger) : IAttributeCollectionStartHandler
{
    protected ILogger Logger { get; } = logger;

    /// <remarks>
    /// This method performs protocol-level validation (including <c>@odata.type</c>
    /// verification), establishes a logging scope with correlation identifiers,
    /// measures execution duration, and applies consistent exception handling.
    /// </remarks>
    public async Task<EntraHandlerResult<AttributeCollectionStartResponse>> HandleAsync(AttributeCollectionStartEvent request, CancellationToken cancellationToken = default)
    {
        using var scope = Logger.BeginScope(new Dictionary<string, object?>
        {
            ["CorrelationId"] = request.CorrelationId,
            ["EventType"] = request.Type,
            ["EventName"] = request.GetType().Name
        });

        var sw = Stopwatch.StartNew();

        Logger.LogInformation("Starting Entra event handling.");

        try
        {
            request.Validate();

            var response = await HandleCoreAsync(request, cancellationToken);

            sw.Stop();

            var actionType = response.Data.Actions.FirstOrDefault()?.OdataType ?? "None";

            Logger.LogInformation(
                "Entra event handled successfully. DurationMs={DurationMs}, Action={ActionType}.",
                sw.ElapsedMilliseconds,
                actionType);

            return new EntraHandlerResult<AttributeCollectionStartResponse>(response);
        }
        catch (Exception ex)
        {
            sw.Stop();

            if (ex.IsEntraException())
            {
                Logger.LogWarning(
                    ex,
                    "Entra domain exception occurred during Entra event handling. DurationMs={DurationMs}.",
                    sw.ElapsedMilliseconds);
            }
            else
            {
                Logger.LogError(
                    ex,
                    "Unexpected failure occurred during Entra event handling. DurationMs={DurationMs}.",
                    sw.ElapsedMilliseconds);
            }

            var defaultResponse = EntraEventResponses.AttributeCollectionStart()
                .ShowBlockPage("Error", "Unexpected error occurred.")
                .Build();

            return new EntraHandlerResult<AttributeCollectionStartResponse>(defaultResponse, ex);
        }
    }

    /// <summary>
    /// Contains the event‑specific business logic for handling the
    /// AttributeCollectionStart event. Implementations should override
    /// this method instead of <see cref="HandleAsync"/>.
    /// </summary>
    protected abstract Task<AttributeCollectionStartResponse> HandleCoreAsync(AttributeCollectionStartEvent request, CancellationToken cancellationToken = default);
}
