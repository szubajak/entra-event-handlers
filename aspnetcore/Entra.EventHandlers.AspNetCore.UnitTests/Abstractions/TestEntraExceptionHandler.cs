using Entra.EventHandlers.AspNetCore.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Entra.EventHandlers.AspNetCore.UnitTests.Abstractions;

internal sealed class TestEntraExceptionHandler : IEntraExceptionHandler
{
    public bool WasCalled
    {
        get; private set;
    }

    public Task HandleAsync(Exception ex, HttpContext context, bool isEntraException)
    {
        WasCalled = true;
        return Task.CompletedTask;
    }
}
