using Entra.EventHandlers.Abstractions.Protocol;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Actions;

public class ProvideClaimsForTokenAction : EntraAction
{
    [JsonPropertyName("@odata.type")]
    public override string OdataType { get; } = EntraOdataTypes.TokenIssuanceStart.ProvideClaimsForToken;

    [JsonPropertyName("claims")]
    public Dictionary<string, object> Claims { get; set; } = [];
}