using Entra.EventHandlers.Abstractions.Actions.Types;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Actions;

public class ContinueAction(ContinueActionType type) : EntraAction
{
    [JsonPropertyName("@odata.type")]
    public override string OdataType { get; } = type.Value;
}

