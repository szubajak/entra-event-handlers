using Entra.EventHandlers.Abstractions.Responses;

namespace Entra.EventHandlers.Builders.Interfaces;

/// <summary>
/// Represents the final stage of the response builder for the
/// AttributeCollectionStart event. At this stage all action
/// configuration is complete and the response object can be
/// constructed.
/// </summary>
public interface IAttributeCollectionStartResponseBuilderFinal
{
    /// <summary>
    /// Builds the <see cref="AttributeCollectionStartResponse"/> instance
    /// representing the configured action.
    /// </summary>
    AttributeCollectionStartResponse Build();
}