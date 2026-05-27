using Entra.EventHandlers.Abstractions.Protocol;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Responses;

public class TokenIssuanceStartResponse : EntraEventResponse<TokenIssuanceStartResponsePayload>
{
}

public class TokenIssuanceStartResponsePayload : EntraEventResponsePayload
{
    [JsonPropertyName("@odata.type")]
    public override string OdataType { get; } = EntraOdataTypes.TokenIssuanceStart.ResponseData;
}