using Entra.EventHandlers.Abstractions.Responses;

namespace Entra.EventHandlers.Builders.Interfaces;

/// <summary>
/// Represents the final stage of the response builder for the
/// EmailOtpSend event. At this stage all action configuration is
/// complete and the response object can be constructed.
/// </summary>
public interface IEmailOtpSendResponseBuilderFinal
{
    /// <summary>
    /// Builds the <see cref="EmailOtpSendResponse"/> instance
    /// representing the configured action.
    /// </summary>
    EmailOtpSendResponse Build();
}