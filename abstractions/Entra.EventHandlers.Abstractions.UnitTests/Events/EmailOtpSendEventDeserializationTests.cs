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

public class EmailOtpSendEventDeserializationTests
{
    [Fact]
    public void FullEventRequest_DeserializesCorrectly()
    {
        // Arrange
        var fixture = new Fixture();

        var tenantId = fixture.Create<Guid>();
        var appId = fixture.Create<Guid>();
        var listenerId = fixture.Create<Guid>();
        var extensionId = fixture.Create<Guid>();
        var correlationId = fixture.Create<Guid>();

        var clientIp = fixture.Create<string>();
        var clientLocale = "en-gb";
        var clientMarket = "en-us";

        var cspId = fixture.Create<Guid>();
        var cspAppDisplayName = fixture.Create<string>();
        var cspDisplayName = fixture.Create<string>();

        var rspId = fixture.Create<Guid>();
        var rspAppDisplayName = fixture.Create<string>();
        var rspDisplayName = fixture.Create<string>();

        var identifier = "someone@example.com";
        var otp = fixture.Create<string>();

        var json =
        $$"""
        {
          "type": "microsoft.graph.authenticationEvent.emailOtpSend",
          "source": "/tenants/{{tenantId}}/applications/{{appId}}",
          "data": {
            "@odata.type": "microsoft.graph.onOtpSendCalloutData",
            "otpContext": {
                "identifier": "{{identifier}}",
                "oneTimeCode": "{{otp}}"
            },
            "tenantId": "{{tenantId}}",
            "authenticationEventListenerId": "{{listenerId}}",
            "customAuthenticationExtensionId": "{{extensionId}}",
            "authenticationContext": {
              "correlationId": "{{correlationId}}",
              "client": {
                "ip": "{{clientIp}}",
                "locale": "{{clientLocale}}",
                "market": "{{clientMarket}}"
              },
              "protocol": "OAUTH2.0",
              "clientServicePrincipal": {
                "id": "{{cspId}}",
                "appId": "{{appId}}",
                "appDisplayName": "{{cspAppDisplayName}}",
                "displayName": "{{cspDisplayName}}"
              },
              "resourceServicePrincipal": {
                "id": "{{rspId}}",
                "appId": "{{appId}}",
                "appDisplayName": "{{rspAppDisplayName}}",
                "displayName": "{{rspDisplayName}}"
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
            var evt = result.Should().BeOfType<EmailOtpSendEvent>().Which;
            evt.Validate();

            evt.Type.Should().Be(EntraEventTypes.EmailOtpSend);
            evt.Source.Should().Be($"/tenants/{tenantId}/applications/{appId}");

            var payload = evt.Data;
            payload.Should().NotBeNull();
            payload.OdataType.Should().Be(EntraOdataTypes.EmailOtpSend.CalloutData);
            payload.TenantId.Should().Be(tenantId);
            payload.AuthenticationEventListenerId.Should().Be(listenerId);
            payload.CustomAuthenticationExtensionId.Should().Be(extensionId);

            var ctx = payload.AuthenticationContext;
            ctx.Should().NotBeNull();
            ctx.CorrelationId.Should().Be(correlationId);
            ctx.Protocol.Should().Be("OAUTH2.0");
            ctx.Client.Should().NotBeNull()
                .And.Subject.Should().Match<ClientInfo>(x =>
                    x.Ip == clientIp &&
                    x.Locale == clientLocale &&
                    x.Market == clientMarket
                );
            ctx.ClientServicePrincipal.Should().NotBeNull()
                .And.Subject.Should().Match<ServicePrincipalInfo>(sp =>
                    sp.Id == cspId &&
                    sp.AppId == appId &&
                    sp.AppDisplayName == cspAppDisplayName &&
                    sp.DisplayName == cspDisplayName
                );
            ctx.ResourceServicePrincipal.Should().NotBeNull()
               .And.Subject.Should().Match<ServicePrincipalInfo>(x =>
                   x.Id == rspId &&
                   x.AppId == appId &&
                   x.AppDisplayName == rspAppDisplayName &&
                   x.DisplayName == rspDisplayName
               );

            var otpContext = payload.OtpContext;
            otpContext.Should().NotBeNull();
            otpContext.Identifier.Should().Be(identifier);
            otpContext.OneTimeCode.Should().Be(otp);

            evt.CorrelationId.Should().Be(payload.AuthenticationContext.CorrelationId);
        }
    }

    [Fact]
    public void MinimalEventRequest_DeserializesCorrectly()
    {
        // Act
        var result = JsonSerializer.Deserialize<EntraEvent>(EventSamples.EmailOtpSend());

        // Assert
        using (new AssertionScope())
        {
            var evt = result.Should().BeOfType<EmailOtpSendEvent>().Which;
            evt.Validate();

            var ctx = evt.Data.AuthenticationContext;
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
        var evt = JsonSerializer.Deserialize<EntraEvent>(EventSamples.EmailOtpSend(odataType: "invalid"));

        // Act
        Action act = () => ((EmailOtpSendEvent)evt!).Validate();

        // Assert
        act.Should().Throw<EntraValidationException>()
           .WithMessage("*@odata.type*");
    }
}
