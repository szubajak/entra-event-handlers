namespace Entra.EventHandlers.UnitTests.Utils;

public sealed class TestScope(object state) : IDisposable
{
    public object State { get; } = state;
    public void Dispose() { }
}
