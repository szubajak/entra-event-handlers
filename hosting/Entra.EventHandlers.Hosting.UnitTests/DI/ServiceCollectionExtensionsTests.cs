using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Hosting.DI;
using Entra.EventHandlers.Hosting.Orchestrators;
using Entra.EventHandlers.Hosting.Resolvers;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Entra.EventHandlers.Hosting.UnitTests.DI;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddEntraEventHandlersHosting_Registers_TestHandler()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEntraEventHandlersHosting();
        var provider = services.BuildServiceProvider();

        // Assert
        var generic = services.SingleOrDefault(x => x.ServiceType == typeof(IEntraEventHandler<TestEvent, TestResponse>));
        generic.Should().NotBeNull();
        generic.Lifetime.Should().Be(ServiceLifetime.Transient);

        var allHandlers = services.Where(x => x.ServiceType == typeof(IEntraEventHandler));

        var concrete = allHandlers.Single();
        concrete.Should().NotBeNull();
        concrete.Lifetime.Should().Be(ServiceLifetime.Transient);
    }


    [Theory]
    [MemberData(nameof(ServicesRegistrations))]
    public void AddEntraEventHandlersHosting_Registers_Services(Type serviceType, ServiceLifetime serviceLifetime)
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEntraEventHandlersHosting();

        // Assert
        var descriptor = services.SingleOrDefault(x => x.ServiceType == serviceType);
        descriptor.Should().NotBeNull();
        descriptor.Lifetime.Should().Be(serviceLifetime);
    }

    public static TheoryData<Type, ServiceLifetime> ServicesRegistrations() =>
        new()
        {
            { typeof(IEntraEventHandlerResolver), ServiceLifetime.Singleton },
            { typeof(IEntraEventOrchestrator), ServiceLifetime.Singleton }
        };
}
