using AutoFixture;
using Azure.Security.KeyVault.Secrets;
using Entra.EventHandlers.Security.Clients;
using Entra.EventHandlers.Security.Options;
using Entra.EventHandlers.Security.Providers;
using Entra.EventHandlers.Security.UnitTests.Utils;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Security.Cryptography;

namespace Entra.EventHandlers.Security.UnitTests.Providers;

public class KeyVaultCertificateProviderTests
{
    private readonly KeyVaultCertificateProvider _sut;

    private const string CertificateName = "MyCert";

    private readonly Fixture _fixture = new();

    private readonly IOptions<KeyVaultCertificateOptions> _options = new OptionsWrapper<KeyVaultCertificateOptions>(
        new KeyVaultCertificateOptions
        {
            VaultUrl = "https://dummy.vault.azure.net/",
            CertificateName = CertificateName
        });

    private readonly TestLogger<KeyVaultCertificateProvider> _logger;
    private readonly ISecretClient _secretClient;

    public KeyVaultCertificateProviderTests()
    {
        _logger = new TestLogger<KeyVaultCertificateProvider>();
        _secretClient = Substitute.For<ISecretClient>();

        _sut = new KeyVaultCertificateProvider(_logger, _options, _secretClient);
    }

    [Fact]
    public async Task GetRsaAsync_Success()
    {
        // Arrange
        var ct = new CancellationTokenSource().Token;

        var expectedRsa = RSA.Create(2048);
        var certificate = TestCertificates.CreatePfxCertificate(expectedRsa);

        var keyVaultSecret = new KeyVaultSecret(CertificateName, Convert.ToBase64String(certificate));

        _secretClient
            .GetSecretAsync(CertificateName, Arg.Is(ct))
            .Returns(keyVaultSecret);

        // Act
        var rsa = await _sut.GetRsaAsync(ct);

        // Assert
        rsa.Should().NotBeNull();
        rsa.ExportRSAPrivateKey().Should().Equal(expectedRsa.ExportRSAPrivateKey());
    }

    [Fact]
    public async Task GetRsaAsync_UsesCachedRsaOnSecondCall()
    {
        // Arrange
        var ct = new CancellationTokenSource().Token;

        var expectedRsa = RSA.Create(2048);
        var certificate = TestCertificates.CreatePfxCertificate(expectedRsa);

        var keyVaultSecret = new KeyVaultSecret(CertificateName, Convert.ToBase64String(certificate));

        _secretClient
            .GetSecretAsync(CertificateName, Arg.Is(ct))
            .Returns(keyVaultSecret);

        // Act
        var first = await _sut.GetRsaAsync(ct);
        var second = await _sut.GetRsaAsync(ct);

        // Assert
        first.Should().Be(second);
        _ = _secretClient.Received(1).GetSecretAsync(CertificateName, ct);
    }

    [Fact]
    public async Task GetRsaAsync_SecretClientThrows_LogsError()
    {
        // Arrange
        var message = _fixture.Create<string>();
        var exception = new InvalidOperationException(message);

        _secretClient
            .GetSecretAsync(CertificateName, Arg.Any<CancellationToken>())
            .Throws(exception);

        // Act
        Func<Task> act = () => _sut.GetRsaAsync(TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
           .WithMessage(message);

        _logger.Entries.Should().ContainSingle(e =>
            e.Level == LogLevel.Error &&
            e.Exception == exception &&
            e.Message.Contains($"Failed to retrieve certificate secret '{CertificateName}' from Key Vault."));
    }

    [Fact]
    public async Task GetRsaAsync_ThrowsWhenSecretIsEmpty()
    {
        // Arrange
        var keyVaultSecret = new KeyVaultSecret(CertificateName, string.Empty);

        _secretClient
            .GetSecretAsync(CertificateName, Arg.Any<CancellationToken>())
            .Returns(keyVaultSecret);

        // Act
        Func<Task> act = () => _sut.GetRsaAsync(TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
           .WithMessage($"Secret '{CertificateName}' is empty.");
    }

    [Fact]
    public async Task GetRsaAsync_ThrowsWhenCertificateHasNoPrivateKey()
    {
        // Arrange
        var certificate = TestCertificates.CreatePfxCertificateWithoutPrivateKey();

        var keyVaultSecret = new KeyVaultSecret(CertificateName, Convert.ToBase64String(certificate));

        _secretClient
            .GetSecretAsync(CertificateName, Arg.Any<CancellationToken>())
            .Returns(keyVaultSecret);

        // Act
        Func<Task> act = () => _sut.GetRsaAsync(TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
           .WithMessage($"Certificate '{CertificateName}' does not contain a private key.");
    }
}
