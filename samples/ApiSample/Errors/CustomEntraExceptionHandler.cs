using Entra.EventHandlers.AspNetCore.Interfaces;

namespace ApiSample.Errors;

public sealed class CustomEntraExceptionHandler : IEntraExceptionHandler
{
    public Task HandleAsync(Exception ex)
    {
        return Task.CompletedTask;
    }
}