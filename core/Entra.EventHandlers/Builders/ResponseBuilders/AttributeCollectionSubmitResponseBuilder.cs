using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Actions.Types;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders.Interfaces;

namespace Entra.EventHandlers.Builders.ResponseBuilders;

/// <summary>
/// Concrete implementation of the response builder for the
/// AttributeCollectionSubmit event. This builder enforces the
/// valid action set for this event and produces a fully
/// constructed <see cref="AttributeCollectionSubmitResponse"/>.
/// </summary>
public class AttributeCollectionSubmitResponseBuilder : IAttributeCollectionSubmitResponseBuilderStart, IAttributeCollectionSubmitResponseBuilderFinal
{
    private EntraAction? _action;

    public IAttributeCollectionSubmitResponseBuilderFinal ContinueWithDefaultBehavior()
    {
        _action = new ContinueAction(ContinueActionType.AttributeCollectionSubmitContinueWithDefaultBehavior);

        return this;
    }

    public IAttributeCollectionSubmitResponseBuilderFinal ModifyAttributeValues(Dictionary<string, object> attributes)
    {
        _action = new ModifyAttributeValuesAction
        {
            Attributes = attributes
        };

        return this;
    }

    public IAttributeCollectionSubmitResponseBuilderFinal ShowBlockPage(string title, string message)
    {
        _action = new ShowBlockPageAction(ShowBlockPageActionType.AttributeCollectionSubmitShowBlockPage)
        {
            Message = message,
            Title = title
        };

        return this;
    }

    public IAttributeCollectionSubmitResponseBuilderFinal ShowValidationError(string message, Dictionary<string, string> attributeErrors)
    {
        _action = new ShowValidationErrorAction
        {
            Message = message,
            AttributeErrors = attributeErrors
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
    public AttributeCollectionSubmitResponse Build() => new()
    {
        Data = new()
        {
            Actions = [_action!]
        }
    };
}
