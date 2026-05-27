namespace Entra.EventHandlers.ResponseBuilders;

public class EntraEventResponses
{
    public static AttributeCollectionStartResponseBuilder AttributeCollectionStart() => new();

    public static AttributeCollectionSubmitResponseBuilder AttributeCollectionSubmit() => new();

    public static TokenIssuanceStartResponseBuilder TokenIssuanceStart() => new();
}
