using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Interfaces;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Responses;

public abstract class EntraEventResponsePayload : IHaveOdataType
{
    [JsonIgnore]
    public abstract string OdataType { get; }

    [JsonPropertyName("actions")]
    public IEnumerable<EntraAction> Actions { get; set; } = [];
}