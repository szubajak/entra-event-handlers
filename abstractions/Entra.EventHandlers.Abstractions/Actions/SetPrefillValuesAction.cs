using Entra.EventHandlers.Abstractions.Protocol;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Actions;

public class SetPrefillValuesAction : EntraAction
{
    [JsonPropertyName("@odata.type")]
    public override string OdataType { get; } = EntraOdataTypes.AttributeCollectionStart.SetPrefillValues;

    [JsonPropertyName("inputs")]
    public Dictionary<string, object> Inputs { get; set; } = [];
}