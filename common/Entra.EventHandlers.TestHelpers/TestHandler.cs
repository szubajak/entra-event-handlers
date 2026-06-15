using Entra.EventHandlers.Abstractions.Interfaces;

namespace Entra.EventHandlers.TestHelpers;

public sealed class TestHandler : IEntraEventHandler<TestEvent, TestResponse>
{
    public async Task<TestResponse> Handle(TestEvent request, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(new TestResponse());
    }
}