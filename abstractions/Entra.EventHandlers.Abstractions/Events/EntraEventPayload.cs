using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Protocol.Authentication;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Events;

/// <summary>
/// Base type for all Microsoft Entra custom extension event payloads.
/// Contains the common fields provided by Entra for every event, including
/// tenant information, extension identifiers, authentication context, and
/// the required <c>@odata.type</c> discriminator.
/// </summary>
/// <remarks>
/// Derived payload types represent the concrete data structures for specific
/// events such as AttributeCollectionStart, AttributeCollectionSubmit, and
/// TokenIssuanceStart.
///
/// The <c>@odata.type</c> field is used by Microsoft Entra to identify the
/// concrete payload type. Implementations must ensure that the value matches
/// the expected discriminator defined by the protocol.
///
/// For an overview of Entra custom extension events and payload structure, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-extension-overview
/// </remarks>
public abstract class EntraEventPayload : IHaveOdataType
{
    /// <summary>
    /// Gets or sets the raw <c>@odata.type</c> value received from Microsoft Entra.
    /// This value is validated against the expected discriminator defined by
    /// the derived payload type.
    /// </summary>
    [JsonPropertyName("@odata.type")]
    public string? RawOdataType { get; init; }

    /// <summary>
    /// Gets the expected <c>@odata.type</c> discriminator for the payload.
    /// Derived types override this value to match the Entra protocol contract.
    /// </summary>
    [JsonIgnore]
    public abstract string OdataType { get; }

    /// <summary>
    /// Gets or sets the identifier of the tenant in which the event occurred.
    /// </summary>
    [JsonPropertyName("tenantId")]
    public Guid TenantId { get; init; }

    /// <summary>
    /// Gets or sets the identifier of the authentication event listener that
    /// triggered the custom extension.
    /// </summary>
    [JsonPropertyName("authenticationEventListenerId")]
    public Guid AuthenticationEventListenerId { get; init; }

    /// <summary>
    /// Gets or sets the identifier of the custom authentication extension
    /// associated with this event.
    /// </summary>
    [JsonPropertyName("customAuthenticationExtensionId")]
    public Guid CustomAuthenticationExtensionId { get; init; }

    /// <summary>
    /// Gets or sets the authentication context containing correlation
    /// information and other metadata relevant to the event.
    /// </summary>
    [JsonPropertyName("authenticationContext")]
    public required AuthenticationContext AuthenticationContext { get; init; }

    /// <summary>
    /// Validates that the raw <c>@odata.type</c> value matches the expected
    /// discriminator for the payload type. Throws an exception if the value
    /// does not conform to the Entra protocol contract.
    /// </summary>
    public void ValidateOdataType()
    {
        if (!string.Equals(RawOdataType, OdataType, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Invalid @odata.type. Expected '{OdataType}', got '{RawOdataType}'.");
        }
    }
}