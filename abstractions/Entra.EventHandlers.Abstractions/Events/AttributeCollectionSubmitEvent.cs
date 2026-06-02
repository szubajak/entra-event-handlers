using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Abstractions.Protocol.SignUp;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Events;

/// <summary>
/// Represents the incoming AttributeCollectionSubmit event sent by Microsoft Entra.
/// This event is triggered after the user submits attribute values during the
/// attribute collection flow.
/// </summary>
/// <remarks>
/// Handlers use this event to validate submitted attributes, modify values,
/// block the flow, or return validation errors.
///
/// For the official event schema and processing guidance, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-extension-onattributecollectionsubmit-retrieve-return-data
public class AttributeCollectionSubmitEvent : EntraEvent<AttributeCollectionSubmitEventPayload>
{
    public override string Type => EntraEventTypes.AttributeCollectionSubmit;
}

/// <summary>
/// Payload for the AttributeCollectionSubmit event.
/// Contains the user-submitted attribute values and related identity
/// information that a custom extension may validate or transform.
///
/// This model mirrors the JSON structure expected by Microsoft Entra.
/// </summary>
/// <remarks>
/// The <c>userSignUpInfo</c> section includes the attributes submitted by the
/// user, along with identity information that may be used to validate or
/// enrich the data.
///
/// For detailed payload structure and supported actions, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-extension-onattributecollectionsubmit-retrieve-return-data
/// </remarks>
public class AttributeCollectionSubmitEventPayload : EntraEventPayload
{
    public override string OdataType { get; } = EntraOdataTypes.AttributeCollectionSubmit.CalloutData;

    /// <summary>
    /// Contains the attribute values and identity information submitted by the user
    /// during the attribute collection flow. This data is used for validation,
    /// transformation, or enforcement of custom logic.
    /// </summary>
    /// <remarks>
    /// The <c>userSignUpInfo</c> object is required for the AttributeCollectionSubmit
    /// event and represents the final values provided by the user.
    ///
    /// For details, see:
    /// https://learn.microsoft.com/en-us/entra/identity-platform/custom-extension-onattributecollectionsubmit-retrieve-return-data
    /// </remarks>
    [JsonPropertyName("userSignUpInfo")]
    public required UserSignUpInfo UserSignUpInfo { get; init; }
}