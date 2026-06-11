using Entra.EventHandlers.Builders.Interfaces;
using Entra.EventHandlers.Builders.ResponseBuilders;

namespace Entra.EventHandlers.Builders;

/// <summary>
/// Provides entry points for constructing Microsoft Entra event responses.
/// Each method returns a strongly typed builder for the corresponding event
/// type, enabling fluent and guided creation of response payloads.
/// </summary>
/// <remarks>
/// These builders are part of the extended feature set and offer a structured,
/// discoverable way to construct responses for AttributeCollectionStart,
/// AttributeCollectionSubmit, TokenIssuanceStart, and EmailOtpSend events.
/// </remarks>
public class EntraEventResponses
{
    /// <summary>
    /// Creates a builder for constructing an AttributeCollectionStart response.
    /// </summary>
    public static IAttributeCollectionStartResponseBuilderStart AttributeCollectionStart() =>
        new AttributeCollectionStartResponseBuilder();

    /// <summary>
    /// Creates a builder for constructing an AttributeCollectionSubmit response.
    /// </summary>
    public static IAttributeCollectionSubmitResponseBuilderStart AttributeCollectionSubmit() =>
        new AttributeCollectionSubmitResponseBuilder();

    /// <summary>
    /// Creates a builder for constructing a TokenIssuanceStart response.
    /// </summary>
    public static ITokenIssuanceStartResponseBuilderStart TokenIssuanceStart() =>
        new TokenIssuanceStartResponseBuilder();

    /// <summary>
    /// Creates a builder for constructing a EmailOtpSend response.
    /// </summary>
    public static IEmailOtpSendResponseBuilderStart EmailOtpSend() =>
        new EmailOtpSendResponseBuilder();

    /// <summary>
    /// Creates a builder for constructing a PasswordSubmit response.
    /// </summary>
    public static IPasswordSubmitResponseBuilderStart PasswordSubmit() =>
        new PasswordSubmitResponseBuilder();
}
