namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public abstract class TestHandlerBase
{
    public CancellationToken CapturedCancellationToken { get; protected set; }
}
