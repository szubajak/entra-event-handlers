using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Actions.Types;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders.Interfaces;

namespace Entra.EventHandlers.Builders.ResponseBuilders;

/// <summary>
/// Concrete implementation of the response builder for the
/// PasswordSubmit event. This builder enforces the required nonce
/// and the valid action set for this event, producing a fully
/// constructed <see cref="PasswordSubmitResponse"/>.
/// </summary>
public sealed class PasswordSubmitResponseBuilder :
    IPasswordSubmitResponseBuilderStart,
    IPasswordSubmitResponseBuilderActions,
    IPasswordSubmitResponseBuilderFinal
{
    private string? _nonce;
    private EntraAction? _action;

    public IPasswordSubmitResponseBuilderActions WithNonce(string nonce)
    {
        _nonce = nonce;
        return this;
    }

    public IPasswordSubmitResponseBuilderFinal MigratePassword()
    {
        _action = new PasswordSubmitAction(PasswordSubmitActionType.MigratePassword);
        return this;
    }

    public IPasswordSubmitResponseBuilderFinal UpdatePassword()
    {
        _action = new PasswordSubmitAction(PasswordSubmitActionType.UpdatePassword);
        return this;
    }

    public IPasswordSubmitResponseBuilderFinal Retry()
    {
        _action = new PasswordSubmitAction(PasswordSubmitActionType.Retry);
        return this;
    }

    public IPasswordSubmitResponseBuilderFinal Block()
    {
        _action = new PasswordSubmitAction(PasswordSubmitActionType.Block);
        return this;
    }

    /// <summary>
    /// Builds the response object using the configured nonce and action.
    /// </summary>
    /// <remarks>
    /// The <c>_nonce</c> and <c>_action</c> fields are guaranteed to be
    /// non-null because the builder API enforces that the nonce is set
    /// and exactly one action is selected before <see cref="Build"/>
    /// can be called.
    /// </remarks>
    public PasswordSubmitResponse Build()
    {
        if (_nonce is null)
            throw new InvalidOperationException("Nonce must be set before building the response.");

        if (_action is null)
            throw new InvalidOperationException("An action must be selected before building the response.");

        return new PasswordSubmitResponse
        {
            Data = new PasswordSubmitResponsePayload
            {
                Nonce = _nonce,
                Actions = [_action]
            }
        };
    }
}
