using Entra.EventHandlers.Abstractions.Interfaces;

namespace Entra.EventHandlers.TestHelpers;

public class TestHandler : IEntraEventHandler<TestEvent, TestResponse>
{
    public TestEvent? ReceivedEvent { get; private set; }

    public CancellationToken? CapturedCancellationToken { get; private set; }

    public Task<TestResponse> Handle(TestEvent request, CancellationToken cancellationToken)
    {
        ReceivedEvent = request;
        CapturedCancellationToken = cancellationToken;
        return Task.FromResult(new TestResponse());
    }
}
