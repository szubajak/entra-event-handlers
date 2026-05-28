namespace Entra.EventHandlers.Abstractions.Protocol;

/// <summary>
/// Provides the OData type identifiers for Microsoft Entra authentication
/// event types. These values are used to identify the incoming event in
/// custom extension handlers.
/// </summary>
public static class EntraEventTypes
{
    /// <summary>
    /// The event type for the AttributeCollectionStart event.
    /// </summary>
    public const string AttributeCollectionStart =
        "microsoft.graph.authenticationEvent.attributeCollectionStart";

    /// <summary>
    /// The event type for the AttributeCollectionSubmit event.
    /// </summary>
    public const string AttributeCollectionSubmit = 
        "microsoft.graph.authenticationEvent.attributeCollectionSubmit";

    /// <summary>
    /// The event type for the TokenIssuanceStart event.
    /// </summary>
    public const string TokenIssuanceStart = 
        "microsoft.graph.authenticationEvent.tokenIssuanceStart";
}
