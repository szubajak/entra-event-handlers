using Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;
using FluentAssertions;
using System.Net;
using System.Text;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Endpoints;

public class AttributeCollectionSubmitEndpointTests(TestAppFactory factory) : IClassFixture<TestAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Post_AttributeCollectionSubmit_ShouldReachEndpoint()
    {
        var response = await _client.PostAsync(
            "/attributecollectionsubmit",
            new StringContent("{}", Encoding.UTF8, "application/json"), 
            TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CancellationToken_ShouldBePassed()
    {
        var handler = factory.Services.GetRequiredService<TestAttributeCollectionSubmitHandler>();

        await _client.PostAsync("/attributecollectionsubmit",
            new StringContent("{}", Encoding.UTF8, "application/json"),
            TestContext.Current.CancellationToken);

        handler.CapturedCancellationToken.CanBeCanceled.Should().BeTrue();
    }
}
