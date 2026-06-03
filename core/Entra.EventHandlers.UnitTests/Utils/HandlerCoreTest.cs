namespace Entra.EventHandlers.UnitTests.Utils;

public sealed class HandlerCoreTest
{
    public int HandleCoreCallCount { get; set; }

    public CancellationToken? PassedCancellationToken { get; set; }

    public bool ShouldThrow { get; set; } = false;

    public void Record(CancellationToken cancellationToken)
    {
        HandleCoreCallCount++;
        PassedCancellationToken = cancellationToken;

        if (ShouldThrow) throw new Exception("Test exception!");
    }
}
