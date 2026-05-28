using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Responses;

/// <summary>
/// Base type for all responses returned to Microsoft Entra from a custom
/// extension handler. Represents the response envelope used by all event
/// response types.
/// </summary>
/// <remarks>
/// Concrete response types derive from this class and include a strongly-typed
/// <c>data</c> payload that defines the actions Entra should perform next.
///
/// For details on Entra custom extension response schemas, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-extension-overview
/// </remarks>
public abstract class EntraEventResponse { }

/// <summary>
/// Base type for Microsoft Entra custom extension responses that include a
/// strongly-typed payload.
/// </summary>
/// <typeparam name="TPayload">
/// The type of the response payload containing the actions or data that
/// Microsoft Entra should apply for the event.
/// </typeparam>
/// <remarks>
/// Derived response types populate the <c>data</c> property with the specific
/// instructions required by the Entra custom extension contract.
/// </remarks>
public abstract class EntraEventResponse<TPayload> : EntraEventResponse
    where TPayload : EntraEventResponsePayload
{
    /// <summary>
    /// Gets or sets the strongly-typed response payload that defines the
    /// actions Microsoft Entra should perform next.
    /// </summary>
    [JsonPropertyName("data")]
    public TPayload? Data { get; set; }
}