using Entra.EventHandlers.Security.Clients;
using Entra.EventHandlers.Security.DI;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Entra.EventHandlers.Security.UnitTests.DI;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddEntraEventHandlers_Invokes_Hosting_Registrations()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEntraEventHandlersSecurity();

        // Act
        var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(ISecretClient));
        descriptor.Should().NotBeNull();
    }
}