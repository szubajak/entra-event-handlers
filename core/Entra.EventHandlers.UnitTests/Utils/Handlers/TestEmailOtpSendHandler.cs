using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Handlers.Base;
using Entra.EventHandlers.TestData;
using Entra.EventHandlers.TestHelpers;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.UnitTests.Utils.Handlers;

public class TestEmailOtpSendHandler(ILogger logger)
    : EmailOtpSendHandlerBase(logger)
{
    public HandlerCoreTest CoreTest { get; } = new HandlerCoreTest();

    public EmailOtpSendResponse ResponseToReturn { get; set; } = TestResponses.CreateEmailOtpSendResponse();

    protected override Task<EmailOtpSendResponse> HandleCoreAsync(
        EmailOtpSendEvent request,
        CancellationToken cancellationToken = default)
    {
        CoreTest.Record(cancellationToken);
        return Task.FromResult(ResponseToReturn);
    }
}
