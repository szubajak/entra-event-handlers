using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Abstractions.Protocol.SignUp;
using Entra.EventHandlers.Abstractions.Responses;
using Mediator;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Events;

public class AttributeCollectionSubmitEvent : EntraEvent<AttributeCollectionSubmitEventPayload>, IRequest<AttributeCollectionSubmitResponse>
{
    public override string Type => EntraEventTypes.AttributeCollectionSubmit;
}

public class AttributeCollectionSubmitEventPayload : EntraEventPayload
{
    public override string OdataType { get; } = EntraOdataTypes.AttributeCollectionSubmit.CalloutData;

    [JsonPropertyName("userSignUpInfo")]
    public required UserSignUpInfo UserSignUpInfo { get; set; }
}