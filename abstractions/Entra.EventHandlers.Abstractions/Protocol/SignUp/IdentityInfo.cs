using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Protocol.SignUp;

/// <summary>
/// Represents an identity associated with the user during the sign-up or
/// attribute collection flow. Includes the sign-in method, issuing authority,
/// and the identifier assigned by the issuer.
/// </summary>
/// <remarks>
/// This data is provided by Microsoft Entra as part of the user sign-up
/// information and may be used to validate identity bindings, enforce
/// restrictions on allowed sign-in types, or correlate external identities.
/// </remarks>
public class IdentityInfo
{
    /// <summary>
    /// Gets or sets the type of sign-in method used by the identity
    /// (for example, <c>email</c>, <c>federated</c>, or <c>phoneNumber</c>).
    /// </summary>
    [JsonPropertyName("signInType")]
    public string? SignInType { get; init; }

    /// <summary>
    /// Gets or sets the issuer of the identity, such as the tenant domain
    /// (<c>contoso.onmicrosoft.com</c>) or an external identity provider.
    /// </summary>
    [JsonPropertyName("issuer")]
    public string? Issuer { get; init; }

    /// <summary>
    /// Gets or sets the identifier assigned by the issuer for this identity,
    /// such as an email address or external provider subject ID.
    /// </summary>
    [JsonPropertyName("issuerAssignedId")]
    public string? IssuerAssignedId { get; init; }
}
