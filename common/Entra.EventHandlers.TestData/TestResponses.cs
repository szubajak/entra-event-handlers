using Entra.EventHandlers.Abstractions.Responses;

namespace Entra.EventHandlers.TestData;

public static class TestResponses
{
    public static AttributeCollectionStartResponse CreateAttributeCollectionStartResponse() =>
        new()
        {
            Data = new AttributeCollectionStartResponsePayload()
        };

    public static AttributeCollectionSubmitResponse CreateAttributeCollectionSubmitResponse() =>
    new()
    {
        Data = new AttributeCollectionSubmitResponsePayload()
    };
}