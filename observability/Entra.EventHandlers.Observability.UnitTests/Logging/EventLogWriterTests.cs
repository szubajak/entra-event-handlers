using AutoFixture;
using Entra.EventHandlers.Observability.Context;
using Entra.EventHandlers.Observability.Logging;
using Entra.EventHandlers.Observability.Models;
using FluentAssertions;

namespace Entra.EventHandlers.Observability.UnitTests.Logging;

public class EventLogWriterTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void Write_String_AddsCustomLogEntryWithMessage()
    {
        // Arrange
        var ctx = new EventLogContext
        {
            DefaultLog = _fixture.Create<EventLogEntry>()
        };

        var message = _fixture.Create<string>();

        var sut = new EventLogWriter(ctx);

        // Act
        sut.Write(message);

        // Assert
        ctx.CustomLogEntries.Should().HaveCount(1);

        var entry = ctx.CustomLogEntries.Single();
        entry.Should().NotBeNull();
        entry.Data.Should().NotBeNull();

        entry.Data.Should().BeEquivalentTo(new
        {
            Message = message
        });
    }

    [Fact]
    public void Write_Object_AddsCustomLogEntryWithObjectData()
    {
        // Arrange
        var ctx = new EventLogContext
        {
            DefaultLog = _fixture.Create<EventLogEntry>()
        };

        var obj = new
        {
            A = _fixture.Create<int>(),
            B = _fixture.Create<string>()
        };

        var sut = new EventLogWriter(ctx);

        // Act
        sut.Write(obj);

        // Assert
        ctx.CustomLogEntries.Should().HaveCount(1);

        var entry = ctx.CustomLogEntries[0];
        entry.Should().NotBeNull();
        entry.Data.Should().NotBeNull();

        entry.Data.Should().BeEquivalentTo(obj);
    }

    [Fact]
    public void Write_MultipleEntries_AppendsToCollection()
    {
        // Arrange
        var ctx = new EventLogContext
        {
            DefaultLog = _fixture.Create<EventLogEntry>()
        };

        var (entry1, entry2, entry3) = (_fixture.Create<string>(), _fixture.Create<string>(), new { X = 123 });

        var sut = new EventLogWriter(ctx);

        // Act
        sut.Write(entry1);
        sut.Write(entry2);
        sut.Write(entry3);

        // Assert
        ctx.CustomLogEntries.Should().HaveCount(3);

        ctx.CustomLogEntries[0].Data.Should().BeEquivalentTo(new { Message = entry1 });
        ctx.CustomLogEntries[1].Data.Should().BeEquivalentTo(new { Message = entry2 });
        ctx.CustomLogEntries[2].Data.Should().BeEquivalentTo(entry3);
    }
}
