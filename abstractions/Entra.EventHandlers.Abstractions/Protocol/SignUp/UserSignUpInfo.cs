using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Protocol.SignUp;

public class UserSignUpInfo
{
    [JsonPropertyName("attributes")]
    public Dictionary<string, DirectoryAttributeValue>? Attributes { get; set; }

    [JsonPropertyName("identities")]
    public IEnumerable<IdentityInfo>? Identities { get; set; }
}
