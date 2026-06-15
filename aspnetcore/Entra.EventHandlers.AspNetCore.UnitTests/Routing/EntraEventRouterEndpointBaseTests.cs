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
    public async Task Run_WhenDeserializationFails_ReturnsBadRequestWithDeserializationError()
    {
        // Arrange
        var fixture = new Fixture();

        var errorMessage = fixture.Create<string>();
        var exception = new EntraDeserializationException(errorMessage);

        var httpContext = new DefaultHttpContext();

        _requestAdapter.ReadEvent(httpContext).Throws(exception);

        // Act
        await _sut.Invoke(httpContext);

        // Assert
        _ = _responseAdapter
            .Received(1)
            .WriteBadRequest(
                httpContext,
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
        var httpContext = new DefaultHttpContext();

        var entraEvent = new TestEvent();
        _requestAdapter.ReadEvent(httpContext).Returns(entraEvent);

        var exception = new EntraHandlerNotFoundException(entraEvent.GetType());
        _resolver.Resolve(entraEvent.GetType()).Throws(exception);

        // Act
        await _sut.Invoke(httpContext);

        // Assert
        _ = _responseAdapter
            .Received(1)
            .WriteBadRequest(
                httpContext,
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
        var httpContext = new DefaultHttpContext();

        var entraEvent = new TestEvent();
        _requestAdapter.ReadEvent(httpContext).Returns(entraEvent);

        var handler = Substitute.For<IEntraEventHandler<TestEvent, TestResponse>>();
        _resolver.Resolve(entraEvent.GetType()).Returns(handler);

        var errorMessage = fixture.Create<string>();
        var exception = new EntraValidationException(errorMessage);
        handler.Handle(entraEvent, httpContext.RequestAborted).Throws(exception);

        // Act
        await _sut.Invoke(httpContext);

        // Assert
        _ = _responseAdapter
            .Received(1)
            .WriteBadRequest(
                httpContext,
                Arg.Is<EntraErrorResponse>(e =>
                    e.Error == EntraErrorCodes.ValidationError &&
                    e.Details == errorMessage
                ));

        _logger.Entries.Should().ContainSingle(e =>
            e.Level == LogLevel.Warning &&
            e.Exception == exception &&
            e.Message.Contains("Router: handled expected Entra exception."));
    }
}
