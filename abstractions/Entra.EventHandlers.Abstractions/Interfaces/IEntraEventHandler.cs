using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Abstractions.Results;

namespace Entra.EventHandlers.Abstractions.Interfaces;

/// <summary>
/// Marker interface for all Microsoft Entra event handlers.
/// This non-generic contract allows the pipeline to work with handlers
/// in a type-agnostic manner (e.g., for discovery, registration, or
/// dependency injection).
/// </summary>
/// <remarks>
/// All concrete handlers implement the generic
/// <see cref="IEntraEventHandler{TRequest, TResponse}"/> interface, which
/// defines the strongly-typed event processing contract.
///
/// This interface exists to support scenarios where handlers must be
/// referenced, resolved, or inspected without knowing their specific
/// request/response types.
/// </remarks>
public interface IEntraEventHandler { }

/// <summary>
/// Defines the contract for handling Microsoft Entra custom extension events.
/// Implementations receive a strongly‑typed event payload and return a
/// <see cref="EntraHandlerResult{TResponse}"/> containing both the protocol
/// response or captured exceptions.
/// </summary>
/// <typeparam name="TRequest">
/// The type of the incoming event model representing the request sent by
/// Microsoft Entra.
/// </typeparam>
/// <typeparam name="TResponse">
/// The type of the protocol response model that the handler must produce.
/// This response instructs Microsoft Entra how to proceed with the
/// authentication flow.
/// </typeparam>
/// <remarks>
/// This interface is the foundation of the Entra Event Handlers pipeline.
/// Each event type (e.g., AttributeCollectionStart, AttributeCollectionSubmit,
/// TokenIssuanceStart) has a corresponding handler interface that specializes
/// this generic contract.
///
/// The returned <see cref="EntraHandlerResult{TResponse}"/> does not modify
/// the Entra protocol. It simply wraps the protocol response with additional
/// metadata used by the hosting layer (e.g., Azure Functions or ASP.NET Core)
/// 
/// For details on Entra custom extension events and response schemas, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-extension-overview
/// </remarks>
public interface IEntraEventHandler<TRequest, TResponse> : IEntraEventHandler
    where TRequest : EntraEvent
    where TResponse : EntraEventResponse
{
    Task<EntraHandlerResult<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
