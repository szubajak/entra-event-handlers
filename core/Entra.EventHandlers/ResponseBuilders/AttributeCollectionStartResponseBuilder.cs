using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Actions.Types;
using Entra.EventHandlers.Abstractions.Responses;

namespace Entra.EventHandlers.ResponseBuilders;

public interface IAttributeCollectionStartResponseBuilderStart
{
    IAttributeCollectionStartResponseBuilderFinal ContinueWithDefaultBehavior();
    IAttributeCollectionStartResponseBuilderFinal SetPrefillValues(Dictionary<string, object> inputs);
    IAttributeCollectionStartResponseBuilderFinal ShowBlockPage(string title, string message);
}

public interface IAttributeCollectionStartResponseBuilderFinal
{
    AttributeCollectionStartResponse Build();
}

public class AttributeCollectionStartResponseBuilder : IAttributeCollectionStartResponseBuilderStart, IAttributeCollectionStartResponseBuilderFinal
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

    public IAttributeCollectionStartResponseBuilderFinal ShowBlockPage(string title, string message)
    {
        _action = new ShowBlockPageAction(ShowBlockPageActionType.AttributeCollectionStartShowBlockPage)
        {
            Message = message,
            Title = title
        };

        return this;
    }

    public AttributeCollectionStartResponse Build() => new()
    {
        Data = new()
        {
            Actions = [_action!]
        }
    };
}
