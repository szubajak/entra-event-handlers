using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Observability.Factories;
using Entra.EventHandlers.Observability.Interfaces;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Entra.EventHandlers.Observability.UnitTests.Factories;

public class EventLogMapperFactoryTests
{
    [Fact]
    public void Get_Should_Return_Registered_Mapper()
    {
        // Arrange
        var services = new ServiceCollection();

        var mapper = Substitute.For<IEventLogMapper<TestEvent, TestResponse>>();
        services.AddSingleton(mapper);

        var provider = services.BuildServiceProvider();
        var sut = new EventLogMapperFactory(provider);

        // Act
        var result = sut.Get<TestEvent, TestResponse>();

        // Assert
        result.Should().NotBeNull();
        Assert.Throws<InvalidOperationException>(() => sut.Get<TestEvent, SomeOtherTestResponse>());
    }

    public sealed class SomeOtherTestResponse : EntraEventResponse
    {
        public string? TestProperty { get; init; }
    }
}
