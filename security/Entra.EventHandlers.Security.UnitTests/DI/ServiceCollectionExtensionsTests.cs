using AutoFixture;
using Entra.EventHandlers.Security.Clients;
using Entra.EventHandlers.Security.DI;
using Entra.EventHandlers.Security.Options;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Entra.EventHandlers.Security.UnitTests.DI;

public class ServiceCollectionExtensionsTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void AddEntraEventHandlersSecurity_Registers_ISecretClient()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEntraEventHandlersSecurity();

        // Act
        var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(ISecretClient));
        descriptor.Should().NotBeNull();
        descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddEntraEventHandlersSecurity_ThrowsOnResolve_WhenOptionsMissing()
    {
        // Arrange
        var services = new ServiceCollection();

        services.AddEntraEventHandlersSecurity();

        using var provider = services.BuildServiceProvider(validateScopes: true);

        // Act
        Action act = () => provider.GetRequiredService<ISecretClient>();

        // Assert
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("*IOptions*KeyVaultCertificateOptions*");
    }

    [Fact]
    public void AddEntraEventHandlersSecurity_ShouldCreateISecretClient_SingletonInstance()
    {
        // Arrange
        var services = new ServiceCollection();

        var options = new OptionsWrapper<KeyVaultCertificateOptions>(
            new KeyVaultCertificateOptions
            {
                VaultUrl = "https://dummy.vault.azure.net/",
                CertificateName = _fixture.Create<string>()
            });

        services.AddSingleton<IOptions<KeyVaultCertificateOptions>>(options);

        services.AddEntraEventHandlersSecurity();

        using var provider = services.BuildServiceProvider(validateScopes: true);

        // Act
        var first = provider.GetRequiredService<ISecretClient>();
        var second = provider.GetRequiredService<ISecretClient>();

        // Assert
        ReferenceEquals(first, second).Should().BeTrue();
    }

    [Fact]
    public void AddEntraEventHandlersSecurity_InvalidVaultUrl_ShouldThrowDuringResolve()
    {
        // Arrange
        var services = new ServiceCollection();

        var options = new OptionsWrapper<KeyVaultCertificateOptions>(
            new KeyVaultCertificateOptions
            {
                VaultUrl = _fixture.Create<string>(),
                CertificateName = _fixture.Create<string>()
            });

        services.AddSingleton<IOptions<KeyVaultCertificateOptions>>(options);

        services.AddEntraEventHandlersSecurity();

        using var provider = services.BuildServiceProvider(validateScopes: true);

        // Act
        Action act = () => provider.GetRequiredService<ISecretClient>();

        // Assert
        act.Should().Throw<UriFormatException>();
    }

    [Fact]
    public void AddEntraEventHandlersSecurity_Success()
    {
        // Arrange
        var services = new ServiceCollection();

        var options = new OptionsWrapper<KeyVaultCertificateOptions>(
            new KeyVaultCertificateOptions
            {
                VaultUrl = "https://dummy.vault.azure.net/",
                CertificateName = _fixture.Create<string>()
            });

        services.AddSingleton<IOptions<KeyVaultCertificateOptions>>(options);

        services.AddEntraEventHandlersSecurity();

        using var provider = services.BuildServiceProvider(validateScopes: true);

        // Act
        var client = provider.GetRequiredService<ISecretClient>();

        // Assert
        client.Should().NotBeNull();
        client.GetType().Name.Should().Be("SecretClientAdapter");
    }
}