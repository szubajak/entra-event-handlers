using Entra.EventHandlers.Abstractions.Protocol;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Events;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(AttributeCollectionStartEvent), EntraEventTypes.AttributeCollectionStart)]
[JsonDerivedType(typeof(AttributeCollectionSubmitEvent), EntraEventTypes.AttributeCollectionSubmit)]
[JsonDerivedType(typeof(TokenIssuanceStartEvent), EntraEventTypes.TokenIssuanceStart)]
public abstract class EntraEvent
{
    [JsonIgnore]
    public abstract string Type { get; }

    [JsonIgnore]
    public abstract Guid CorrelationId { get; }
}

public abstract class EntraEvent<TPayload> : EntraEvent
    where TPayload : EntraEventPayload
{
    [JsonPropertyName("source")]
    public required string Source { get; set; }

    [JsonPropertyName("data")]
    public required TPayload Data { get; set; }

    public override Guid CorrelationId => Data.AuthenticationContext.CorrelationId;

    public void Validate()
    {
        Data.ValidateOdataType();
    }
}