using Entra.EventHandlers.Abstractions.Responses;

namespace Entra.EventHandlers.TestData;

public static class TestResponses
{
    public static AttributeCollectionStartResponse CreateAttributeCollectionStartResponse() =>
        new()
        {
            Data = new AttributeCollectionStartResponsePayload()
        };
}