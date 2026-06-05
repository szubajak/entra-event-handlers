using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.AzureFunctions.DI;
using Entra.EventHandlers.AzureFunctions.Routing;
using Entra.EventHandlers.AzureFunctions.UnitTests.Utils;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Entra.EventHandlers.AzureFunctions.UnitTests.DI;

public class EntraEventHandlersFunctionExtensionsTests
{
    [Fact]
    public void AddEntraEventHandlersForFunctions_RegistersTestHandler()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEntraEventHandlersForFunctions();
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
        services.AddEntraEventHandlersForFunctions();

        // Assert
        var provider = services.BuildServiceProvider();

        provider.GetService<IEntraEventHandlerResolver>()
            .Should().NotBeNull();
    }

    [Fact]
    public void AddEntraEventHandlersForFunctions_RegistersAdapters()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEntraEventHandlersForFunctions();

        // Assert
        var provider = services.BuildServiceProvider();

        provider.GetService<IHttpRequestAdapter>()
            .Should().NotBeNull();
        provider.GetService<IHttpResponseAdapter>()
            .Should().NotBeNull();
    }
}
