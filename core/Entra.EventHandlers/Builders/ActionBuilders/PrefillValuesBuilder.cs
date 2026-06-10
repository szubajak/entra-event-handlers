using Entra.EventHandlers.Builders.Interfaces;
using Entra.EventHandlers.Builders.ResponseBuilders;

namespace Entra.EventHandlers.Builders.ActionBuilders;

/// <summary>
/// Internal implementation of the fluent builder used to construct the
/// <c>inputs</c> dictionary for the SetPrefillValues action.
/// </summary>
public sealed class PrefillValuesBuilder(AttributeCollectionStartResponseBuilder parent) : IPrefillValuesBuilder
{
    private readonly AttributeCollectionStartResponseBuilder _parent = parent;
    private readonly Dictionary<string, object> _inputs = [];

    public IPrefillValuesBuilder Add(string key, object value)
    {
        _inputs[key] = value;
        return this;
    }

    public IAttributeCollectionStartResponseBuilderFinal Done()
    {
        return _parent.SetPrefillValues(_inputs);
    }
}
