using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public class TestPasswordSubmitHandlerThrowsUnexpected : IPasswordSubmitHandler
{
    public Task<PasswordSubmitResponse> Handle(
        PasswordSubmitEvent request,
        CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("Unexpected error!");
    }
}
