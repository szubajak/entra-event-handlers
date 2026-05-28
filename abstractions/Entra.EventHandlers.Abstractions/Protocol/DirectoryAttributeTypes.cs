namespace Entra.EventHandlers.Abstractions.Protocol;

/// <summary>
/// Provides the attribute type identifiers used by Microsoft Entra to classify
/// directory attributes as built-in or schema extension attributes.
/// </summary>
public static class DirectoryAttributeTypes
{
    /// <summary>
    /// Indicates that the attribute is a built-in Microsoft Entra directory
    /// attribute.
    /// </summary>
    public const string BuiltIn = "builtIn";

    /// <summary>
    /// Indicates that the attribute originates from a directory schema
    /// extension.
    /// </summary>
    public const string DirectorySchemaExtension = "directorySchemaExtension";
}
