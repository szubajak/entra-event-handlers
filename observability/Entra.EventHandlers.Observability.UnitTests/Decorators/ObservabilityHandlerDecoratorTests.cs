using AutoFixture;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Results;
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
    private ObservabilityHandlerDecorator<TestEvent, TestResponse> _sut = null!;

    private readonly IEntraEventHandler<TestEvent, TestResponse> _handler;
    private readonly IEventLogPublisher _publisher;
    private readonly IEventLogMapperFactory _mapperFactory;

    public ObservabilityHandlerDecoratorTests()
    {
        _handler = Substitute.For<IEntraEventHandler<TestEvent, TestResponse>>();
        _publisher = Substitute.For<IEventLogPublisher>();
        _mapperFactory = Substitute.For<IEventLogMapperFactory>();
    }

    [Fact]
    public async Task Decorator_Should_Map_And_Publish_EventLog()
    {
        // Arrange
        var fixture = new Fixture();

        var ct = new CancellationTokenSource().Token;
        var request = new TestEvent();
        var response = new TestResponse();

        var expectedResult = new EntraHandlerResult<TestResponse>(response);

        _handler.HandleAsync(request, ct).Returns(expectedResult);

        var logEntry = fixture.Create<EventLogEntry>();
        var mapper = Substitute.For<IEventLogMapper<TestEvent, TestResponse>>();
        mapper.Map(request, response).Returns(logEntry);

        _mapperFactory.Get<TestEvent, TestResponse>().Returns(mapper);

        EventLogContext capturedEventLogContext = null!;
        _publisher.Publish(Arg.Do<EventLogContext>(x => capturedEventLogContext = x));

        var ctx = fixture.Create<EventLogContext>();
        _sut = new ObservabilityHandlerDecorator<TestEvent, TestResponse>(_handler, _publisher, _mapperFactory, ctx);

        // Act
        var result = await _sut.HandleAsync(request, ct);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        result.Response.Should().Be(response);

        _publisher.Received(1).Publish(ctx);
        capturedEventLogContext.DefaultLog.Should().Be(logEntry);
    }

    [Fact]
    public async Task Decorator_Should_ReturnResult_WhenHasException_AndNotPublish()
    {
        // Arrange
        var ct = new CancellationTokenSource().Token;
        var request = new TestEvent();

        var exception = new InvalidOperationException("Invalid!");
        var response = new TestResponse();

        var resultWithException = new EntraHandlerResult<TestResponse>(response, exception);

        _handler.HandleAsync(request, ct).Returns(resultWithException);

        var ctx = new EventLogContext
        {
            DefaultLog = null!
        };

        _sut = new ObservabilityHandlerDecorator<TestEvent, TestResponse>(_handler, _publisher, _mapperFactory, ctx);

        // Act
        var result = await _sut.HandleAsync(request, ct);

        // Assert
        result.Should().Be(resultWithException);

        _mapperFactory.DidNotReceive().Get<TestEvent, TestResponse>();
        _publisher.DidNotReceive().Publish(Arg.Any<EventLogContext>());

        ctx.DefaultLog.Should().BeNull();
    }
}
