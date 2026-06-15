using AutoFixture;
using Entra.EventHandlers.Abstractions.Errors;
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

        _requestAdapter.ReadEvent(Arg.Is(httpContext)).Throws(exception);

        // Act
        await _sut.Invoke(httpContext);

        // Assert
        await _responseAdapter
            .Received()
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
}
