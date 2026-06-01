namespace Entra.EventHandlers.Abstractions.Protocol;

/// <summary>
/// Provides the OData type identifiers used by Microsoft Entra for
/// authentication event callouts, responses, actions, and directory
/// attribute values.
/// </summary>
public static class EntraOdataTypes
{
    /// <summary>
    /// OData type identifiers for the AttributeCollectionStart event.
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
        /// The OData type for the continue-with-default-behavior action.
        /// </summary>
        public const string ContinueWithDefaultBehavior =
            "microsoft.graph.attributeCollectionStart.continueWithDefaultBehavior";

        /// <summary>
        /// The OData type for the show-block-page action.
        /// </summary>
        public const string ShowBlockPage =
            "microsoft.graph.attributeCollectionStart.showBlockPage";

        /// <summary>
        /// The OData type for the set-prefill-values action.
        /// </summary>
        public const string SetPrefillValues =
            "microsoft.graph.attributeCollectionStart.setPrefillValues";
    }

    /// <summary>
    /// OData type identifiers for the AttributeCollectionSubmit event.
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
        /// The OData type for the continue-with-default-behavior action.
        /// </summary>
        public const string ContinueWithDefaultBehavior =
            "microsoft.graph.attributeCollectionSubmit.continueWithDefaultBehavior";

        /// <summary>
        /// The OData type for the show-block-page action.
        /// </summary>
        public const string ShowBlockPage =
            "microsoft.graph.attributeCollectionSubmit.showBlockPage";

        /// <summary>
        /// The OData type for the modify-attribute-values action.
        /// </summary>
        public const string ModifyAttributeValues =
            "microsoft.graph.attributeCollectionSubmit.modifyAttributeValues";

        /// <summary>
        /// The OData type for the show-validation-error action.
        /// </summary>
        public const string ShowValidationError =
            "microsoft.graph.attributeCollectionSubmit.showValidationError";
    }

    /// <summary>
    /// OData type identifiers for the TokenIssuanceStart event.
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
        /// The OData type for the provide-claims-for-token action.
        /// </summary>
        public const string ProvideClaimsForToken =
            "microsoft.graph.tokenIssuanceStart.provideClaimsForToken";
    }

    /// <summary>
    /// OData type identifiers for the EmailOtpSend event.
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
        /// The OData type for the continue-with-default-behavior action.
        /// </summary>
        public const string ContinueWithDefaultBehavior =
            "microsoft.graph.OtpSend.continueWithDefaultBehavior";
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
