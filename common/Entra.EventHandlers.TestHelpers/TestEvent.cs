using Entra.EventHandlers.Abstractions.Events;

namespace Entra.EventHandlers.TestHelpers;

public sealed class TestEvent : EntraEvent
{
    public override string Type => "TestEvent";

    public override Guid CorrelationId => Guid.Parse("00000000-0000-0000-0000-000000000000");
}