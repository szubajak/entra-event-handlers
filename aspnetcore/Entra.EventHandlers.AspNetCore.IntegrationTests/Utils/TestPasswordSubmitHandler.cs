using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public class TestPasswordSubmitHandler : TestHandlerBase, IPasswordSubmitHandler
{
    public Task<PasswordSubmitResponse> Handle(
        PasswordSubmitEvent request,
        CancellationToken cancellationToken = default)
    {
        CapturedCancellationToken = cancellationToken;

        return Task.FromResult(
            EntraEventResponses
                .PasswordSubmit()
                .WithNonce("test-nonce")
                .MigratePassword()
                .Build());
    }
}
