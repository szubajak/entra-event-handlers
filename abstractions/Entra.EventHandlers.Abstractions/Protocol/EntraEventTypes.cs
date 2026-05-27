namespace Entra.EventHandlers.Abstractions.Protocol;

public static class EntraEventTypes
{
    public const string AttributeCollectionStart =
        "microsoft.graph.authenticationEvent.attributeCollectionStart";

    public const string AttributeCollectionSubmit = 
        "microsoft.graph.authenticationEvent.attributeCollectionSubmit";

    public const string TokenIssuanceStart = 
        "microsoft.graph.authenticationEvent.tokenIssuanceStart";
}
