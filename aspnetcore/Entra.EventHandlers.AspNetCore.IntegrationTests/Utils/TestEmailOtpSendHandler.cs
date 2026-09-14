using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Abstractions.Results;
using Entra.EventHandlers.Builders;
using Entra.EventHandlers.TestHelpers;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public class TestEmailOtpSendHandler : TestHandlerBase, IEmailOtpSendHandler
{
    public Task<EntraHandlerResult<EmailOtpSendResponse>> HandleAsync(
        EmailOtpSendEvent request,
        CancellationToken cancellationToken = default)
    {
        request.Validate();

        WasCalled = true;
        CapturedCancellationToken = cancellationToken;

        var response = EntraEventResponses.EmailOtpSend()
           .ContinueWithDefaultBehavior()
           .Build();

        return Task.FromResult(new EntraHandlerResult<EmailOtpSendResponse>(response));
    }
}
