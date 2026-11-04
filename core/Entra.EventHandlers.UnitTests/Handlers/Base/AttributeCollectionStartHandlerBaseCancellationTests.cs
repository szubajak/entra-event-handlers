using AutoFixture;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.TestData;
using Entra.EventHandlers.TestHelpers;
using Entra.EventHandlers.UnitTests.Utils.Handlers;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.UnitTests.Handlers.Base;

public class AttributeCollectionStartHandlerBaseCancellationTests
{
    [Fact]
    public async Task HandleAsync_Cancellation_PropagatesAndLogsInformation()
    {
        // Arrange
        var fixture = new Fixture();
        var evt = TestEvents.CreateAttributeCollectionStartEvent(fixture);
        var logger = new TestLogger();
        var sut = new TestCancellableAttributeCollectionStartHandler(logger);

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act/Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() => sut.HandleAsync(evt, cts.Token));

        // Core should be invoked exactly once with the provided token
        sut.CoreTest.HandleCoreCallCount.Should().Be(1);
        sut.CoreTest.CapturedCancellationToken.Should().Be(cts.Token);

        // Verify an Information-level log indicating cancellation
        logger.Entries.Should().Contain(e =>
            e.Level == LogLevel.Information &&
            e.Message.Contains("was canceled", StringComparison.OrdinalIgnoreCase));

        var cancellationLog = logger.Entries.Single(e =>
            e.Level == LogLevel.Information &&
            e.Message.Contains("was canceled", StringComparison.OrdinalIgnoreCase));

        // Ensure duration is logged as structured state
        var state = cancellationLog.State.As<IReadOnlyList<KeyValuePair<string, object>>>();
        state.Should().Contain(kv => kv.Key == "DurationMs");

        // No response is returned because the exception propagates
    }
}
