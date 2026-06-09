using Entra.EventHandlers.Abstractions.Protocol;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Responses;

/// <summary>
/// Represents the response returned to Microsoft Entra for a
/// PasswordSubmit event.
///
/// Handlers use this type to construct a valid response according to the
/// Entra custom extension contract.
/// </summary>
/// <remarks>
/// For the official response schema and guidance, see:
/// https://learn.microsoft.com/en-us/entra/external-id/customers/how-to-migrate-passwords-just-in-time
/// </remarks>
public class PasswordSubmitResponse : EntraEventResponse<PasswordSubmitResponsePayload>
{
}

/// <summary>
/// Payload for the response to a PasswordSubmit event. Specifies the
/// actions that Microsoft Entra should take after evaluating the submitted
/// password, such as migrating, updating, retrying, or blocking the
/// password submission.
///
/// This model mirrors the JSON structure expected by Microsoft Entra.
/// </summary>
/// <remarks>
/// The <c>nonce</c> value is required. Microsoft Entra sends the nonce as
/// part of the <c>encryptedPasswordContext</c> in the request payload and
/// expects the same value to be returned in the response for verification
/// purposes.
///
/// For detailed response structure and supported actions, see:
/// https://learn.microsoft.com/en-us/entra/external-id/customers/how-to-migrate-passwords-just-in-time
/// </remarks>
public class PasswordSubmitResponsePayload : EntraEventResponsePayload
{
    [JsonPropertyName("@odata.type")]
    public override string OdataType { get; } = EntraOdataTypes.PasswordSubmit.ResponseData;

    /// <summary>
    /// A nonce value that must be returned to Microsoft Entra. This value is
    /// provided in the request payload and is used by Entra to verify the
    /// integrity of the password migration flow.
    /// </summary>
    [JsonPropertyName("nonce")]
    public required string Nonce { get; init; }
}