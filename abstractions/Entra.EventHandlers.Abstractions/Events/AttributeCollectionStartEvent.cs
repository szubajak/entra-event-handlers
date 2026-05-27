using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Abstractions.Protocol.SignUp;
using Entra.EventHandlers.Abstractions.Responses;
using Mediator;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Events;

/// <summary>
/// Represents the AttributeCollectionStart event sent by Microsoft Entra
/// to a custom extension endpoint during an authentication flow.
///
/// This type provides a strongly typed model for the incoming JSON payload.
/// It mirrors the public contract defined by Microsoft and contains no
/// additional logic.
/// </summary>
/// <remarks>
/// For the official Microsoft schema and field descriptions, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-extension-onattributecollectionstart-retrieve-return-data
/// </remarks>
public class AttributeCollectionStartEvent : EntraEvent<AttributeCollectionStartEventPayload>, IRequest<AttributeCollectionStartResponse>
{
    public override string Type => EntraEventTypes.AttributeCollectionStart;
}

/// <summary>
/// Payload for the AttributeCollectionStart event. Contains data provided
/// by Microsoft Entra when initiating the attribute collection step of a
/// custom authentication flow.
///
/// This model reflects the structure of the JSON payload sent by Entra.
/// </summary>
/// <remarks>
/// Field meanings and behavior are defined by Microsoft.  
/// See the official documentation for details:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-extension-onattributecollectionstart-retrieve-return-data
/// </remarks>
public class AttributeCollectionStartEventPayload : EntraEventPayload
{
    public override string OdataType { get; } = EntraOdataTypes.AttributeCollectionStart.CalloutData;

    [JsonPropertyName("userSignUpInfo")]
    public UserSignUpInfo? UserSignUpInfo { get; set; }
}