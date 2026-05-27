using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Protocol.Authentication;

public class ServicePrincipalInfo
{
    [JsonPropertyName("id")]
    public Guid? Id { get; set; }

    [JsonPropertyName("appId")]
    public Guid? AppId { get; set; }

    [JsonPropertyName("appDisplayName")]
    public string? AppDisplayName { get; set; }

    [JsonPropertyName("displayName")]
    public string? DisplayName { get; set; }
}
