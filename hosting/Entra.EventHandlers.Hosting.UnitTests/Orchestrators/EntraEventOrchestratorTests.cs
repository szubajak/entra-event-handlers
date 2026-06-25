using AutoFixture;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Hosting.Orchestrators;
using Entra.EventHandlers.Hosting.Resolvers;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using NSubstitute;

namespace Entra.EventHandlers.Hosting.UnitTests.Orchestrators;

public class EntraEventOrchestratorTests
{
    private readonly EntraEventOrchestrator _sut;

    private readonly IEntraEventHandlerResolver _resolver;

    public EntraEventOrchestratorTests()
    {
        _resolver = Substitute.For<IEntraEventHandlerResolver>();

        _sut = new EntraEventOrchestrator(_resolver);
    }

    [Fact]
    public async Task DispatchAsync_Success()
    {
        // Arrange
        var fixture = new Fixture();

        var evt = fixture.Create<AttributeCollectionStartEvent>();

        var response = new AttributeCollectionStartResponse();

        var cts = new CancellationTokenSource();

        var handler = Substitute.For<IEntraEventHandler<AttributeCollectionStartEvent, AttributeCollectionStartResponse>>();
        handler.HandleAsync(evt, cts.Token).Returns(response);

        _resolver.Resolve<AttributeCollectionStartEvent, AttributeCollectionStartResponse>()
            .Returns(handler);

        // Act
        var result = await _sut.DispatchAsync(evt, cts.Token);

        // Assert
        result.Should().Be(response);
    }

    [Fact]
    public async Task DispatchAsync_UnknownEvent_ThrowsNotSupportedException()
    {
        // Arrange
        var evt = new TestEvent();

        // Act
        var act = () => _sut.DispatchAsync(evt, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotSupportedException>()
            .WithMessage("Unsupported event type*");
    }
}