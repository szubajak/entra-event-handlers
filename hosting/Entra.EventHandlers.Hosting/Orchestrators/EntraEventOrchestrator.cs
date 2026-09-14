using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Abstractions.Results;
using Entra.EventHandlers.Hosting.Resolvers;

namespace Entra.EventHandlers.Hosting.Orchestrators;

/// <summary>
/// Coordinates the execution of Microsoft Entra External ID events by routing the
/// incoming <see cref="EntraEvent"/> instance to its corresponding strongly typed
/// <see cref="IEntraEventHandler{TEvent,TResponse}"/> implementation.
/// </summary>
public interface IEntraEventOrchestrator
{
    /// <summary>
    /// Dispatches the specified <paramref name="evt"/> to the appropriate event handler
    /// based on its concrete runtime type and returns the resulting response.
    /// </summary>
    /// <param name="evt">The deserialized event instance to process.</param>
    /// <param name="cancellationToken">A token that may be used to cancel the operation.</param>
    /// <returns>
    /// The <see cref="EntraEventResponse"/> produced by the resolved handler.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Thrown when the event type is not recognized or is not supported by the orchestrator.
    /// </exception>
    Task<EntraHandlerResult<EntraEventResponse>> DispatchAsync(EntraEvent evt, CancellationToken cancellationToken = default);
}

/// <inheritdoc />
public class EntraEventOrchestrator(IEntraEventHandlerResolver resolver) : IEntraEventOrchestrator
{
    private readonly IEntraEventHandlerResolver _resolver = resolver;

    public Task<EntraHandlerResult<EntraEventResponse>> DispatchAsync(EntraEvent evt, CancellationToken cancellationToken = default) =>
        evt switch
        {
            AttributeCollectionStartEvent e =>
                DispatchTypedAsync<AttributeCollectionStartEvent, AttributeCollectionStartResponse>(e, cancellationToken),

            AttributeCollectionSubmitEvent e =>
                DispatchTypedAsync<AttributeCollectionSubmitEvent, AttributeCollectionSubmitResponse>(e, cancellationToken),

            TokenIssuanceStartEvent e =>
                DispatchTypedAsync<TokenIssuanceStartEvent, TokenIssuanceStartResponse>(e, cancellationToken),

            EmailOtpSendEvent e =>
                DispatchTypedAsync<EmailOtpSendEvent, EmailOtpSendResponse>(e, cancellationToken),

            PasswordSubmitEvent e =>
                DispatchTypedAsync<PasswordSubmitEvent, PasswordSubmitResponse>(e, cancellationToken),

            VerifiedIdClaimValidationEvent e =>
                DispatchTypedAsync<VerifiedIdClaimValidationEvent, VerifiedIdClaimValidationResponse>(e, cancellationToken),

            _ => throw new NotSupportedException($"Unsupported event type: {evt.GetType().Name}")
        };

    private async Task<EntraHandlerResult<EntraEventResponse>> DispatchTypedAsync<TEvent, TResponse>(TEvent evt, CancellationToken cancellationToken = default)
        where TEvent : EntraEvent
        where TResponse : EntraEventResponse
    {
        var handler = _resolver.Resolve<TEvent, TResponse>();

        var result = await handler.HandleAsync(evt, cancellationToken);

        return new EntraHandlerResult<EntraEventResponse>(result.Response, result.Exception);
    }
}
