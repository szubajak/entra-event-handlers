using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Observability.Interfaces;
using Entra.EventHandlers.Observability.Models;

namespace Entra.EventHandlers.Observability.Mappers;

public interface IEmailOtpSendEventLogMapper : IEventLogMapper<EmailOtpSendEvent, EmailOtpSendResponse> {}

public sealed class EmailOtpSendEventLogMapper : IEmailOtpSendEventLogMapper
{
    public EventLogEntry Map(EmailOtpSendEvent request, EmailOtpSendResponse response) =>
        new()
        {
            TenantId = request.Data.TenantId
        };
}
