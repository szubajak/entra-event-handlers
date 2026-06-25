using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.AzureFunctions.DI;
using Entra.EventHandlers.Hosting.Orchestrators;
using Entra.EventHandlers.Hosting.Resolvers;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Entra.EventHandlers.AzureFunctions.UnitTests.DI;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddEntraEventHandlers_ResolvesHandler()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddEntraEventHandlers();

        var provider = services.BuildServiceProvider();
        var resolver = provider.GetRequiredService<IEntraEventHandlerResolver>();

        // Act
        var handler = resolver.Resolve<TestEvent, TestResponse>();

        // Assert
        handler.Should().BeOfType<TestHandler>();
    }


    [Fact]
    public void AddEntraEventHandlers_RegistersResolver()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddEntraEventHandlers();

        // Act
        var provider = services.BuildServiceProvider();

        // Assert
        provider.GetService<IEntraEventHandlerResolver>()
            .Should().NotBeNull();
    }

    [Fact]
    public void AddEntraEventHandlers_RegistersOrchestrator()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddEntraEventHandlers();

        // Act
        var provider = services.BuildServiceProvider();

        // Assert
        provider.GetService<IEntraEventOrchestrator>()
            .Should().NotBeNull();
    }

    [Fact]
    public void AddEntraEventHandlers_RegistersAdapters()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddEntraEventHandlers();

        // Act
        var provider = services.BuildServiceProvider();

        // Assert
        provider.GetService<IRequestAdapter>()
            .Should().NotBeNull();
        provider.GetService<IResponseAdapter>()
            .Should().NotBeNull();
    }
}
