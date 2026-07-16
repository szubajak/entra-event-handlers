using AutoFixture;
using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.Hosting.Orchestrators;
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
    private readonly IEntraEventOrchestrator _orchestrator;
    private readonly IRequestAdapter _requestAdapter;
    private readonly IResponseAdapter _responseAdapter;

    public EntraEventRouterEndpointBaseTests()
    {
        _logger = new TestLogger();
        _orchestrator = Substitute.For<IEntraEventOrchestrator>();
        _requestAdapter = Substitute.For<IRequestAdapter>();
        _responseAdapter = Substitute.For<IResponseAdapter>();

        _sut = new TestEntraEventRouterEndpointBase(_logger, _orchestrator, _requestAdapter, _responseAdapter);
    }

    [Fact]
    public async Task InvokeAsync_WhenDeserializationFails_ReturnsBadRequestWithDeserializationError()
    {
        // Arrange
        var fixture = new Fixture();

        var errorMessage = fixture.Create<string>();
        var exception = new EntraDeserializationException(errorMessage);

        var ctx = new DefaultHttpContext();

        _requestAdapter
            .ReadEventAsync(ctx)
            .Throws(exception);

        // Act
        await _sut.Invoke(ctx);

        // Assert
        _ = _responseAdapter
            .Received(1)
            .WriteBadRequestAsync(
                ctx,
                Arg.Is<EntraErrorResponse>(e =>
                    e != null &&
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
        _requestAdapter
            .ReadEventAsync(ctx)
            .Returns(entraEvent);

        var exception = new EntraHandlerNotFoundException(entraEvent.GetType());
        _orchestrator
            .DispatchAsync(entraEvent, ctx.RequestAborted)
            .Throws(exception);

        // Act
        await _sut.Invoke(ctx);

        // Assert
        _ = _responseAdapter
            .Received(1)
            .WriteBadRequestAsync(
                ctx,
                Arg.Is<EntraErrorResponse>(e =>
                    e != null &&
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
        _requestAdapter
            .ReadEventAsync(ctx)
            .Returns(entraEvent);

        var errorMessage = fixture.Create<string>();
        var exception = new EntraValidationException(errorMessage);
        _orchestrator
            .DispatchAsync(entraEvent, ctx.RequestAborted)
            .Throws(exception);

        // Act
        await _sut.Invoke(ctx);

        // Assert
        _ = _responseAdapter
            .Received(1)
            .WriteBadRequestAsync(
                ctx,
                Arg.Is<EntraErrorResponse>(e =>
                    e != null &&
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
        _requestAdapter
            .ReadEventAsync(ctx)
            .Returns(entraEvent);

        var exception = new InvalidOperationException();
        _orchestrator
            .DispatchAsync(entraEvent, ctx.RequestAborted)
            .Throws(exception);

        // Act
        await _sut.Invoke(ctx);

        // Assert
        _ = _responseAdapter
            .Received(1)
            .WriteServerErrorAsync(
                ctx,
                Arg.Is<EntraErrorResponse>(e =>
                    e != null &&
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
        _requestAdapter
            .ReadEventAsync(ctx)
            .Returns(entraEvent);

        var entraResponse = new TestResponse();
        _orchestrator
            .DispatchAsync(entraEvent, ctx.RequestAborted)
            .Returns(entraResponse);

        _responseAdapter
            .WriteOkAsync(ctx, entraResponse)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Invoke(ctx);

        // Assert
        _ = _orchestrator
            .Received(1)
            .DispatchAsync(entraEvent, ctx.RequestAborted);

        _ = _responseAdapter
            .Received(1)
            .WriteOkAsync(ctx, entraResponse);
    }
}
