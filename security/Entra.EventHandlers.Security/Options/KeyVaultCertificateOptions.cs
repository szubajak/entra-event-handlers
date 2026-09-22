namespace Entra.EventHandlers.Security.Options;

public sealed class KeyVaultCertificateOptions
{
    public const string SectionName = "KeyVault";

    public required string VaultUrl { get; init; }

    public required string CertificateName { get; init; }
}