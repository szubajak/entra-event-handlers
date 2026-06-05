using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.AzureFunctions.DI;
using Entra.EventHandlers.AzureFunctions.Routing;
using Entra.EventHandlers.AzureFunctions.UnitTests.Utils;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Entra.EventHandlers.AzureFunctions.UnitTests.Routing;

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
        services.AddEntraEventHandlersForFunctions();

        var provider = services.BuildServiceProvider();

        var resolver = provider.GetRequiredService<IEntraEventHandlerResolver>();

        // Act
        var handler = resolver.Resolve(typeof(TestEvent));

        // Assert
        handler.Should().BeOfType<TestHandler>();
    }
}