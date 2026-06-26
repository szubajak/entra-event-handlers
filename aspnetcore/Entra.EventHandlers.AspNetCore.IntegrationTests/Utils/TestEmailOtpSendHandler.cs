using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders;
using Entra.EventHandlers.TestHelpers;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public class TestEmailOtpSendHandler : TestHandlerBase, IEmailOtpSendHandler
{
    public Task<EmailOtpSendResponse> HandleAsync(
        EmailOtpSendEvent request,
        CancellationToken cancellationToken = default)
    {
        WasCalled = true;
        CapturedCancellationToken = cancellationToken;

        return Task.FromResult(
            EntraEventResponses
                .EmailOtpSend()
                .ContinueWithDefaultBehavior()
                .Build());
    }
}
