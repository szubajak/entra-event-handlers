using Entra.EventHandlers.Abstractions.Protocol;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Actions;

public class ShowValidationErrorAction : EntraAction
{
    [JsonPropertyName("@odata.type")]
    public override string OdataType { get; } = EntraOdataTypes.AttributeCollectionSubmit.ShowValidationError;

    [JsonPropertyName("message")]
    public required string Message { get; set; }

    [JsonPropertyName("attributeErrors")]
    public Dictionary<string, string> AttributeErrors { get; set; } = [];
}