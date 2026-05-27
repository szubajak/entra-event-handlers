using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Protocol.Authentication;

public class AuthenticationContext
{
    [JsonPropertyName("correlationId")]
    public Guid CorrelationId { get; set; }

    [JsonPropertyName("client")]
    public ClientInfo? Client { get; set; }

    [JsonPropertyName("protocol")]
    public string? Protocol { get; set; }

    [JsonPropertyName("clientServicePrincipal")]
    public ServicePrincipalInfo? ClientServicePrincipal { get; set; }

    [JsonPropertyName("resourceServicePrincipal")]
    public ServicePrincipalInfo? ResourceServicePrincipal { get; set; }

    [JsonPropertyName("user")]
    public UserInfo? User { get; set; }
}
