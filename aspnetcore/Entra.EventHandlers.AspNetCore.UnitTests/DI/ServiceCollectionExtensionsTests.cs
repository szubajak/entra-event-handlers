using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AspNetCore.Abstractions;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.AspNetCore.DI;
using Entra.EventHandlers.Hosting.Resolvers;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

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

    [Theory]
    [MemberData(nameof(AllEndpointTypes))]
    public void AddEntraEventHandlers_RegistersEndpoints(Type endpointType)
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();

        RegisterAllHandlersAsSubstitutes(services);

        // Act
        services.AddEntraEventHandlers();

        // Assert
        var provider = services.BuildServiceProvider();

        provider.GetService(endpointType)
            .Should().NotBeNull();
    }

    public static TheoryData<Type> AllEndpointTypes()
    {
        var data = new TheoryData<Type>();

        var endpointBase = typeof(EntraEndpointBase);

        var endpoints = endpointBase.Assembly
            .GetTypes()
            .Where(t => !t.IsAbstract && endpointBase.IsAssignableFrom(t));

        foreach (var endpoint in endpoints)
            data.Add(endpoint);

        return data;
    }

    private static void RegisterAllHandlersAsSubstitutes(IServiceCollection services)
    {
        foreach (var handler in GetAllHandlerInterfaces())
        {
            services.AddTransient(handler, _ => Substitute.For([handler], []));
        }
    }

    public static IEnumerable<Type> GetAllHandlerInterfaces()
    {
        var handlerBase = typeof(IEntraEventHandler<,>);

        return handlerBase.Assembly
            .GetTypes()
            .Where(t => t.IsInterface && t != handlerBase)
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerBase));
    }

}
