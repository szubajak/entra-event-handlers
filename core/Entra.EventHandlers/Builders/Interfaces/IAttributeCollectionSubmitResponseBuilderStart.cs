namespace Entra.EventHandlers.Builders.Interfaces;

public interface IAttributeCollectionSubmitResponseBuilderStart
{
    IAttributeCollectionSubmitResponseBuilderFinal ContinueWithDefaultBehavior();
    IAttributeCollectionSubmitResponseBuilderFinal ModifyAttributeValues(Dictionary<string, object> attributes);
    IAttributeCollectionSubmitResponseBuilderFinal ShowBlockPage(string title, string message);
    IAttributeCollectionSubmitResponseBuilderFinal ShowValidationError(string message, Dictionary<string, string> attributeErrors);
}