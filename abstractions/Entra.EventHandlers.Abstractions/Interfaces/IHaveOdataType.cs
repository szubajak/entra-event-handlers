namespace Entra.EventHandlers.Abstractions.Interfaces;

/// <summary>
/// Defines a contract for models that expose an <c>@odata.type</c> discriminator
/// required by the Microsoft Entra custom extension protocol.
/// </summary>
/// <remarks>
/// All request and response payloads in the Entra event model include an
/// <c>@odata.type</c> field that identifies the concrete type being sent or
/// returned. Implementations use this interface to ensure that the value is
/// always present and correctly serialized.
/// </remarks>
public interface IHaveOdataType
{
    string OdataType { get; }
}
