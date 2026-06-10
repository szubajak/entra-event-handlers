using Entra.EventHandlers.Abstractions.Events;

namespace Entra.EventHandlers.Abstractions.Protocol;

/// <summary>
/// Provides the OData <c>@odata.type</c> identifiers used by Microsoft Entra
/// for authentication event callouts, responses, and action types. These
/// constants act as type discriminators during polymorphic serialization
/// and deserialization of event payloads.
/// </summary>
/// <remarks>
/// All values must match the Microsoft Graph contract exactly. They are used
/// internally by the SDK to bind incoming event requests to the correct
/// strongly typed models.
/// </remarks>
public static class EntraOdataTypes
{
    /// <summary>
    /// OData type identifiers for the
    /// <see cref="AttributeCollectionStartEvent"/> request and its related
    /// response and action types.
    /// </summary>
    public static class AttributeCollectionStart
    {
        /// <summary>
        /// The OData type for the callout data payload.
        /// </summary>
        public const string CalloutData =
            "microsoft.graph.onAttributeCollectionStartCalloutData";

        /// <summary>
        /// The OData type for the response data payload.
        /// </summary>
        public const string ResponseData =
            "microsoft.graph.onAttributeCollectionStartResponseData";

        /// <summary>
        /// The action that instructs Entra to continue with its default behavior.
        /// </summary>
        public const string ContinueWithDefaultBehavior =
            "microsoft.graph.attributeCollectionStart.continueWithDefaultBehavior";

        /// <summary>
        /// The action that displays a block page to the user.
        /// </summary>
        public const string ShowBlockPage =
            "microsoft.graph.attributeCollectionStart.showBlockPage";

        /// <summary>
        /// The action that sets prefill values for the attribute collection form.
        /// </summary>
        public const string SetPrefillValues =
            "microsoft.graph.attributeCollectionStart.setPrefillValues";
    }

    /// <summary>
    /// OData type identifiers for the
    /// <see cref="AttributeCollectionSubmitEvent"/> request and its related
    /// response and action types.
    /// </summary>
    public static class AttributeCollectionSubmit
    {
        /// <summary>
        /// The OData type for the callout data payload.
        /// </summary>
        public const string CalloutData =
            "microsoft.graph.onAttributeCollectionSubmitCalloutData";

        /// <summary>
        /// The OData type for the response data payload.
        /// </summary>
        public const string ResponseData =
            "microsoft.graph.onAttributeCollectionSubmitResponseData";

        /// <summary>
        /// The action that instructs Entra to continue with its default behavior.
        /// </summary>
        public const string ContinueWithDefaultBehavior =
            "microsoft.graph.attributeCollectionSubmit.continueWithDefaultBehavior";

        /// <summary>
        /// The action that displays a block page to the user.
        /// </summary>
        public const string ShowBlockPage =
            "microsoft.graph.attributeCollectionSubmit.showBlockPage";

        /// <summary>
        /// The action that modifies submitted attribute values before validation.
        /// </summary>
        public const string ModifyAttributeValues =
            "microsoft.graph.attributeCollectionSubmit.modifyAttributeValues";

        /// <summary>
        /// The action that returns a validation error to the user.
        /// </summary>
        public const string ShowValidationError =
            "microsoft.graph.attributeCollectionSubmit.showValidationError";
    }

    /// <summary>
    /// OData type identifiers for the
    /// <see cref="TokenIssuanceStartEvent"/> request and its related
    /// response and action types.
    /// </summary>
    public static class TokenIssuanceStart
    {
        /// <summary>
        /// The OData type for the callout data payload.
        /// </summary>
        public const string CalloutData =
            "microsoft.graph.onTokenIssuanceStartCalloutData";

        /// <summary>
        /// The OData type for the response data payload.
        /// </summary>
        public const string ResponseData =
            "microsoft.graph.onTokenIssuanceStartResponseData";

        /// <summary>
        /// The action that provides additional claims to include in the issued token.
        /// </summary>
        public const string ProvideClaimsForToken =
            "microsoft.graph.tokenIssuanceStart.provideClaimsForToken";
    }

    /// <summary>
    /// OData type identifiers for the
    /// <see cref="EmailOtpSendEvent"/> request and its related response
    /// and action types.
    /// </summary>
    public static class EmailOtpSend
    {
        /// <summary>
        /// The OData type for the callout data payload.
        /// </summary>
        public const string CalloutData =
            "microsoft.graph.onOtpSendCalloutData";

        /// <summary>
        /// The OData type for the response data payload.
        /// </summary>
        public const string ResponseData =
            "microsoft.graph.OnOtpSendResponseData";

        /// <summary>
        /// The action that instructs Entra to continue with its default behavior.
        /// </summary>
        public const string ContinueWithDefaultBehavior =
            "microsoft.graph.OtpSend.continueWithDefaultBehavior";
    }

    /// <summary>
    /// OData type identifiers for the
    /// <see cref="PasswordSubmitEvent"/> request and its related response
    /// and action types.
    /// </summary>
    public static class PasswordSubmit
    {
        /// <summary>
        /// The OData type for the callout data payload.
        /// </summary>
        public const string CalloutData =
            "microsoft.graph.onPasswordSubmitCalloutData";

        /// <summary>
        /// The OData type for the response data payload.
        /// </summary>
        public const string ResponseData =
            "microsoft.graph.onPasswordSubmitResponseData";

        /// <summary>
        /// The action that migrates the user’s password to a new system.
        /// </summary>
        public const string MigratePassword =
            "microsoft.graph.passwordSubmit.MigratePassword";

        /// <summary>
        /// The OData type for the action that indicates the submitted password is
        /// correct but weak, and instructs the user to reset their password.
        /// </summary>
        public const string UpdatePassword =
            "microsoft.graph.passwordSubmit.UpdatePassword";

        /// <summary>
        /// The action that instructs the user to retry password submission.
        /// </summary>
        public const string Retry =
            "microsoft.graph.passwordSubmit.Retry";

        /// <summary>
        /// The action that blocks the password submission attempt.
        /// </summary>
        public const string Block =
            "microsoft.graph.passwordSubmit.Block";
    }

    /// <summary>
    /// OData type identifiers for directory attribute value types.
    /// </summary>
    public static class DirectoryAttributes
    {
        /// <summary>
        /// The OData type for a string directory attribute value.
        /// </summary>
        public const string String =
            "microsoft.graph.stringDirectoryAttributeValue";

        /// <summary>
        /// The OData type for an int64 directory attribute value.
        /// </summary>
        public const string Int64 =
            "microsoft.graph.int64DirectoryAttributeValue";

        /// <summary>
        /// The OData type for a boolean directory attribute value.
        /// </summary>
        public const string Boolean =
            "microsoft.graph.booleanDirectoryAttributeValue";
    }
}
