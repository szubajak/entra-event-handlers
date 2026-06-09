using Entra.EventHandlers.Abstractions.Events;

namespace Entra.EventHandlers.Abstractions.Protocol;

/// <summary>
/// Provides the OData <c>@odata.type</c> identifiers for Microsoft Entra
/// authentication event requests. These constants are used as type
/// discriminators when deserializing incoming event payloads.
/// </summary>
/// <remarks>
/// These values must match the Microsoft Graph contract exactly.
/// They are used during polymorphic deserialization of incoming
/// authentication event requests.
/// </remarks>

public static class EntraEventTypes
{
    /// <summary>
    /// The OData type for an <see cref="AttributeCollectionStartEvent"/> request.
    /// </summary>
    public const string AttributeCollectionStart =
        "microsoft.graph.authenticationEvent.attributeCollectionStart";

    /// <summary>
    /// The OData type for an <see cref="AttributeCollectionSubmitEvent"/> request.
    /// </summary>
    public const string AttributeCollectionSubmit =
        "microsoft.graph.authenticationEvent.attributeCollectionSubmit";

    /// <summary>
    /// The OData type for a <see cref="TokenIssuanceStartEvent"/> request.
    /// </summary>
    public const string TokenIssuanceStart =
        "microsoft.graph.authenticationEvent.tokenIssuanceStart";

    /// <summary>
    /// The OData type for an <see cref="EmailOtpSendEvent"/> request.
    /// </summary>
    public const string EmailOtpSend =
        "microsoft.graph.authenticationEvent.emailOtpSend";

    /// <summary>
    /// The OData type for a <see cref="PasswordSubmitEvent"/> request.
    /// </summary>
    public const string PasswordSubmit =
        "microsoft.graph.authenticationEvent.passwordSubmit";
}
