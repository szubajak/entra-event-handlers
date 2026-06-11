using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Actions.Types;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders.ActionBuilders;
using Entra.EventHandlers.Builders.Interfaces;

namespace Entra.EventHandlers.Builders.ResponseBuilders;

/// <summary>
/// Concrete implementation of the response builder for the
/// AttributeCollectionStart event. This builder enforces the
/// valid action set for this event and produces a fully
/// constructed <see cref="AttributeCollectionStartResponse"/>.
/// </summary>
public sealed class AttributeCollectionStartResponseBuilder : IAttributeCollectionStartResponseBuilderStart, IAttributeCollectionStartResponseBuilderFinal
{
    private EntraAction? _action;

    public IAttributeCollectionStartResponseBuilderFinal ContinueWithDefaultBehavior()
    {
        _action = new ContinueAction(ContinueActionType.AttributeCollectionStartContinueWithDefaultBehavior);

        return this;
    }

    public IAttributeCollectionStartResponseBuilderFinal SetPrefillValues(Dictionary<string, object> inputs)
    {
        _action = new SetPrefillValuesAction
        {
            Inputs = inputs
        };

        return this;
    }

    public IPrefillValuesBuilder SetPrefillValues()
    {
        return new PrefillValuesBuilder(this);
    }

    public IAttributeCollectionStartResponseBuilderFinal ShowBlockPage(string title, string message)
    {
        _action = new ShowBlockPageAction(ShowBlockPageActionType.AttributeCollectionStartShowBlockPage)
        {
            Message = message,
            Title = title
        };

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
    public AttributeCollectionStartResponse Build()
    {
        if (_action is null)
            throw new InvalidOperationException("An action must be selected before building the response.");

        return new AttributeCollectionStartResponse
        {
            Data = new AttributeCollectionStartResponsePayload
            {
                Actions = [_action]
            }
        };
    }
}
