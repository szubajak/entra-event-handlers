using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;

namespace Entra.EventHandlers.Abstractions.Interfaces;

/// <summary>
/// Defines a handler for the AttributeCollectionStart event.
/// Implementations process the initial attribute collection request and produce
/// a valid response according to the Microsoft Entra custom extension contract.
/// </summary>
/// <remarks>
/// The AttributeCollectionStart event is triggered at the beginning of the
/// attribute collection flow. Handlers may pre-populate attribute values,
/// determine which attributes should be collected, or influence the next step
/// in the flow.
///
/// For details on the AttributeCollectionStart event and the expected response
/// schema, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-extension-onattributecollectionstart-retrieve-return-data
/// </remarks>
public interface IAttributeCollectionStartHandler 
    : IEntraEventHandler<AttributeCollectionStartEvent, AttributeCollectionStartResponse>
{
}
