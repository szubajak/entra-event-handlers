using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Protocol.Authentication;

/// <summary>
/// Represents information about a Microsoft Entra service principal associated
/// with the authentication request. Includes identifiers and display metadata
/// for both client and resource applications.
/// </summary>
/// <remarks>
/// This data is provided by Microsoft Entra as part of the authentication
/// context and may be used for diagnostics, authorization decisions, or
/// conditional logic in custom extension handlers.
/// </remarks>
public sealed class ServicePrincipalInfo
{
    /// <summary>
    /// Gets or sets the object ID of the service principal in the tenant.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid? Id { get; init; }

    /// <summary>
    /// Gets or sets the application (client) ID associated with the service
    /// principal.
    /// </summary>
    [JsonPropertyName("appId")]
    public Guid? AppId { get; init; }

    /// <summary>
    /// Gets or sets the display name of the application as registered in
    /// Microsoft Entra.
    /// </summary>
    [JsonPropertyName("appDisplayName")]
    public string? AppDisplayName { get; init; }

    /// <summary>
    /// Gets or sets the display name of the service principal instance.
    /// </summary>
    [JsonPropertyName("displayName")]
    public string? DisplayName { get; init; }
}
