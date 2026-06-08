using AutoFixture;
using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.AzureFunctions.Routing;
using Entra.EventHandlers.Hosting.Resolvers;
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
    private readonly TestRouter _sut;

    private readonly TestLogger<EntraEventRouterFunctionBase> _logger = new();
    private readonly IEntraEventHandlerResolver _resolver;
    private readonly IRequestAdapter _requestAdapter;
    private readonly IResponseAdapter _responseAdapter;

    public EntraEventRouterFunctionBaseTests()
    {
        _resolver = Substitute.For<IEntraEventHandlerResolver>();
        _requestAdapter = Substitute.For<IRequestAdapter>();
        _responseAdapter = Substitute.For<IResponseAdapter>();

        _sut = new TestRouter(_logger, _resolver, _requestAdapter, _responseAdapter);
    }

    [Fact]
    public async Task Run_WhenDeserializationFails_ReturnsBadRequestWithDeserializationError()
    {
        // Arrange
        var fixture = new Fixture();

        var ctx = Substitute.For<FunctionContext>();
        var request = Substitute.For<HttpRequestData>(ctx);
        var response = Substitute.For<HttpResponseData>(ctx);

        var errorMessage = fixture.Create<string>();
        var exception = new EntraDeserializationException(errorMessage);
        _requestAdapter.ReadEvent(request).Throws(exception);

        _responseAdapter
            .BadRequest(request, Arg.Any<EntraErrorResponse>())
            .Returns(response);

        // Act
        var result = await _sut.RunAsync(request, ctx);

        // Assert
        _ = _responseAdapter
            .Received(1)
            .BadRequest(
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
    public async Task Run_WhenHandlerNotFound_ReturnsBadRequestWithHandlerNotFoundError()
    {
        // Arrange
        var ctx = Substitute.For<FunctionContext>();
        var request = Substitute.For<HttpRequestData>(ctx);
        var response = Substitute.For<HttpResponseData>(ctx);

        var entraEvent = new TestEvent();
        _requestAdapter.ReadEvent(request).Returns(entraEvent);

        var exception = new EntraHandlerNotFoundException(entraEvent.GetType());
        _resolver.Resolve(entraEvent.GetType()).Throws(exception);

        _responseAdapter
            .BadRequest(request, Arg.Any<EntraErrorResponse>())
            .Returns(response);

        // Act
        var result = await _sut.RunAsync(request, ctx);

        // Assert
        _ = _responseAdapter
            .Received(1)
            .BadRequest(
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
    public async Task Run_ValidationFails_ReturnsBadRequestWithValidationError()
    {
        // Arrange
        var fixture = new Fixture();

        var ctx = Substitute.For<FunctionContext>();
        var request = Substitute.For<HttpRequestData>(ctx);
        var response = Substitute.For<HttpResponseData>(ctx);

        var entraEvent = new TestEvent();
        _requestAdapter.ReadEvent(request).Returns(entraEvent);

        var handler = Substitute.For<IEntraEventHandler<TestEvent, TestResponse>>();
        _resolver.Resolve(entraEvent.GetType()).Returns(handler);

        var errorMessage = fixture.Create<string>();
        var exception = new EntraValidationException(errorMessage);
        handler.Handle(entraEvent, ctx.CancellationToken).Throws(exception);

        _responseAdapter
            .BadRequest(request, Arg.Any<EntraErrorResponse>())
            .Returns(response);

        // Act
        var result = await _sut.RunAsync(request, ctx);

        // Assert
        _ = _responseAdapter
            .Received(1)
            .BadRequest(
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
    public async Task Run_WhenUnexpectedExceptionThrown_ReturnsServerErrorWithUnhandledException()
    {
        // Arrange
        var ctx = Substitute.For<FunctionContext>();
        var request = Substitute.For<HttpRequestData>(ctx);
        var response = Substitute.For<HttpResponseData>(ctx);

        var exception = new InvalidOperationException();

        _requestAdapter.ReadEvent(request).Throws(exception);

        _responseAdapter
            .ServerError(request, Arg.Any<EntraErrorResponse>())
            .Returns(response);

        // Act
        var result = await _sut.RunAsync(request, ctx);

        // Assert
        _ = _responseAdapter
            .Received(1)
            .ServerError(
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
    public async Task Run_Success()
    {
        // Arrange
        var ctx = Substitute.For<FunctionContext>();
        var request = Substitute.For<HttpRequestData>(ctx);
        var response = Substitute.For<HttpResponseData>(ctx);

        var entraEvent = new TestEvent();
        _requestAdapter.ReadEvent(request).Returns(entraEvent);

        var handler = Substitute.For<IEntraEventHandler<TestEvent, TestResponse>>();
        _resolver.Resolve(entraEvent.GetType()).Returns(handler);

        var entraResponse = new TestResponse();
        handler.Handle(entraEvent, ctx.CancellationToken).Returns(entraResponse);

        _responseAdapter.From(request, entraResponse).Returns(response);

        // Act
        var result = await _sut.RunAsync(request, ctx);

        // Assert
        result.Should().Be(response);
        _ = handler.Received(1).Handle(entraEvent, ctx.CancellationToken);
    }
}