using AutoFixture;
using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.Hosting.Resolvers;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Entra.EventHandlers.AspNetCore.UnitTests.Routing;

public class EntraEventRouterEndpointBaseTests
{
    private readonly TestEntraEventRouterEndpointBase _sut;

    private readonly TestLogger _logger;
    private readonly IEntraEventHandlerResolver _resolver;
    private readonly IRequestAdapter _requestAdapter;
    private readonly IResponseAdapter _responseAdapter;

    public EntraEventRouterEndpointBaseTests()
    {
        _logger = new TestLogger();
        _resolver = Substitute.For<IEntraEventHandlerResolver>();
        _requestAdapter = Substitute.For<IRequestAdapter>();
        _responseAdapter = Substitute.For<IResponseAdapter>();

        _sut = new TestEntraEventRouterEndpointBase(_logger, _resolver, _requestAdapter, _responseAdapter);
    }

    [Fact]
    public async Task InvokeAsync_WhenDeserializationFails_ReturnsBadRequestWithDeserializationError()
    {
        // Arrange
        var fixture = new Fixture();

        var errorMessage = fixture.Create<string>();
        var exception = new EntraDeserializationException(errorMessage);

        var ctx = new DefaultHttpContext();

        _requestAdapter.ReadEvent(ctx).Throws(exception);

        // Act
        await _sut.Invoke(ctx);

        // Assert
        _ = _responseAdapter
            .Received(1)
            .WriteBadRequest(
                ctx,
                Arg.Is<EntraErrorResponse>(e =>
                    e.Error == EntraErrorCodes.DeserializationError &&
                    e.Details == errorMessage
                ));

        _logger.Entries.Should().ContainSingle(e =>
            e.Level == LogLevel.Warning &&
            e.Exception == exception &&
            e.Message.Contains("Router: handled expected Entra exception."));
    }

    [Fact]
    public async Task InvokeAsync_WhenHandlerNotFound_ReturnsBadRequestWithHandlerNotFoundError()
    {
        // Arrange
        var ctx = new DefaultHttpContext();

        var entraEvent = new TestEvent();
        _requestAdapter.ReadEvent(ctx).Returns(entraEvent);

        var exception = new EntraHandlerNotFoundException(entraEvent.GetType());
        _resolver.Resolve(entraEvent.GetType()).Throws(exception);

        // Act
        await _sut.Invoke(ctx);

        // Assert
        _ = _responseAdapter
            .Received(1)
            .WriteBadRequest(
                ctx,
                Arg.Is<EntraErrorResponse>(e =>
                    e.Error == EntraErrorCodes.HandlerNotFound &&
                    !string.IsNullOrEmpty(e.Details) &&
                    e.Details.Contains(entraEvent.GetType().Name)
          ));

        _logger.Entries.Should().ContainSingle(e =>
            e.Level == LogLevel.Warning &&
            e.Exception == exception &&
            e.Message.Contains("Router: handled expected Entra exception."));
    }

    [Fact]
    public async Task InvokeAsync_ValidationFails_ReturnsBadRequestWithValidationError()
    {
        // Arrange
        var fixture = new Fixture();
        var ctx = new DefaultHttpContext();

        var entraEvent = new TestEvent();
        _requestAdapter.ReadEvent(ctx).Returns(entraEvent);

        var handler = Substitute.For<IEntraEventHandler<TestEvent, TestResponse>>();
        _resolver.Resolve(entraEvent.GetType()).Returns(handler);

        var errorMessage = fixture.Create<string>();
        var exception = new EntraValidationException(errorMessage);
        handler.Handle(entraEvent, ctx.RequestAborted).Throws(exception);

        // Act
        await _sut.Invoke(ctx);

        // Assert
        _ = _responseAdapter
            .Received(1)
            .WriteBadRequest(
                ctx,
                Arg.Is<EntraErrorResponse>(e =>
                    e.Error == EntraErrorCodes.ValidationError &&
                    e.Details == errorMessage
                ));

        _logger.Entries.Should().ContainSingle(e =>
            e.Level == LogLevel.Warning &&
            e.Exception == exception &&
            e.Message.Contains("Router: handled expected Entra exception."));
    }

    [Fact]
    public async Task InvokeAsync_WhenUnexpectedExceptionThrown_ReturnsServerErrorWithUnhandledException()
    {
        // Arrange
        var ctx = new DefaultHttpContext();

        var entraEvent = new TestEvent();
        _requestAdapter.ReadEvent(ctx).Returns(entraEvent);

        var handler = Substitute.For<IEntraEventHandler<TestEvent, TestResponse>>();
        _resolver.Resolve(entraEvent.GetType()).Returns(handler);

        var exception = new InvalidOperationException();
        handler.Handle(entraEvent, ctx.RequestAborted).Throws(exception);

        // Act
        await _sut.Invoke(ctx);

        // Assert
        _ = _responseAdapter
            .Received(1)
            .WriteServerError(
                ctx,
                Arg.Is<EntraErrorResponse>(e =>
                    e.Error == EntraErrorCodes.UnhandledException &&
                    e.Details == "An unexpected error occurred."
                ));

        _logger.Entries.Should().ContainSingle(e =>
            e.Level == LogLevel.Error &&
            e.Exception == exception &&
            e.Message.Contains("Router: unhandled exception while processing Entra event."));
    }

    [Fact]
    public async Task InvokeAsync_Success()
    {
        // Arrange
        var ctx = new DefaultHttpContext();

        var entraEvent = new TestEvent();
        _requestAdapter.ReadEvent(ctx).Returns(entraEvent);

        var handler = Substitute.For<IEntraEventHandler<TestEvent, TestResponse>>();
        _resolver.Resolve(entraEvent.GetType()).Returns(handler);

        var entraResponse = new TestResponse();
        handler.Handle(entraEvent, ctx.RequestAborted).Returns(entraResponse);

        _responseAdapter.WriteOk(ctx, entraResponse).Returns(Task.CompletedTask);

        // Act
        await _sut.Invoke(ctx);

        // Assert
        _ = handler.Received(1).Handle(entraEvent, ctx.RequestAborted);
        _ = _responseAdapter.Received(1).WriteOk(ctx, entraResponse);
    }
}
