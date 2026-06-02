using Entra.EventHandlers.Abstractions.Protocol;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Events;

/// <summary>
/// Base type for all Microsoft Entra custom extension events.
/// Represents the incoming event envelope received from Entra before it is
/// dispatched to a specific handler.
/// </summary>
/// <remarks>
/// This type participates in polymorphic deserialization using the
/// <c>type</c> discriminator field. Each derived event type specifies its
/// own discriminator value via <see cref="JsonDerivedTypeAttribute"/>.
///
/// For an overview of Entra custom extension events, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-extension-overview
/// </remarks>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(AttributeCollectionStartEvent), EntraEventTypes.AttributeCollectionStart)]
[JsonDerivedType(typeof(AttributeCollectionSubmitEvent), EntraEventTypes.AttributeCollectionSubmit)]
[JsonDerivedType(typeof(TokenIssuanceStartEvent), EntraEventTypes.TokenIssuanceStart)]
[JsonDerivedType(typeof(EmailOtpSendEvent), EntraEventTypes.EmailOtpSend)]
public abstract class EntraEvent
{
    /// <summary>
    /// Gets the event type discriminator used by Microsoft Entra to identify
    /// the specific event being processed.
    /// </summary>
    [JsonIgnore]
    public abstract string Type { get; }

    /// <summary>
    /// Gets the correlation identifier associated with the event. This value
    /// is provided by Microsoft Entra and is useful for tracing and diagnostics.
    /// </summary>
    [JsonIgnore]
    public abstract Guid CorrelationId { get; }
}

/// <summary>
/// Base type for Microsoft Entra custom extension events that include a
/// strongly-typed payload.
/// </summary>
/// <typeparam name="TPayload">
/// The type of the event payload containing the data sent by Microsoft Entra.
/// </typeparam>
/// <remarks>
/// This class provides common infrastructure for all event types, including
/// access to the <c>source</c> and <c>data</c> fields, correlation tracking,
/// and payload validation.
/// </remarks>
public abstract class EntraEvent<TPayload> : EntraEvent
    where TPayload : EntraEventPayload
{
    /// <summary>
    /// Gets or sets the source of the event as provided by Microsoft Entra.
    /// </summary>
    [JsonPropertyName("source")]
    public required string Source { get; init; }

    /// <summary>
    /// Gets or sets the strongly-typed payload containing event-specific data.
    /// </summary>
    [JsonPropertyName("data")]
    public required TPayload Data { get; init; }

    /// <summary>
    /// Gets the correlation identifier extracted from the event payload.
    /// </summary>
    public override Guid CorrelationId => Data.AuthenticationContext.CorrelationId;

    /// <summary>
    /// Validates the payload, ensuring that the <c>@odata.type</c> discriminator
    /// is present and correct according to the Entra protocol contract.
    /// </summary>
    public void Validate()
    {
        Data.ValidateOdataType();
    }
}