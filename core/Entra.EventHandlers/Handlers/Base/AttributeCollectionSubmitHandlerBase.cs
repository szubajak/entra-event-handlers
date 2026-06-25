using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Entra.EventHandlers.Handlers.Base;

/// <summary>
/// Provides a base implementation of <see cref="IAttributeCollectionSubmitHandler"/>
/// that applies shared processing behavior such as structured logging,
/// correlation scoping, execution timing, and exception handling.
/// Derived classes should override <see cref="HandleCoreAsync"/> to implement
/// event‑specific business logic.
/// This base class also validates incoming events according to the Entra
/// protocol contract before invoking handler logic.
/// </summary>
public abstract class AttributeCollectionSubmitHandlerBase(ILogger logger) : IAttributeCollectionSubmitHandler
{
    protected ILogger Logger { get; } = logger;

    /// <remarks>
    /// This method performs protocol‑level validation (including <c>@odata.type</c>
    /// verification), establishes a logging scope with correlation identifiers,
    /// measures execution duration, and applies consistent exception handling.
    /// </remarks>
    public async Task<AttributeCollectionSubmitResponse> HandleAsync(AttributeCollectionSubmitEvent request, CancellationToken cancellationToken)
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

            Logger.LogInformation(
                "Successfully handled event. DurationMs={Duration}, Action={ActionType}",
                sw.ElapsedMilliseconds,
                response?.Data?.Actions?.FirstOrDefault()?.OdataType);

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
                .AttributeCollectionSubmit()
                .ShowBlockPage("Error", "Unexpected error occurred.")
                .Build();
        }
    }

    /// <summary>
    /// Contains the event‑specific business logic for handling the
    /// AttributeCollectionSubmit event. Implementations should override
    /// this method instead of <see cref="HandleAsync"/>.
    /// </summary>
    protected abstract Task<AttributeCollectionSubmitResponse> HandleCoreAsync(AttributeCollectionSubmitEvent request, CancellationToken cancellationToken);
}
