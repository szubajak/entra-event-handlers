using AutoFixture;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Observability.Clients;
using Entra.EventHandlers.Observability.Context;
using Entra.EventHandlers.Observability.Decorators;
using Entra.EventHandlers.Observability.DI;
using Entra.EventHandlers.Observability.Factories;
using Entra.EventHandlers.Observability.Interfaces;
using Entra.EventHandlers.Observability.Logging;
using Entra.EventHandlers.Observability.Mappers;
using Entra.EventHandlers.Observability.Models;
using Entra.EventHandlers.Observability.UnitTests.Utils;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Entra.EventHandlers.Observability.UnitTests.DI;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddEntraEventHandlersObservability_Registers_EventLogContext_AsScoped()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEntraEventHandlersObservability();
        var provider = services.BuildServiceProvider();

        // Assert
        var scope1 = provider.CreateScope();
        var scope2 = provider.CreateScope();

        var ctx1 = scope1.ServiceProvider.GetRequiredService<EventLogContext>();
        var ctx2 = scope2.ServiceProvider.GetRequiredService<EventLogContext>();

        ctx1.Should().NotBeSameAs(ctx2);
    }

    [Fact]
    public void AddEntraEventHandlersObservability_Registers_EventLogWriter_AsScoped()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEntraEventHandlersObservability();
        var provider = services.BuildServiceProvider();

        // Assert
        var scope1 = provider.CreateScope();
        var scope2 = provider.CreateScope();

        var w1 = scope1.ServiceProvider.GetRequiredService<IEventLogWriter>();
        var w2 = scope2.ServiceProvider.GetRequiredService<IEventLogWriter>();

        w1.Should().NotBeSameAs(w2);
        w1.Should().BeOfType<EventLogWriter>();
    }

    [Theory]
    [MemberData(nameof(ServicesRegistrations))]
    public void AddEntraEventHandlersObservability_Registers_Services(Type serviceType)
    {
        // Arrange
        var services = new ServiceCollection();

        services.AddSingleton(new HttpClient(new FakeHttpMessageHandler()));

        // Act
        services.AddEntraEventHandlersObservability();
        var provider = services.BuildServiceProvider();

        // Assert
        provider.GetRequiredService(serviceType).Should().NotBeNull();
    }

    [Theory]
    [MemberData(nameof(MapperRegistrations))]
    public void AddEntraEventHandlersObservability_Registers_Mappers(Type concreteType, Type genericType)
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEntraEventHandlersObservability();
        var provider = services.BuildServiceProvider();

        // Assert
        var concrete = provider.GetRequiredService(concreteType);
        var generic = provider.GetRequiredService(genericType);

        concrete.Should().NotBeNull();
        generic.Should().BeSameAs(concrete);
    }

    [Fact]
    public void AddEntraEventHandlersObservability_Decorates_IEntraEventHandler_With_ObservabilityDecorator()
    {
        // Arrange
        var services = new ServiceCollection();

        services.AddSingleton(new HttpClient(new FakeHttpMessageHandler()));
        services.AddTransient<IEntraEventHandler<TestEvent, TestResponse>, TestHandler>();

        // Act
        services.AddEntraEventHandlersObservability();
        var provider = services.BuildServiceProvider();

        // Assert
        var handler = provider.GetRequiredService<IEntraEventHandler<TestEvent, TestResponse>>();

        handler.Should().NotBeNull();
        handler.Should().BeOfType<ObservabilityHandlerDecorator<TestEvent, TestResponse>>();
    }

    [Fact]
    public void AddEntraEventHandlersObservability_DoesNotFail_WhenNoHandlersExist()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        Action act = () => services.AddEntraEventHandlersObservability();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public async Task Decorator_Invokes_Inner_Handler_And_ObservabilityPipeline()
    {
        // Arrange
        var fixture = new Fixture();

        var ct = new CancellationTokenSource().Token;
        var request = new TestEvent();

        var services = new ServiceCollection();

        services.AddSingleton(new HttpClient(new FakeHttpMessageHandler()));

        var testHandler = new TestHandler();
        services.AddSingleton<IEntraEventHandler<TestEvent, TestResponse>>(testHandler);

        services.AddEntraEventHandlersObservability();

        var mapper = Substitute.For<IEventLogMapper<TestEvent, TestResponse>>();
        var logEntry = fixture.Create<EventLogEntry>();

        // Reponse used to build EventLogContext
        TestResponse capturedResponse = null!;
        mapper.Map(request, Arg.Do<TestResponse>(x => capturedResponse = x)).Returns(logEntry);
        services.AddSingleton(mapper);

        var publisher = Substitute.For<IEventLogPublisher>();
        services.AddSingleton(publisher);

        // Act
        var provider = services.BuildServiceProvider();

        // Assert
        var handler = provider.GetRequiredService<IEntraEventHandler<TestEvent, TestResponse>>();
        handler.Should().BeOfType<ObservabilityHandlerDecorator<TestEvent, TestResponse>>();

        var response = await handler.HandleAsync(request, ct);
        capturedResponse.Should().Be(response);

        testHandler.WasCalled.Should().BeTrue();
        testHandler.CapturedCancellationToken.Should().Be(ct);

        mapper.Received(1).Map(request, response);
        publisher.Received(1).Publish(Arg.Is<EventLogContext>(c => c.DefaultLog == logEntry));
    }

    public static TheoryData<Type> ServicesRegistrations() =>
        new()
        {
            { typeof(IEventLogPublisher) },
            { typeof(IObservabilityApiClient) },
            { typeof(IEventLogMapperFactory) },
            { typeof(IEventLogContextMapper) }
        };

    public static TheoryData<Type, Type> MapperRegistrations() =>
        new()
        {
            { typeof(IEmailOtpSendEventLogMapper), typeof(IEventLogMapper<EmailOtpSendEvent, EmailOtpSendResponse>) }
        };
}
