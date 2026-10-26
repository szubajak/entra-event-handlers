using AutoFixture;
using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.AspNetCore.Interfaces;
using Entra.EventHandlers.Hosting.Errors;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Entra.EventHandlers.AspNetCore.UnitTests.Abstractions;

public class EntraEndpointBaseTests
{
    private readonly TestEntraEndpointBase _sut;

    private readonly TestLogger _logger;
    private readonly IHttpAdapter _httpAdapter;

    public EntraEndpointBaseTests()
    {
        _logger = new TestLogger();
        _httpAdapter = Substitute.For<IHttpAdapter>();

        _sut = new TestEntraEndpointBase(_logger, _httpAdapter);
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

        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();

        var ctx = new DefaultHttpContext { RequestServices = provider };

        // Act
        await _sut.Invoke(ctx);

        // Assert
        _ = _httpAdapter.Received(1).WriteErrorAsync(ctx, StatusCodes.Status400BadRequest, Arg.Any<EntraErrorResponse>());

        _logger
            .Entries.Should()
            .ContainSingle(e =>
                e.Level == LogLevel.Warning
                && e.Exception == exception
                && e.Message.Contains("Entra domain exception occurred in hosting layer during Entra event handling.")
            );
    }

    [Fact]
    public async Task InvokeAsync_UnexpectedException_MapsToServerError()
    {
        // Arrange
        var exception = new InvalidOperationException();

        _sut.ExecuteDelegate = _ => throw exception;

        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();

        var ctx = new DefaultHttpContext { RequestServices = provider };

        // Act
        await _sut.Invoke(ctx);

        // Assert
        _ = _httpAdapter.Received(1).WriteErrorAsync(ctx, StatusCodes.Status500InternalServerError, Arg.Any<EntraErrorResponse>());

        _logger
            .Entries.Should()
            .ContainSingle(e =>
                e.Level == LogLevel.Error
                && e.Exception == exception
                && e.Message.Contains("Unexpected failure occurred in hosting layer during Entra event handling.")
            );
    }

    [Fact]
    public async Task InvokeAsync_CustomExceptionHandler_InvokesIt()
    {
        // Arrange
        var fixture = new Fixture();

        var exception = new EntraValidationException(fixture.Create<string>());

        var handler = new TestEntraExceptionHandler();

        var services = new ServiceCollection();
        services.AddSingleton<IEntraExceptionHandler>(handler);
        var provider = services.BuildServiceProvider();

        var ctx = new DefaultHttpContext { RequestServices = provider };

        _sut.ExecuteDelegate = _ => throw exception;

        // Act
        await _sut.Invoke(ctx);

        // Assert
        handler.WasCalled.Should().BeTrue();
    }
}
