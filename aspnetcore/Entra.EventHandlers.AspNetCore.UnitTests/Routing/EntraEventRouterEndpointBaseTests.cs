using AutoFixture;
using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Abstractions.Results;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.AspNetCore.Interfaces;
using Entra.EventHandlers.Hosting.Errors;
using Entra.EventHandlers.Hosting.Orchestrators;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Entra.EventHandlers.AspNetCore.UnitTests.Routing;

public class EntraEventRouterEndpointBaseTests
{
    private readonly TestEntraEventRouterEndpointBase _sut;

    private readonly Fixture _fixture = new();
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
        var errorMessage = _fixture.Create<string>();
        var exception = new EntraDeserializationException(errorMessage);

        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();

        var ctx = new DefaultHttpContext
        {
            RequestServices = provider
        };

        _requestAdapter
            .ReadEventAsync(ctx)
            .Throws(exception);

        // Act
        await _sut.Invoke(ctx);

        // Assert
        _ = _responseAdapter
            .Received(1)
            .WriteErrorAsync(
                ctx,
                StatusCodes.Status400BadRequest,
                Arg.Is<EntraErrorResponse>(e =>
                    e != null &&
                    e.Error == EntraErrorCodes.DeserializationError &&
                    e.Details == errorMessage
            ));

        _logger.Entries.Should().ContainSingle(e =>
            e.Level == LogLevel.Warning &&
            e.Exception == exception &&
            e.Message.Contains("Entra domain exception occurred in hosting layer during Entra event handling."));
    }

    [Fact]
    public async Task InvokeAsync_WhenHandlerNotFound_ReturnsBadRequestWithHandlerNotFoundError()
    {
        // Arrange
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();

        var ctx = new DefaultHttpContext
        {
            RequestServices = provider
        };

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
            .WriteErrorAsync(
                ctx,
                StatusCodes.Status400BadRequest,
                Arg.Is<EntraErrorResponse>(e =>
                    e != null &&
                    e.Error == EntraErrorCodes.HandlerNotFound &&
                    !string.IsNullOrEmpty(e.Details) &&
                    e.Details.Contains(entraEvent.GetType().Name)
            ));

        _logger.Entries.Should().ContainSingle(e =>
            e.Level == LogLevel.Warning &&
            e.Exception == exception &&
            e.Message.Contains("Entra domain exception occurred in hosting layer during Entra event handling."));
    }

    [Fact]
    public async Task InvokeAsync_ValidationFails_ReturnsBadRequestWithValidationError()
    {
        // Arrange
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();

        var ctx = new DefaultHttpContext
        {
            RequestServices = provider
        };

        var entraEvent = new TestEvent();
        _requestAdapter
            .ReadEventAsync(ctx)
            .Returns(entraEvent);

        var errorMessage = _fixture.Create<string>();
        var exception = new EntraValidationException(errorMessage);
        _orchestrator
            .DispatchAsync(entraEvent, ctx.RequestAborted)
            .Throws(exception);

        // Act
        await _sut.Invoke(ctx);

        // Assert
        _ = _responseAdapter
            .Received(1)
            .WriteErrorAsync(
                ctx,
                StatusCodes.Status400BadRequest,
                Arg.Is<EntraErrorResponse>(e =>
                    e != null &&
                    e.Error == EntraErrorCodes.ValidationError &&
                    e.Details == errorMessage
            ));

        _logger.Entries.Should().ContainSingle(e =>
            e.Level == LogLevel.Warning &&
            e.Exception == exception &&
            e.Message.Contains("Entra domain exception occurred in hosting layer during Entra event handling."));
    }

    [Fact]
    public async Task InvokeAsync_WhenUnexpectedExceptionThrown_ReturnsServerErrorWithUnhandledException()
    {
        // Arrange
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();

        var ctx = new DefaultHttpContext
        {
            RequestServices = provider
        };

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
            .WriteErrorAsync(
                ctx,
                StatusCodes.Status500InternalServerError,
                Arg.Is<EntraErrorResponse>(e =>
                    e != null &&
                    e.Error == EntraErrorCodes.UnhandledException &&
                    e.Details == "Unexpected failure occurred."
            ));

        _logger.Entries.Should().ContainSingle(e =>
            e.Level == LogLevel.Error &&
            e.Exception == exception &&
            e.Message.Contains("Unexpected failure occurred in hosting layer during Entra event handling."));
    }

    [Fact]
    public async Task InvokeAsync_WhenResultHasException_CallsExceptionHandler()
    {
        // Arrange
        var services = new ServiceCollection();
        var exceptionHandler = Substitute.For<IEntraExceptionHandler>();
        services.AddSingleton(exceptionHandler);
        var provider = services.BuildServiceProvider();

        var ctx = new DefaultHttpContext
        {
            RequestServices = provider
        };

        var entraEvent = new TestEvent();
        _requestAdapter
            .ReadEventAsync(ctx)
            .Returns(entraEvent);

        var entraResponse = new TestResponse();
        var exception = new InvalidOperationException("Invalid!");

        var handlerResult = new EntraHandlerResult<EntraEventResponse>(entraResponse, exception);

        _orchestrator
            .DispatchAsync(entraEvent, ctx.RequestAborted)
            .Returns(handlerResult);

        _responseAdapter
            .WriteOkAsync(ctx, entraResponse)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Invoke(ctx);

        // Assert
        _ = exceptionHandler
            .Received(1)
            .HandleAsync(exception);

        _ =_responseAdapter
            .Received(1)
            .WriteOkAsync(ctx, entraResponse);
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

        var expectedResult = new EntraHandlerResult<EntraEventResponse>(entraResponse);

        _orchestrator
            .DispatchAsync(entraEvent, ctx.RequestAborted)
            .Returns(expectedResult);

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
