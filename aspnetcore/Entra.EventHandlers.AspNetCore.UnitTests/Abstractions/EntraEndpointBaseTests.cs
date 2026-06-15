using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
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
}
