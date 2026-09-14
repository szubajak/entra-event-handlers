using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Abstractions.Results;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public class TestPasswordSubmitHandlerThrowsUnexpected : IPasswordSubmitHandler
{
    public Task<EntraHandlerResult<PasswordSubmitResponse>> HandleAsync(
        PasswordSubmitEvent request,
        CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("Unexpected error!");
    }
}
