using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.AspNetCore.DI;
using Entra.EventHandlers.Hosting.Resolvers;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Entra.EventHandlers.AspNetCore.UnitTests.DI;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddEntraEventHandlers_ResolvesHandler()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEntraEventHandlers();

        // Assert
        var provider = services.BuildServiceProvider();
        var resolver = provider.GetRequiredService<IEntraEventHandlerResolver>();
        var handler = resolver.Resolve(typeof(TestEvent));

        handler.Should().BeOfType<TestHandler>();
    }


    [Fact]
    public void AddEntraEventHandlers_RegistersResolver()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEntraEventHandlers();

        // Assert
        var provider = services.BuildServiceProvider();

        provider.GetService<IEntraEventHandlerResolver>()
            .Should().NotBeNull();
    }

    [Fact]
    public void AddEntraEventHandlers_RegistersAdapters()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEntraEventHandlers();

        // Assert
        var provider = services.BuildServiceProvider();

        provider.GetService<IRequestAdapter>()
            .Should().NotBeNull();
        provider.GetService<IResponseAdapter>()
            .Should().NotBeNull();
    }
}
