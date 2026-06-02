using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Actions.Types;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders.Interfaces;

namespace Entra.EventHandlers.Builders.ResponseBuilders;

/// <summary>
/// Concrete implementation of the response builder for the
/// EmailOtpSend event. This builder enforces the valid action
/// set for this event and produces a fully constructed
/// <see cref="EmailOtpSendResponse"/>.
/// </summary>
public class EmailOtpSendResponseBuilder : IEmailOtpSendResponseBuilderStart, IEmailOtpSendResponseBuilderFinal
{
    private EntraAction? _action;

    public IEmailOtpSendResponseBuilderFinal ContinueWithDefaultBehavior()
    {
        _action = new ContinueAction(ContinueActionType.EmailOtpSendContinueWithDefaultBehavior);

        return this;
    }

    /// <summary>
    /// Builds the response object using the configured action.
    /// </summary>
    /// <remarks>
    /// The <c>_action</c> field is guaranteed to be non-null because
    /// the builder API ensures that exactly one action is selected
    /// before <see cref="Build"/> can be called.
    /// </remarks>
    public EmailOtpSendResponse Build() => new()
    {
        Data = new()
        {
            Actions = [_action!]
        }
    };
}
