using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public class TestPasswordSubmitHandlerThrows : IPasswordSubmitHandler
{
    public Task<PasswordSubmitResponse> Handle(
        PasswordSubmitEvent request,
        CancellationToken cancellationToken = default)
    {
        throw new EntraValidationException("Invalid data!");
    }
}
