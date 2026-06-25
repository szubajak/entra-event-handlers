namespace Entra.EventHandlers.TestHelpers;

public abstract class TestHandlerBase
{
    public bool WasCalled { get; protected set; }

    public CancellationToken CapturedCancellationToken { get; protected set; }
}
