using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Protocol.VerifiedId;

/// <summary>
/// Represents the core identity claims extracted from a user's Verified ID
/// credential during a claim validation event.
/// </summary>
/// <remarks>
/// These claims reflect the verified attributes issued by the credential
/// provider and may be used to validate user identity, enforce policy, or
/// correlate with existing directory records.
/// </remarks>
public sealed class VerifiedIdClaims
{
    /// <summary>
    /// Gets or sets the verified first name of the user, as provided by the
    /// credential issuer.
    /// </summary>
    [JsonPropertyName("firstName")]
    public string? FirstName { get; init; }

    /// <summary>
    /// Gets or sets the verified last name of the user.
    /// </summary>
    [JsonPropertyName("lastName")]
    public string? LastName { get; init; }

    /// <summary>
    /// Gets or sets the verified date of birth of the user, typically in
    /// <c>YYYY-MM-DD</c> format.
    /// </summary>
    [JsonPropertyName("dateOfBirth")]
    public string? DateOfBirth { get; init; }
}
