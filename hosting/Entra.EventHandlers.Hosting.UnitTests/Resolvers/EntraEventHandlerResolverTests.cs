using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Hosting.DI;
using Entra.EventHandlers.Hosting.Resolvers;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Entra.EventHandlers.Hosting.UnitTests.Resolvers;

public class EntraEventHandlerResolverTests
{
    [Fact]
    public void Resolve_ReturnsMatchingHandler()
    {
        // Arrange
        var handler = new TestHandler();
        var resolver = new EntraEventHandlerResolver([handler]);

        // Act
        var result = resolver.Resolve(typeof(TestEvent));

        // Assert
        result.Should().Be(handler);
    }

    [Fact]
    public void Resolve_Throws_WhenHandlerNotFound()
    {
        // Arrange
        var resolver = new EntraEventHandlerResolver([]);

        // Act
        Action act = () => resolver.Resolve(typeof(TestEvent));

        // Assert
        act.Should().Throw<EntraHandlerNotFoundException>()
           .WithMessage("*TestEvent*");
    }

    [Fact]
    public void Resolve_CanResolveTestHandler()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddEntraEventHandlersHosting();

        var provider = services.BuildServiceProvider();

        var resolver = provider.GetRequiredService<IEntraEventHandlerResolver>();

        // Act
        var handler = resolver.Resolve(typeof(TestEvent));

        // Assert
        handler.Should().BeOfType<TestHandler>();
    }
}