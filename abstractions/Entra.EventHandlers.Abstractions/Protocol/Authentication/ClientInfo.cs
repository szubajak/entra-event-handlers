using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Protocol.Authentication;

public class ClientInfo
{
    [JsonPropertyName("ip")]
    public string? Ip { get; set; }

    [JsonPropertyName("locale")]
    public string? Locale { get; set; }

    [JsonPropertyName("market")]
    public string? Market { get; set; }
}
