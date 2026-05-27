using Entra.EventHandlers.Abstractions.Protocol;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Actions;

public class ModifyAttributeValuesAction : EntraAction
{
    [JsonPropertyName("@odata.type")]
    public override string OdataType { get; } = EntraOdataTypes.AttributeCollectionSubmit.ModifyAttributeValues;

    [JsonPropertyName("attributes")]
    public Dictionary<string, object> Attributes { get; set; } = [];
}