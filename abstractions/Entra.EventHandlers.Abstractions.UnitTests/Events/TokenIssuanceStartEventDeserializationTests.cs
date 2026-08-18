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

public class TokenIssuanceStartEventDeserializationTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void FullEventRequest_DeserializesCorrectly()
    {
        // Arrange
        var tenantId = _fixture.Create<Guid>();
        var appId = _fixture.Create<Guid>();
        var listenerId = _fixture.Create<Guid>();
        var extensionId = _fixture.Create<Guid>();
        var correlationId = _fixture.Create<Guid>();

        var clientIp = _fixture.Create<string>();
        var clientLocale = "en-gb";
        var clientMarket = "en-us";

        var cspId = _fixture.Create<Guid>();
        var cspAppDisplayName = _fixture.Create<string>();
        var cspDisplayName = _fixture.Create<string>();

        var rspId = _fixture.Create<Guid>();
        var rspAppDisplayName = _fixture.Create<string>();
        var rspDisplayName = _fixture.Create<string>();

        var userCompanyName = _fixture.Create<string>();
        var userCreatedDateTime = _fixture.Create<DateTime>().ToString("o");
        var userDisplayName = _fixture.Create<string>();
        var userGivenName = _fixture.Create<string>();
        var userId = _fixture.Create<Guid>();
        var userMail = $"{_fixture.Create<string>()}@example.com";
        var userPsam = _fixture.Create<string>();
        var userPsid = _fixture.Create<string>();
        var userPupn = _fixture.Create<string>();
        var userPreferredLanguage = "en-us";
        var userSurname = _fixture.Create<string>();
        var userPrincipalName = $"{_fixture.Create<string>()}@example.com";

        var json =
        $$"""
        {
          "type": "microsoft.graph.authenticationEvent.tokenIssuanceStart",
          "source": "/tenants/{{tenantId}}/applications/{{appId}}",
          "data": {
            "@odata.type": "microsoft.graph.onTokenIssuanceStartCalloutData",
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
              },
              "user": {
                "companyName": "{{userCompanyName}}",
                "createdDateTime": "{{userCreatedDateTime}}",
                "displayName": "{{userDisplayName}}",
                "givenName": "{{userGivenName}}",
                "id": "{{userId}}",
                "mail": "{{userMail}}",
                "onPremisesSamAccountName": "{{userPsam}}",
                "onPremisesSecurityIdentifier": "{{userPsid}}",
                "onPremisesUserPrincipalName": "{{userPupn}}",
                "preferredLanguage": "{{userPreferredLanguage}}",
                "surname": "{{userSurname}}",
                "userPrincipalName": "{{userPrincipalName}}",
                "userType": "Member"
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
            var evt = result.Should().BeOfType<TokenIssuanceStartEvent>().Which;
            evt.Validate();

            evt.Type.Should().Be(EntraEventTypes.TokenIssuanceStart);
            evt.Source.Should().Be($"/tenants/{tenantId}/applications/{appId}");

            var payload = evt.Data;
            payload.Should().NotBeNull();
            payload.OdataType.Should().Be(EntraOdataTypes.TokenIssuanceStart.CalloutData);
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
            ctx.User.Should().NotBeNull()
                .And.Subject.Should().Match<UserInfo>(x =>
                    x.CompanyName == userCompanyName &&
                    x.CreatedDateTime == DateTime.Parse(userCreatedDateTime) &&
                    x.DiplayName == userDisplayName &&
                    x.GivenName == userGivenName &&
                    x.Id == userId &&
                    x.Mail == userMail &&
                    x.OnPremisesSamAccountName == userPsam &&
                    x.OnPremisesSecurityIdentifier == userPsid &&
                    x.OnPremisesUserPrincipalName == userPupn &&
                    x.PreferredLanguage == userPreferredLanguage &&
                    x.Surname == userSurname &&
                    x.UserPrincipalName == userPrincipalName &&
                    x.UserType == "Member"
                );

            evt.CorrelationId.Should().Be(payload.AuthenticationContext.CorrelationId);
        }
    }

    [Fact]
    public void ExternalUser_DeserializesCorrectly()
    {
        // Arrange
        var userCompanyName = _fixture.Create<string>();
        var userCreatedDateTime = _fixture.Create<DateTime>().ToString("o");
        var userDisplayName = _fixture.Create<string>();
        var userId = _fixture.Create<Guid>();
        var userMail = $"{_fixture.Create<string>()}@example.com";
        var userPreferredDataLocation = _fixture.Create<string>();
        var userPrincipalName = $"{_fixture.Create<string>()}#EXT#@example.onmicrosoft.com";

        var json =
        $$"""
        {
          "companyName": "{{userCompanyName}}",
          "createdDateTime": "{{userCreatedDateTime}}",
          "displayName": "{{userDisplayName}}",
          "id": "{{userId}}",
          "mail": "{{userMail}}",
          "preferredDataLocation": "{{userPreferredDataLocation}}",
          "userPrincipalName": "{{userPrincipalName}}",
          "userType": "Guest"
        }
        """;

        // Act
        var user = JsonSerializer.Deserialize<UserInfo>(json);

        // Assert
        using (new AssertionScope())
        {
            user.Should().NotBeNull();
            user.CompanyName.Should().Be(userCompanyName);
            user.CreatedDateTime.Should().Be(DateTime.Parse(userCreatedDateTime));
            user.DiplayName.Should().Be(userDisplayName);
            user.Id.Should().Be(userId);
            user.Mail.Should().Be(userMail);
            user.PreferredDataLocation.Should().Be(userPreferredDataLocation);
            user.UserPrincipalName.Should().Be(userPrincipalName);
        }
    }

    [Fact]
    public void MinimalEventRequest_DeserializesCorrectly()
    {
        // Act
        var result = JsonSerializer.Deserialize<EntraEvent>(EventSamples.TokenIssuanceStart());

        // Assert
        using (new AssertionScope())
        {
            var evt = result.Should().BeOfType<TokenIssuanceStartEvent>().Which;
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
        // Arrange
        var evt = JsonSerializer.Deserialize<EntraEvent>(EventSamples.TokenIssuanceStart(odataType: "invalid"));

        // Act
        Action act = () => ((TokenIssuanceStartEvent)evt!).Validate();

        // Assert
        act.Should().Throw<EntraValidationException>()
           .WithMessage("*@odata.type*");
    }
}
