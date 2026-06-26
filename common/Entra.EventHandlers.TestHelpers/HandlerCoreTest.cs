namespace Entra.EventHandlers.TestHelpers;

public sealed class HandlerCoreTest
{
    public int HandleCoreCallCount { get; set; }

    public CancellationToken? CapturedCancellationToken { get; set; }

    public bool ShouldThrow { get; set; } = false;

    public void Record(CancellationToken cancellationToken)
    {
        HandleCoreCallCount++;
        CapturedCancellationToken = cancellationToken;

        if (ShouldThrow) throw new Exception("Test exception!");
    }
}
