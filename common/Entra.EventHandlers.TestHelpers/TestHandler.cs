using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Results;

namespace Entra.EventHandlers.TestHelpers;

public class TestHandler : TestHandlerBase, IEntraEventHandler<TestEvent, TestResponse>
{
    public Task<EntraHandlerResult<TestResponse>> HandleAsync(TestEvent evt, CancellationToken cancellationToken = default)
    {
        WasCalled = true;
        CapturedCancellationToken = cancellationToken;

        return Task.FromResult(new EntraHandlerResult<TestResponse>(new TestResponse()));
    }
}
