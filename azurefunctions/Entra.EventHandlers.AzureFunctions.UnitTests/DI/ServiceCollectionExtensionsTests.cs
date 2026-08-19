using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.AzureFunctions.DI;
using Entra.EventHandlers.Hosting.Orchestrators;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Entra.EventHandlers.AzureFunctions.UnitTests.DI;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddEntraEventHandlers_Invokes_Hosting_Registrations()
    {
        // Arrange
        var services = new ServiceCollection();
        
        // Act
        services.AddEntraEventHandlers();

        // Act
        var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(IEntraEventOrchestrator));
        descriptor.Should().NotBeNull();
    }

    [Theory]
    [MemberData(nameof(ServicesRegistrations))]
    public void AddEntraEventHandlers_Registers_Services(Type serviceType, ServiceLifetime serviceLifetime)
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEntraEventHandlers();

        // Assert
        var descriptor = services.SingleOrDefault(x => x.ServiceType == serviceType);
        descriptor.Should().NotBeNull();
        descriptor.Lifetime.Should().Be(serviceLifetime);
    }

    public static TheoryData<Type, ServiceLifetime> ServicesRegistrations() =>
        new()
        {
            { typeof(IRequestAdapter), ServiceLifetime.Singleton },
            { typeof(IResponseAdapter), ServiceLifetime.Singleton }
        };
}
