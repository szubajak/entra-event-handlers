using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Protocol.VerifiedId;

/// <summary>
/// Represents the set of verified ID claims and additional metadata provided
/// by Microsoft Entra during a Verified ID claim validation event.
/// </summary>
/// <remarks>
/// This context contains both the core claims extracted from the user's
/// Verified ID credential and any additional information supplied by the
/// issuing authority or validation process.
///
/// These values may be used by custom extension handlers to enforce
/// business rules, validate identity attributes, or correlate the verified
/// credential with internal user records.
/// </remarks>
public sealed class VerifiedIdClaimsContext
{
    /// <summary>
    /// Gets or sets additional metadata associated with the verified ID
    /// credential, such as internal identifiers or issuer‑specific fields.
    /// </summary>
    [JsonPropertyName("additionalInfo")]
    public VerifiedIdAdditionalInfo? AdditionalInfo { get; init; }

    /// <summary>
    /// Gets or sets the set of verified claims extracted from the user's
    /// credential, such as name or date of birth.
    /// </summary>
    [JsonPropertyName("claims")]
    public VerifiedIdClaims? Claims { get; init; }
}
