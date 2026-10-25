using AutoFixture;
using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Security.Decryptors;
using Entra.EventHandlers.Security.Providers;
using FluentAssertions;
using Jose;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Security.Cryptography;

namespace Entra.EventHandlers.Security.UnitTests.Decryptors;

public class KeyVaultPasswordContextDecryptorTests
{
    private readonly KeyVaultPasswordContextDecryptor _sut;

    private readonly Fixture _fixture = new();

    private readonly IKeyVaultCertificateProvider _certificateProvider;

    public KeyVaultPasswordContextDecryptorTests()
    {
        _certificateProvider = Substitute.For<IKeyVaultCertificateProvider>();

        _sut = new KeyVaultPasswordContextDecryptor(_certificateProvider);
    }

    [Fact]
    public async Task DecryptAsync_NullEncryptedPasswordContext_ShouldThrow()
    {
        // Act
        Func<Task> act = () => _sut.DecryptAsync(null!, TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>()
           .WithMessage("Value cannot be null. (Parameter 'encryptedPasswordContext')");
    }

    [Fact]
    public async Task DecryptAsync_EmptyEncryptedPasswordContext_ShouldThrow()
    {
        // Act
        Func<Task> act = () => _sut.DecryptAsync(string.Empty, TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
           .WithMessage("The value cannot be an empty string. (Parameter 'encryptedPasswordContext')");
    }

    [Fact]
    public async Task DecryptAsync_NullRsa_ShouldThrow()
    {
        // Arrange
        var encryptedPasswordContext = _fixture.Create<string>();
        _certificateProvider.GetRsaAsync(Arg.Any<CancellationToken>()).Returns((RSA)null!);

        // Act
        Func<Task> act = () => _sut.DecryptAsync(encryptedPasswordContext, TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<EntraSecurityException>()
           .WithMessage("The RSA key required to decrypt the password context is unavailable.");
    }

    [Fact]
    public async Task DecryptAsync_InvalidBase64_ShouldThrowEntraSecurityException()
    {
        // Arrange
        var invalidEncryptedPasswordContext = "!!";

        using var rsa = RSA.Create(2048);

        _certificateProvider.GetRsaAsync(Arg.Any<CancellationToken>()).Returns(rsa);

        // Act
        Func<Task> act = () => _sut.DecryptAsync(invalidEncryptedPasswordContext, TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<EntraSecurityException>().WithMessage("Unable to decrypt the password context.*"))
            .WithInnerException<JoseException>();
    }

    [Fact]
    public async Task DecryptAsync_OperationCanceled_ShouldPropagateWithoutWrapping()
    {
        // Arrange
        var error = _fixture.Create<string>();

        _certificateProvider.GetRsaAsync(Arg.Any<CancellationToken>())
            .Throws(new OperationCanceledException(error));

        // Act
        Func<Task> act = () => _sut.DecryptAsync(_fixture.Create<string>(), TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>()
            .WithMessage(error);
    }

    [Fact]
    public async Task DecryptAsync_InvalidJson_ShouldThrowEntraDeserializationException()
    {
        // Arrange
        var nonce = _fixture.Create<string>();
        var mail = _fixture.Create<string>();
        var password = _fixture.Create<string>();

        using var rsa = RSA.Create(2048);

        var payload =
        """
        {
          "@odata.type": "invalidJson"
        """;

        var innerJwt = JWT.Encode(payload, null, JwsAlgorithm.none);
        var encryptedPasswordContext = JWT.Encode(innerJwt, rsa, JweAlgorithm.RSA_OAEP_256, JweEncryption.A256GCM);

        _certificateProvider.GetRsaAsync(Arg.Any<CancellationToken>()).Returns(rsa);

        // Act
        Func<Task> act = () => _sut.DecryptAsync(encryptedPasswordContext, TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<EntraDeserializationException>()
            .WithMessage("Unable to deserialize the decrypted password context.");
    }

    [Fact]
    public async Task DecryptAsync_Success()
    {
        // Arrange
        var ct = new CancellationTokenSource().Token;

        var nonce = _fixture.Create<string>();
        var mail = _fixture.Create<string>();
        var password = _fixture.Create<string>();

        using var rsa = RSA.Create(2048);

        var payload = new PasswordContextPayload
        {
            Nonce = nonce,
            Username = mail,
            Password = password
        };

        var innerJwt = JWT.Encode(payload, null, JwsAlgorithm.none);
        var encryptedPasswordContext = JWT.Encode(innerJwt, rsa, JweAlgorithm.RSA_OAEP_256, JweEncryption.A256GCM);

        _certificateProvider.GetRsaAsync(ct).Returns(rsa);

        // Act
        var result = await _sut.DecryptAsync(encryptedPasswordContext, ct);

        // Assert
        result.Nonce.Should().Be(nonce);
        result.Username.Should().Be(mail);
        result.Password.Should().Be(password);
    }
}
