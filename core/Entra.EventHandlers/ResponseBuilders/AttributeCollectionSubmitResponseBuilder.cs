using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Actions.Types;
using Entra.EventHandlers.Abstractions.Responses;

namespace Entra.EventHandlers.ResponseBuilders;

public interface IAttributeCollectionSubmitResponseBuilderStart
{
    IAttributeCollectionSubmitResponseBuilderFinal ContinueWithDefaultBehavior();
    IAttributeCollectionSubmitResponseBuilderFinal ModifyAttributeValues(Dictionary<string, object> attributes);
    IAttributeCollectionSubmitResponseBuilderFinal ShowBlockPage(string title, string message);
    IAttributeCollectionSubmitResponseBuilderFinal ShowValidationError(string message, Dictionary<string, string> attributeErrors);
}

public interface IAttributeCollectionSubmitResponseBuilderFinal
{
    AttributeCollectionSubmitResponse Build();
}

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

    public AttributeCollectionSubmitResponse Build() => new()
    {
        Data = new()
        {
            Actions = [_action!]
        }
    };
}
