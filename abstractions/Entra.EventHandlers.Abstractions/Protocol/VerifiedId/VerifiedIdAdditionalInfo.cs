using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Protocol.VerifiedId;

/// <summary>
/// Represents additional metadata associated with a Verified ID credential,
/// typically used to correlate the credential with internal systems.
/// </summary>
/// <remarks>
/// This information is issuer‑specific and may include identifiers or
/// attributes that support downstream business logic or user lookup.
/// </remarks>
public sealed class VerifiedIdAdditionalInfo
{
    /// <summary>
    /// Gets or sets the employee identifier associated with the verified
    /// credential, if provided by the issuer.
    /// </summary>
    [JsonPropertyName("employeeId")]
    public string? EmployeeId { get; init; }
}
