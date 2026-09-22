using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Entra.EventHandlers.Security.UnitTests.Utils;

public static class TestCertificates
{
    public static byte[] CreatePfxCertificate(RSA rsa)
    {
        var request = new CertificateRequest("CN=Test", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddYears(1));
        return certificate.Export(X509ContentType.Pkcs12);
    }

    public static byte[] CreatePfxCertificateWithoutPrivateKey()
    {
        using var rsa = RSA.Create(2048);

        var request = new CertificateRequest("CN=Test", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        using var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddYears(1));

        byte[] publicCertBytes = certificate.Export(X509ContentType.Cert);

        using var publicOnlyCert = X509CertificateLoader.LoadCertificate(publicCertBytes);

        var collection = new X509Certificate2Collection(publicOnlyCert);
        return collection.Export(X509ContentType.Pkcs12)
            ?? throw new CryptographicException("Failed to export the public certificate collection as a PFX array.");
    }
}
