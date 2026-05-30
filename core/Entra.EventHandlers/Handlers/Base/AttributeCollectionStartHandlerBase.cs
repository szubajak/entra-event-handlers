using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Entra.EventHandlers.Handlers.Base;

/// <summary>
/// Provides a base implementation of <see cref="IAttributeCollectionStartHandler"/>
/// that applies shared processing behavior such as structured logging,
/// correlation scoping, execution timing, and exception handling.
/// Derived classes should override <see cref="HandleCore"/> to implement
/// event‑specific business logic.
/// This base class also validates incoming events according to the Entra
/// protocol contract before invoking handler logic.
/// </summary>
public abstract class AttributeCollectionStartHandlerBase(ILogger<AttributeCollectionStartHandlerBase> logger) : IAttributeCollectionStartHandler
{
    private readonly ILogger<AttributeCollectionStartHandlerBase> _logger =  logger;

    /// <remarks>
    /// This method performs protocol-level validation (including <c>@odata.type</c>
    /// verification), establishes a logging scope with correlation identifiers,
    /// measures execution duration, and applies consistent exception handling.
    /// </remarks>
    public async Task<AttributeCollectionStartResponse> Handle(AttributeCollectionStartEvent request, CancellationToken cancellationToken)
    {
        using var scope = _logger.BeginScope(new Dictionary<string, object?>
        {
            ["CorrelationId"] = request.CorrelationId,
            ["EventType"] = request.Type,
            ["EventName"] = nameof(AttributeCollectionStartEvent)
        });

        var sw = Stopwatch.StartNew();

        _logger.LogInformation("Handling event");

        try
        {
            request.Validate();

            var response = await HandleCore(request, cancellationToken);

            sw.Stop();

            _logger.LogInformation(
                "Successfully handled event. DurationMs={Duration}, Action={ActionType}",
                sw.ElapsedMilliseconds,
                response?.Data?.Actions?.FirstOrDefault()?.OdataType);

            return response!;
        }
        catch (Exception ex)
        {
            sw.Stop();

            _logger.LogError(
                ex,
                "Unhandled exception. DurationMs={Duration}",
                sw.ElapsedMilliseconds);

            return EntraEventResponses
                .AttributeCollectionStart()
                .ShowBlockPage("Error", "Unexpected error occurred.")
                .Build();
        }
    }

    /// <summary>
    /// Contains the event‑specific business logic for handling the
    /// AttributeCollectionStart event. Implementations should override
    /// this method instead of <see cref="Handle"/>.
    /// </summary>
    protected abstract Task<AttributeCollectionStartResponse> HandleCore(AttributeCollectionStartEvent request, CancellationToken cancellationToken);
}
