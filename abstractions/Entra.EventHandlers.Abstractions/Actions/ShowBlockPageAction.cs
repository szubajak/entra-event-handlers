using Entra.EventHandlers.Abstractions.Actions.Types;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Actions;

public class ShowBlockPageAction(ShowBlockPageActionType type) : EntraAction
{
    [JsonPropertyName("@odata.type")]
    public override string OdataType { get; } = type.Value;

    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("message")]
    public required string Message { get; set; }
}
