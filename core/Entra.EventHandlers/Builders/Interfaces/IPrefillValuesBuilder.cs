namespace Entra.EventHandlers.Builders.Interfaces;

/// <summary>
/// Provides a fluent builder for constructing the <c>inputs</c> dictionary
/// used by the <c>SetPrefillValues</c> action in the
/// AttributeCollectionStart response. This stage allows adding
/// prefilled attribute values one entry at a time.
/// </summary>
public interface IPrefillValuesBuilder
{
    /// <summary>
    /// Adds a prefilled attribute value to the inputs collection.
    /// </summary>
    /// <param name="key">The attribute name.</param>
    /// <param name="value">The prefilled value to assign.</param>
    /// <returns>
    /// The same <see cref="IPrefillValuesBuilder"/> instance for
    /// fluent chaining.
    /// </returns>
    IPrefillValuesBuilder Add(string key, object value);

    /// <summary>
    /// Completes the prefill-values builder and returns control to the
    /// parent response builder.
    /// </summary>
    /// <returns>
    /// The final stage of the AttributeCollectionStart response builder.
    /// </returns>
    IAttributeCollectionStartResponseBuilderFinal Done();
}