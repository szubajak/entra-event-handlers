using Entra.EventHandlers.Abstractions.Interfaces;

namespace Entra.EventHandlers.AzureFunctions.UnitTests.Utils;

public sealed class TestHandler : IEntraEventHandler<TestEvent, TestResponse>
{
    public async Task<TestResponse> Handle(TestEvent request, CancellationToken cancellationToken = default)
    {
        return new TestResponse();
    }
}