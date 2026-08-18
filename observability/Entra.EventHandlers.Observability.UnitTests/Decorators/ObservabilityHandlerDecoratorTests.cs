using AutoFixture;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Observability.Context;
using Entra.EventHandlers.Observability.Decorators;
using Entra.EventHandlers.Observability.Factories;
using Entra.EventHandlers.Observability.Interfaces;
using Entra.EventHandlers.Observability.Logging;
using Entra.EventHandlers.Observability.Models;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using NSubstitute;

namespace Entra.EventHandlers.Observability.UnitTests.Decorators;

public class ObservabilityHandlerDecoratorTests
{
    [Fact]
    public async Task Decorator_Should_Map_And_Publish_EventLog()
    {
        // Arrange
        var fixture = new Fixture();

        var ct = new CancellationTokenSource().Token;
        var request = new TestEvent();
        var response = new TestResponse();

        var handler = Substitute.For<IEntraEventHandler<TestEvent, TestResponse>>();
        handler.HandleAsync(request, ct).Returns(response);

        var logEntry = fixture.Create<EventLogEntry>();
        var mapper = Substitute.For<IEventLogMapper<TestEvent, TestResponse>>();
        mapper.Map(request, response).Returns(logEntry);

        var mapperFactory = Substitute.For<IEventLogMapperFactory>();
        mapperFactory.Get<TestEvent, TestResponse>().Returns(mapper);

        EventLogContext capturedEventLogContext = null!;
        var publisher = Substitute.For<IEventLogPublisher>();
        publisher.Publish(Arg.Do<EventLogContext>(x => capturedEventLogContext = x));

        var ctx = fixture.Create<EventLogContext>();
        var sut = new ObservabilityHandlerDecorator<TestEvent, TestResponse>(handler, publisher, mapperFactory, ctx);

        // Act
        var result = await sut.HandleAsync(request, ct);

        // Assert
        result.Should().Be(response);

        publisher.Received(1).Publish(ctx);
        capturedEventLogContext.DefaultLog.Should().Be(logEntry);
    }
}
