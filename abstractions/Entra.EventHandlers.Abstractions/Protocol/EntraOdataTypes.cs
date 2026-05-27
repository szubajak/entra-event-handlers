namespace Entra.EventHandlers.Abstractions.Protocol;

public static class EntraOdataTypes
{
    public static class AttributeCollectionStart
    {
        public const string CalloutData =
            "microsoft.graph.onAttributeCollectionStartCalloutData";

        public const string ResponseData =
            "microsoft.graph.onAttributeCollectionStartResponseData";

        public const string ContinueWithDefaultBehavior =
            "microsoft.graph.attributeCollectionStart.continueWithDefaultBehavior";

        public const string ShowBlockPage =
            "microsoft.graph.attributeCollectionStart.showBlockPage";

        public const string SetPrefillValues =
            "microsoft.graph.attributeCollectionStart.setPrefillValues";
    }

    public static class AttributeCollectionSubmit
    {
        public const string CalloutData =
            "microsoft.graph.onAttributeCollectionSubmitCalloutData";

        public const string ResponseData =
            "microsoft.graph.onAttributeCollectionSubmitResponseData";

        public const string ContinueWithDefaultBehavior =
            "microsoft.graph.attributeCollectionSubmit.continueWithDefaultBehavior";

        public const string ShowBlockPage =
            "microsoft.graph.attributeCollectionSubmit.showBlockPage";

        public const string ModifyAttributeValues =
            "microsoft.graph.attributeCollectionSubmit.modifyAttributeValues";

        public const string ShowValidationError =
            "microsoft.graph.attributeCollectionSubmit.showValidationError";
    }

    public static class TokenIssuanceStart
    {
        public const string CalloutData =
            "microsoft.graph.onTokenIssuanceStartCalloutData";

        public const string ResponseData =
            "microsoft.graph.onTokenIssuanceStartResponseData";

        public const string ProvideClaimsForToken =
            "microsoft.graph.tokenIssuanceStart.provideClaimsForToken";
    }

    public static class DirectoryAttributes
    {
        public const string String =
            "microsoft.graph.stringDirectoryAttributeValue";

        public const string Int64 =
            "microsoft.graph.int64DirectoryAttributeValue";

        public const string Boolean =
            "microsoft.graph.booleanDirectoryAttributeValue";
    }
}
