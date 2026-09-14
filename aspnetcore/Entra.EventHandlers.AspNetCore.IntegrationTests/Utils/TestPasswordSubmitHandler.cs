using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Abstractions.Results;
using Entra.EventHandlers.Builders;
using Entra.EventHandlers.TestHelpers;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public class TestPasswordSubmitHandler : TestHandlerBase, IPasswordSubmitHandler
{
    public Task<EntraHandlerResult<PasswordSubmitResponse>> HandleAsync(
        PasswordSubmitEvent request,
        CancellationToken cancellationToken = default)
    {
        request.Validate();

        WasCalled = true;
        CapturedCancellationToken = cancellationToken;

        var response = EntraEventResponses.PasswordSubmit()
            .WithNonce("test-nonce")
            .MigratePassword()
            .Build();

        return Task.FromResult(new EntraHandlerResult<PasswordSubmitResponse>(response));
    }
}
