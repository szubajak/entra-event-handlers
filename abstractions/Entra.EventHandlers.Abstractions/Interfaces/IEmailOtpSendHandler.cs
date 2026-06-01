using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;

namespace Entra.EventHandlers.Abstractions.Interfaces;

/// <summary>
/// Defines a handler for the EmailOtpSend event. Implementations process the
/// OTP send request and produce a valid response according to the Microsoft
/// Entra custom extension contract.
/// </summary>
/// <remarks>
/// The EmailOtpSend event is triggered when Microsoft Entra generates a
/// one‑time passcode (OTP) intended for delivery to the user. Handlers may
/// implement custom OTP delivery, apply fraud detection logic, perform
/// auditing, or allow Entra to continue with its default behavior.
///
/// For details on the EmailOtpSend event and the expected response schema,
/// see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-extension-email-otp-send-data
/// </remarks>
public interface IEmailOtpSendHandler
    : IEntraEventHandler<EmailOtpSendEvent, EmailOtpSendResponse>
{
}
