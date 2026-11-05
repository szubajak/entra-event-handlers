namespace Entra.EventHandlers.TestHelpers;

public sealed class HandlerCoreTest
{
    public int HandleCoreCallCount { get; set; }

    public CancellationToken? CapturedCancellationToken { get; set; }

    public Exception? ExceptionToThrow { get; set; }

    public void Record(CancellationToken cancellationToken = default)
    {
        HandleCoreCallCount++;
        CapturedCancellationToken = cancellationToken;

        if (ExceptionToThrow is not null)
        {
            throw ExceptionToThrow;
        }
    }
}
