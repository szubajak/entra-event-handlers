using AutoFixture;
using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Entra.EventHandlers.AspNetCore.UnitTests.Abstractions;

public class EntraEndpointBaseTests
{
    private readonly TestEntraEndpointBase _sut;

    private readonly TestLogger _logger;
    private readonly IRequestAdapter _requestAdapter;
    private readonly IResponseAdapter _responseAdapter;

    public EntraEndpointBaseTests()
    {
        _logger = new TestLogger();
        _requestAdapter = Substitute.For<IRequestAdapter>();
        _responseAdapter = Substitute.For<IResponseAdapter>();

        _sut = new TestEntraEndpointBase(_logger, _requestAdapter, _responseAdapter);
    }

    [Fact]
    public async Task InvokeAsync_Calls_ExecuteAsync()
    {
        // Arrange
        bool executed = false;
        _sut.ExecuteDelegate = _ =>
        {
            executed = true;
            return Task.CompletedTask;
        };

        // Act
        await _sut.Invoke(new DefaultHttpContext());

        // Assert
        executed.Should().BeTrue();
    }

    [Fact]
    public async Task InvokeAsync_EntraException_MapsToBadRequest()
    {
        // Arrange
        var fixture = new Fixture();

        var exception = new EntraValidationException(fixture.Create<string>());

        _sut.ExecuteDelegate = _ => throw exception;

        var ctx = new DefaultHttpContext();

        // Act
        await _sut.Invoke(ctx);

        // Assert
        await _responseAdapter.Received(1)
            .WriteBadRequestAsync(ctx, Arg.Any<EntraErrorResponse>());

        _logger.Entries.Should().ContainSingle(e =>
            e.Level == LogLevel.Warning &&
            e.Exception == exception &&
            e.Message.Contains("Handled expected Entra exception."));
    }

    [Fact]
    public async Task InvokeAsync_UnexpectedException_MapsToServerError()
    {
        // Arrange
        var exception = new InvalidOperationException();

        _sut.ExecuteDelegate = _ => throw exception;

        var ctx = new DefaultHttpContext();

        // Act
        await _sut.Invoke(ctx);

        // Assert
        await _responseAdapter.Received(1)
            .WriteServerErrorAsync(ctx, Arg.Any<EntraErrorResponse>());

        _logger.Entries.Should().ContainSingle(e =>
            e.Level == LogLevel.Error &&
            e.Exception == exception &&
            e.Message.Contains("Unhandled exception while processing Entra event."));
    }
}
