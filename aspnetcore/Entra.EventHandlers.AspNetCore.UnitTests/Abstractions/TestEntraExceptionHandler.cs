using Entra.EventHandlers.AspNetCore.Interfaces;

namespace Entra.EventHandlers.AspNetCore.UnitTests.Abstractions;

internal sealed class TestEntraExceptionHandler : IEntraExceptionHandler
{
    public bool WasCalled { get; private set; }

    public Task HandleAsync(Exception ex)
    {
        WasCalled = true;
        return Task.CompletedTask;
    }
}
