using Entra.EventHandlers.Abstractions.Responses;

namespace Entra.EventHandlers.Builders.Interfaces;

/// <summary>
/// Represents the final stage of the response builder for the
/// PasswordSubmit event. At this stage the nonce has been provided,
/// an action has been selected, and the response object can be
/// constructed.
/// </summary>
public interface IPasswordSubmitResponseBuilderFinal
{
    /// <summary>
    /// Builds the <see cref="PasswordSubmitResponse"/> instance
    /// representing the configured action and nonce.
    /// </summary>
    PasswordSubmitResponse Build();
}
