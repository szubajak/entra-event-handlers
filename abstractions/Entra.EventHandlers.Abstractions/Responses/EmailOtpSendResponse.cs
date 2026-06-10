using Entra.EventHandlers.Abstractions.Protocol;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Responses;

/// <summary>
/// Represents the response returned to Microsoft Entra for an
/// EmailOtpSend event.
///
/// Handlers use this type to construct a valid response according to the
/// Entra custom extension contract.
/// </summary>
/// <remarks>
/// For the official response schema, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-extension-email-otp-send-data
/// </remarks>
public sealed class EmailOtpSendResponse : EntraEventResponse<EmailOtpSendResponsePayload>
{
}

/// <summary>
/// Payload for the response to an EmailOtpSend event.
/// Specifies the action that Microsoft Entra should perform next.
///
/// This model mirrors the JSON structure expected by Microsoft Entra.
/// </summary>
/// <remarks>
/// For detailed response structure and action definitions, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-extension-email-otp-send-data
/// </remarks>
public sealed class EmailOtpSendResponsePayload : EntraEventResponsePayload
{
    [JsonPropertyName("@odata.type")]
    public override string OdataType { get; } = EntraOdataTypes.EmailOtpSend.ResponseData;
}