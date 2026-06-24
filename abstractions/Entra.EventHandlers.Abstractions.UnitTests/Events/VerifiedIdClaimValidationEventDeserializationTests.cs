using AutoFixture;
using Entra.EventHandlers.Abstractions.Errors;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Abstractions.Protocol.Authentication;
using Entra.EventHandlers.TestHelpers;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Text.Json;

namespace Entra.EventHandlers.Abstractions.UnitTests.Events;

public class VerifiedIdClaimValidationEventDeserializationTests
{
    [Fact]
    public void FullEventRequest_DeserializesCorrectly()
    {
        // Arrange
        var fixture = new Fixture();

        var tenantId = fixture.Create<Guid>();
        var appId = fixture.Create<Guid>();
        var correlationId = fixture.Create<Guid>();

        var userPrincipalName = $"{fixture.Create<string>()}@example.com";

        var json =
        $$"""
        {
          "type": "microsoft.graph.authenticationEvent.verifiedIdClaimValidation",
          "source": "/tenants/{{tenantId}}/applications/{{appId}}",
          "data": {
            "@odata.type": "microsoft.graph.onVerifiedIdClaimValidationCalloutData",
            "tenantId": "{{tenantId}}",
            "authenticationContext": {
              "correlationId": "{{correlationId}}",
              "user": {
                "userPrincipalName": "{{userPrincipalName}}"
              }
            }
          }
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<EntraEvent>(json);

        // Assert
        using (new AssertionScope())
        {
            var evt = result.Should().BeOfType<VerifiedIdClaimValidationEvent>().Which;
            evt.Validate();

            evt.Type.Should().Be(EntraEventTypes.VerifiedIdClaimValidation);
            evt.Source.Should().Be($"/tenants/{tenantId}/applications/{appId}");

            var payload = evt.Data;
            payload.Should().NotBeNull();
            payload.OdataType.Should().Be(EntraOdataTypes.VerifiedIdClaimValidation.CalloutData);
            payload.TenantId.Should().Be(tenantId);

            var ctx = payload.AuthenticationContext;
            ctx.Should().NotBeNull();
            ctx.CorrelationId.Should().Be(correlationId);
            ctx.User.Should().NotBeNull()
                .And.Subject.Should().Match<UserInfo>(x =>
                    x.UserPrincipalName == userPrincipalName
                );

            evt.CorrelationId.Should().Be(payload.AuthenticationContext.CorrelationId);
        }
    }

    [Fact]
    public void MinimalEventRequest_DeserializesCorrectly()
    {
        // Act
        var result = JsonSerializer.Deserialize<EntraEvent>(EventSamples.VerifiedIdClaimValidation());

        // Assert
        using (new AssertionScope())
        {
            var evt = result.Should().BeOfType<VerifiedIdClaimValidationEvent>().Which;
            evt.Validate();

            var payload = evt.Data;
            payload.Should().NotBeNull();
            payload.VerifiedIdClaimsContext.Should().BeNull();  

            var ctx = payload.AuthenticationContext;
            ctx.Should().NotBeNull();
            ctx.Client.Should().BeNull();
            ctx.ClientServicePrincipal.Should().BeNull();
            ctx.ResourceServicePrincipal.Should().BeNull();
            ctx.Protocol.Should().BeNull();
            ctx.User.Should().BeNull();
        }
    }

    [Fact]
    public void InvalidOdataType_ThrowsEntraValidationException()
    {
        var evt = JsonSerializer.Deserialize<EntraEvent>(EventSamples.VerifiedIdClaimValidation(odataType: "invalid"));

        // Act
        Action act = () => ((VerifiedIdClaimValidationEvent)evt!).Validate();

        // Assert
        act.Should().Throw<EntraValidationException>()
           .WithMessage("*@odata.type*");
    }
}
