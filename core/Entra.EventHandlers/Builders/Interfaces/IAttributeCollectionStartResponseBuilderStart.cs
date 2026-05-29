namespace Entra.EventHandlers.Builders.Interfaces;

/// <summary>
/// Defines the initial stage of the response builder for the
/// AttributeCollectionStart event. This stage exposes all valid
/// actions that can be returned to Microsoft Entra during the
/// attribute collection start flow.
/// </summary>
public interface IAttributeCollectionStartResponseBuilderStart
{
    /// <summary>
    /// Returns a response instructing Entra to continue the flow
    /// with its default behavior.
    /// </summary>
    IAttributeCollectionStartResponseBuilderFinal ContinueWithDefaultBehavior();

    /// <summary>
    /// Returns a response that provides prefilled attribute values
    /// using the supplied dictionary.
    /// </summary>
    IAttributeCollectionStartResponseBuilderFinal SetPrefillValues(Dictionary<string, object> inputs);

    /// <summary>
    /// Begins a fluent builder for constructing prefilled attribute
    /// values one entry at a time.
    /// </summary>
    IPrefillValuesBuilder SetPrefillValues();

    /// <summary>
    /// Returns a response instructing Entra to show a block page
    /// with the specified title and message.
    /// </summary>
    IAttributeCollectionStartResponseBuilderFinal ShowBlockPage(string title, string message);
}
