namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils.Abstractions;

public abstract class TestHandlerBase
{
    public bool WasCalled { get; protected set; }

    public CancellationToken CapturedCancellationToken { get; protected set; }
}
