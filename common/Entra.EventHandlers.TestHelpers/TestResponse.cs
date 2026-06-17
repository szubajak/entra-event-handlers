using Entra.EventHandlers.Abstractions.Responses;

namespace Entra.EventHandlers.TestHelpers;

public sealed class TestResponse : EntraEventResponse
{
    public string? TestProperty { get; init; }
}