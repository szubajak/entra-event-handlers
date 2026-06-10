using Entra.EventHandlers.Abstractions.Protocol;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Responses;

/// <summary>
/// Represents the response returned to Microsoft Entra for an
/// AttributeCollectionStart event.
///
/// Handlers use this type to construct a valid response according to the
/// Entra custom extension contract.
/// </summary>
/// <remarks>
/// For the official response schema, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-extension-onattributecollectionstart-retrieve-return-data
/// </remarks>
public sealed class AttributeCollectionStartResponse : EntraEventResponse<AttributeCollectionStartResponsePayload>
{
}

/// <summary>
/// Payload for the response to an AttributeCollectionStart event.
/// Specifies the action(s) that Entra should perform next.
///
/// This model mirrors the JSON structure expected by Microsoft Entra.
/// </summary>
/// <remarks>
/// For detailed response structure and action definitions, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-extension-onattributecollectionstart-retrieve-return-data
/// </remarks>
public sealed class AttributeCollectionStartResponsePayload : EntraEventResponsePayload
{
    [JsonPropertyName("@odata.type")]
    public override string OdataType { get; } = EntraOdataTypes.AttributeCollectionStart.ResponseData;
}