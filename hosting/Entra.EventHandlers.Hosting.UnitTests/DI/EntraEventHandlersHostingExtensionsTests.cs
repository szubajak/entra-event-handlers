using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Hosting.DI;
using Entra.EventHandlers.Hosting.Resolvers;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Entra.EventHandlers.Hosting.UnitTests.DI;

public class EntraEventHandlersHostingExtensionsTests
{
    [Fact]
    public void AddEntraEventHandlersForFunctions_RegistersTestHandler()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEntraEventHandlersHosting();
        var provider = services.BuildServiceProvider();

        // Assert
        var generic = provider.GetService<IEntraEventHandler<TestEvent, TestResponse>>();
        generic.Should().NotBeNull().And.BeOfType<TestHandler>();

        var all = provider.GetServices<IEntraEventHandler>();
        all.Should().ContainSingle(h => h.GetType() == typeof(TestHandler));
    }


    [Fact]
    public void AddEntraEventHandlersForFunctions_RegistersResolver()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEntraEventHandlersHosting();

        // Assert
        var provider = services.BuildServiceProvider();

        provider.GetService<IEntraEventHandlerResolver>()
            .Should().NotBeNull();
    }
}
