using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Responses;

public abstract class EntraEventResponse
{
}

public abstract class EntraEventResponse<TPayload> : EntraEventResponse
    where TPayload : EntraEventResponsePayload
{
    [JsonPropertyName("data")]
    public TPayload? Data { get; set; }
}