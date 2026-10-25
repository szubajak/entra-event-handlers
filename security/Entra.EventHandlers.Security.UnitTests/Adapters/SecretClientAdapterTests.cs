using AutoFixture;
using Azure;
using Azure.Security.KeyVault.Secrets;
using Entra.EventHandlers.Security.Adapters;
using FluentAssertions;
using NSubstitute;

namespace Entra.EventHandlers.Security.UnitTests.Adapters;

public class SecretClientAdapterTests
{
    private readonly SecretClientAdapter _sut;

    private readonly Fixture _fixture = new();

    private readonly SecretClient _secretClient;

    public SecretClientAdapterTests()
    {
        _secretClient = Substitute.For<SecretClient>();

        _sut = new SecretClientAdapter(_secretClient);
    }

    [Fact]
    public async Task GetSecretAsync_Success()
    {
        // Arrange
        var ct = new CancellationTokenSource().Token;
        var name = _fixture.Create<string>();
        var value = _fixture.Create<string>();

        var secret = new KeyVaultSecret(name, value);

        var response = Substitute.For<Response<KeyVaultSecret>>();
        response.Value.Returns(secret);

        _secretClient.GetSecretAsync(name, cancellationToken: ct).Returns(response);

        // Act
        var result = await _sut.GetSecretAsync(name, cancellationToken: ct);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(secret);
    }
}
