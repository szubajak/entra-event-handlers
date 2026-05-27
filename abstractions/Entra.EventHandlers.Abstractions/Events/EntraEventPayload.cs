using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Protocol.Authentication;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Events;

public abstract class EntraEventPayload : IHaveOdataType
{
    [JsonPropertyName("@odata.type")]
    public string? RawOdataType { get; set; }

    [JsonIgnore]
    public abstract string OdataType { get; }

    [JsonPropertyName("tenantId")]
    public Guid TenantId { get; set; }

    [JsonPropertyName("authenticationEventListenerId")]
    public Guid AuthenticationEventListenerId { get; set; }

    [JsonPropertyName("customAuthenticationExtensionId")]
    public Guid CustomAuthenticationExtensionId { get; set; }

    [JsonPropertyName("authenticationContext")]
    public required AuthenticationContext AuthenticationContext { get; set; }

    public void ValidateOdataType()
    {
        if (!string.Equals(RawOdataType, OdataType, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Invalid @odata.type. Expected '{OdataType}', got '{RawOdataType}'.");
        }
    }
}