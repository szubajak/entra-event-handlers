namespace Entra.EventHandlers.Builders.Interfaces;

/// <summary>
/// Defines the initial stage of the response builder for the
/// EmailOtpSend event. This stage exposes all valid actions that
/// can be returned to Microsoft Entra during the email OTP send flow.
/// </summary>
public interface IEmailOtpSendResponseBuilderStart
{
    /// <summary>
    /// Returns a response instructing Entra to continue the flow
    /// with its default behavior.
    /// </summary>
    IEmailOtpSendResponseBuilderFinal ContinueWithDefaultBehavior();
}
