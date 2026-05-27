using Entra.EventHandlers.Abstractions.Protocol;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Responses;

public class AttributeCollectionSubmitResponse : EntraEventResponse<AttributeCollectionSubmitResponsePayload>
{
}

public class AttributeCollectionSubmitResponsePayload : EntraEventResponsePayload
{
    [JsonPropertyName("@odata.type")]
    public override string OdataType { get; } = EntraOdataTypes.AttributeCollectionSubmit.ResponseData;
}