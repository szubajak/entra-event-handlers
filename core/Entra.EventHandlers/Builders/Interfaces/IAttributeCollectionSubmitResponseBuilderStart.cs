namespace Entra.EventHandlers.Builders.Interfaces;

/// <summary>
/// Defines the initial stage of the response builder for the
/// AttributeCollectionSubmit event. This stage exposes all valid
/// actions that can be returned to Microsoft Entra after the user
/// submits attribute values.
/// </summary>
public interface IAttributeCollectionSubmitResponseBuilderStart
{
    /// <summary>
    /// Returns a response instructing Entra to continue the flow
    /// with its default behavior, accepting the submitted values
    /// without modification.
    /// </summary>
    IAttributeCollectionSubmitResponseBuilderFinal ContinueWithDefaultBehavior();

    /// <summary>
    /// Returns a response that modifies one or more submitted
    /// attribute values before the flow continues. The supplied
    /// dictionary maps attribute names to their updated values.
    /// </summary>
    IAttributeCollectionSubmitResponseBuilderFinal ModifyAttributeValues(Dictionary<string, object> attributes);

    /// <summary>
    /// Returns a response instructing Entra to show a block page
    /// with the specified title and message, preventing the user
    /// from continuing the flow.
    /// </summary>
    IAttributeCollectionSubmitResponseBuilderFinal ShowBlockPage(string title, string message);

    /// <summary>
    /// Returns a response that stops the flow and returns validation
    /// errors to the user. The <paramref name="message"/> provides a
    /// general validation message, while <paramref name="attributeErrors"/>
    /// contains per-attribute error messages keyed by attribute name.
    /// </summary>
    IAttributeCollectionSubmitResponseBuilderFinal ShowValidationError(string message, Dictionary<string, string> attributeErrors);
}