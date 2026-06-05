namespace Entra.EventHandlers.TestUtils;

public sealed class TestScope(object state) : IDisposable
{
    public object State { get; } = state;
    public void Dispose() { }
}
