using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Workforce.Builders.ActionBuilders;
using Entra.EventHandlers.Workforce.Builders.Interfaces;

namespace Entra.EventHandlers.Workforce.Builders.ResponseBuilders;

/// <summary>
/// Concrete implementation of the response builder for the
/// VerifiedIdClaimValidation event. This builder enforces the
/// valid action set for this event and produces a fully
/// constructed <see cref="VerifiedIdClaimValidationResponse"/>.
/// </summary>
public sealed class VerifiedIdClaimValidationResponseBuilder
    : IVerifiedIdClaimValidationResponseBuilderStart, IVerifiedIdClaimValidationResponseBuilderFinal
{
    private EntraAction? _action;

    public IVerifiedIdClaimValidationResponseBuilderFinal Pass()
    {
        _action = new VerifiedIdClaimValidationPassAction();

        return this;
    }

    public IVerifiedIdClaimValidationResponseBuilderFinal Failed(IEnumerable<string> failedClaims)
    {
        _action = new VerifiedIdClaimValidationFailedAction
        {
            FailedClaims = [.. failedClaims]
        };

        return this;
    }

    public IFailedClaimsBuilder Failed()
    {
        return new FailedClaimsBuilder(this);
    }

    /// <summary>
    /// Builds the response object using the configured action.
    /// </summary>
    /// <remarks>
    /// The <c>_action</c> field is guaranteed to be non-null because
    /// the builder API ensures that exactly one action is selected
    /// before <see cref="Build"/> can be called.
    /// </remarks>
    public VerifiedIdClaimValidationResponse Build()
    {
        if (_action is null)
            throw new InvalidOperationException("An action must be selected before building the response.");

        return new VerifiedIdClaimValidationResponse
        {
            Data = new VerifiedIdClaimValidationResponsePayload
            {
                Actions = [_action]
            }
        };
    }
}
