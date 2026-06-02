namespace Entra.EventHandlers.UnitTests.Logging;

public sealed class TestScope(object state) : IDisposable
{
    public object State { get; } = state;
    public void Dispose() { }
}
