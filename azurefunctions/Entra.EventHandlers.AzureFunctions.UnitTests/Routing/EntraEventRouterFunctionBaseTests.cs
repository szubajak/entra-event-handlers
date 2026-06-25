using AutoFixture;
using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.Hosting.Orchestrators;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Entra.EventHandlers.AzureFunctions.UnitTests.Routing;

public class EntraEventRouterFunctionBaseTests
{
    private readonly TestEntraEventRouterFunctionBase _sut;

    private readonly TestLogger _logger;
    private readonly IEntraEventOrchestrator _orchestrator;
    private readonly IRequestAdapter _requestAdapter;
    private readonly IResponseAdapter _responseAdapter;

    public EntraEventRouterFunctionBaseTests()
    {
        _logger = new TestLogger();
        _orchestrator = Substitute.For<IEntraEventOrchestrator>();
        _requestAdapter = Substitute.For<IRequestAdapter>();
        _responseAdapter = Substitute.For<IResponseAdapter>();

        _sut = new TestEntraEventRouterFunctionBase(_logger, _orchestrator, _requestAdapter, _responseAdapter);
    }

    [Fact]
    public async Task RunAsync_WhenDeserializationFails_ReturnsBadRequestWithDeserializationError()
    {
        // Arrange
        var fixture = new Fixture();

        var ctx = Substitute.For<FunctionContext>();
        var request = Substitute.For<HttpRequestData>(ctx);
        var response = Substitute.For<HttpResponseData>(ctx);

        var errorMessage = fixture.Create<string>();
        var exception = new EntraDeserializationException(errorMessage);
        _requestAdapter
            .ReadEventAsync(request)
            .Throws(exception);

        _responseAdapter
            .BadRequestAsync(request, Arg.Any<EntraErrorResponse>())
            .Returns(response);

        // Act
        var result = await _sut.RunAsync(request);

        // Assert
        result.Should().Be(response);

        _ = _responseAdapter
            .Received(1)
            .BadRequestAsync(
                request,
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
    public async Task RunAsync_WhenHandlerNotFound_ReturnsBadRequestWithHandlerNotFoundError()
    {
        // Arrange
        var ctx = Substitute.For<FunctionContext>();
        var request = Substitute.For<HttpRequestData>(ctx);
        var response = Substitute.For<HttpResponseData>(ctx);

        var entraEvent = new TestEvent();
        _requestAdapter
            .ReadEventAsync(request)
            .Returns(entraEvent);

        var exception = new EntraHandlerNotFoundException(entraEvent.GetType());

        _orchestrator
            .DispatchAsync(entraEvent, ctx.CancellationToken)
            .Throws(exception);

        _responseAdapter
            .BadRequestAsync(request, Arg.Any<EntraErrorResponse>())
            .Returns(response);

        // Act
        var result = await _sut.RunAsync(request);

        // Assert
        result.Should().Be(response);

        _ = _responseAdapter
            .Received(1)
            .BadRequestAsync(
                request,
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
    public async Task RunAsync_ValidationFails_ReturnsBadRequestWithValidationError()
    {
        // Arrange
        var fixture = new Fixture();

        var ctx = Substitute.For<FunctionContext>();
        var request = Substitute.For<HttpRequestData>(ctx);
        var response = Substitute.For<HttpResponseData>(ctx);

        var entraEvent = new TestEvent();
        _requestAdapter
            .ReadEventAsync(request)
            .Returns(entraEvent);

        var errorMessage = fixture.Create<string>();
        var exception = new EntraValidationException(errorMessage);

        _orchestrator
            .DispatchAsync(entraEvent, ctx.CancellationToken)
            .Throws(exception);

        _responseAdapter
            .BadRequestAsync(request, Arg.Any<EntraErrorResponse>())
            .Returns(response);

        // Act
        var result = await _sut.RunAsync(request);

        // Assert
        result.Should().Be(response);

        _ = _responseAdapter
            .Received(1)
            .BadRequestAsync(
                request,
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
    public async Task RunAsync_WhenUnexpectedExceptionThrown_ReturnsServerErrorWithUnhandledException()
    {
        // Arrange
        var ctx = Substitute.For<FunctionContext>();
        var request = Substitute.For<HttpRequestData>(ctx);
        var response = Substitute.For<HttpResponseData>(ctx);

        var entraEvent = new TestEvent();
        _requestAdapter
            .ReadEventAsync(request)
            .Returns(entraEvent);

        var exception = new InvalidOperationException();

        _orchestrator
            .DispatchAsync(entraEvent, ctx.CancellationToken)
            .Throws(exception);

        _responseAdapter
            .ServerErrorAsync(request, Arg.Any<EntraErrorResponse>())
            .Returns(response);

        // Act
        var result = await _sut.RunAsync(request);

        // Assert
        result.Should().Be(response);

        _ = _responseAdapter
            .Received(1)
            .ServerErrorAsync(
                request,
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
    public async Task RunAsync_Success()
    {
        // Arrange
        var ctx = Substitute.For<FunctionContext>();
        var request = Substitute.For<HttpRequestData>(ctx);
        var response = Substitute.For<HttpResponseData>(ctx);

        var entraEvent = new TestEvent();
        _requestAdapter
            .ReadEventAsync(request)
            .Returns(entraEvent);

        var entraResponse = new TestResponse();

        _orchestrator
            .DispatchAsync(entraEvent, ctx.CancellationToken)
            .Returns(entraResponse);

        _responseAdapter
            .FromAsync(request, entraResponse)
            .Returns(response);

        // Act
        var result = await _sut.RunAsync(request);

        // Assert
        result.Should().Be(response);

        _ = _orchestrator
            .Received(1)
            .DispatchAsync(entraEvent, ctx.CancellationToken);
    }
}