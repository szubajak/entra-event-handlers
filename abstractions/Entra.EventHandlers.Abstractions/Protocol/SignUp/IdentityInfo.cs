using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Protocol.SignUp;

public class IdentityInfo
{
    [JsonPropertyName("signInType")]
    public string? SignInType { get; set; }

    [JsonPropertyName("issuer")]
    public string? Issuer { get; set; }

    [JsonPropertyName("issuerAssignedId")]
    public string? IssuerAssignedId { get; set; }
}
