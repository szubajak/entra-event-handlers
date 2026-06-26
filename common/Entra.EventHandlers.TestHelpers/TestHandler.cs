using Entra.EventHandlers.Abstractions.Interfaces;

namespace Entra.EventHandlers.TestHelpers;

public class TestHandler : TestHandlerBase, IEntraEventHandler<TestEvent, TestResponse>
{
    public Task<TestResponse> HandleAsync(TestEvent evt, CancellationToken cancellationToken)
    {
        WasCalled = true;
        CapturedCancellationToken = cancellationToken;
        return Task.FromResult(new TestResponse());
    }
}
