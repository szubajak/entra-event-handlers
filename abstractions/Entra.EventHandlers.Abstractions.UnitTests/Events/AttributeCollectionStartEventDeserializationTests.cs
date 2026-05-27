using AutoFixture;
using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Protocol;
using Entra.EventHandlers.Abstractions.Protocol.Authentication;
using Entra.EventHandlers.Abstractions.Protocol.SignUp;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Text.Json;

namespace Entra.EventHandlers.Abstractions.UnitTests.Events;

public class AttributeCollectionStartEventDeserializationTests
{
    [Fact]
    public void Deserializes_FullEventRequest_Correctly()
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

        var givenName = fixture.Create<string>();
        var companyName = fixture.Create<string>();

        var (attr1, val1) = (fixture.Create<string>(), fixture.Create<string>());
        var (attr2, val2) = (fixture.Create<string>(), fixture.Create<Int64>());
        var (attr3, val3) = (fixture.Create<string>(), fixture.Create<Boolean>());

        var signInType = "email";
        var issuer = "example.onmicrosoft.com";
        var issuerAssignedId = $"{fixture.Create<string>()}@{issuer}";

        var json =
        $$"""
        {
          "type": "microsoft.graph.authenticationEvent.attributeCollectionStart",
          "source": "/tenants/{{tenantId}}/applications/{{appId}}",
          "data": {
            "@odata.type": "microsoft.graph.onAttributeCollectionStartCalloutData",
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
            },
            "userSignUpInfo": {
              "attributes": {
                "givenName": {
                  "@odata.type": "microsoft.graph.stringDirectoryAttributeValue",
                  "value": "{{givenName}}",
                  "attributeType": "builtIn"
                },
                "companyName": {
                  "@odata.type": "microsoft.graph.stringDirectoryAttributeValue",
                  "value": "{{companyName}}",
                  "attributeType": "builtIn"
                },
                "{{attr1}}": {
                  "@odata.type": "microsoft.graph.stringDirectoryAttributeValue",
                  "value": "{{val1}}",
                  "attributeType": "directorySchemaExtension"
                },
                "{{attr2}}": {
                  "@odata.type": "microsoft.graph.int64DirectoryAttributeValue",
                  "value": {{val2}},
                  "attributeType": "directorySchemaExtension"
                },
                "{{attr3}}": {
                  "@odata.type": "microsoft.graph.booleanDirectoryAttributeValue",
                  "value": {{val3.ToString().ToLowerInvariant()}},
                  "attributeType": "directorySchemaExtension"
                }
              },
              "identities": [
                {
                  "signInType": "{{signInType}}",
                  "issuer": "{{issuer}}",
                  "issuerAssignedId": "{{issuerAssignedId}}"
                }
              ]
            }
          }
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<EntraEvent>(json);

        // Assert
        using (new AssertionScope())
        {
            var evt = result.Should().BeOfType<AttributeCollectionStartEvent>().Which;
            evt.Validate();

            evt.Type.Should().Be(EntraEventTypes.AttributeCollectionStart);
            evt.Source.Should().Be($"/tenants/{tenantId}/applications/{appId}");

            var payload = evt.Data;
            payload.Should().NotBeNull();
            payload.OdataType.Should().Be(EntraOdataTypes.AttributeCollectionStart.CalloutData);
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

            var signUp = payload.UserSignUpInfo;
            signUp.Should().NotBeNull();
            signUp.Identities.Should()
                .NotBeNullOrEmpty()
                .And.HaveCount(1)
                .And.Subject.Single().Should().Match<IdentityInfo>(x =>
                    x.SignInType == signInType &&
                    x.Issuer == issuer &&
                    x.IssuerAssignedId == issuerAssignedId
                );

            var attributes = signUp.Attributes;
            attributes.Should().NotBeNullOrEmpty();
            attributes.Should().ContainKey("givenName")
                .WhoseValue.Should().Match<DirectoryAttributeValue>(x =>
                    x.OdataType == EntraOdataTypes.DirectoryAttributes.String &&
                    x.Value.As<JsonElement>().GetString() == givenName &&
                    x.AttributeType == DirectoryAttributeTypes.BuiltIn
                );
            attributes.Should().ContainKey("companyName")
                .WhoseValue.Should().Match<DirectoryAttributeValue>(x =>
                    x.OdataType == EntraOdataTypes.DirectoryAttributes.String &&
                    x.Value.As<JsonElement>().GetString() == companyName &&
                    x.AttributeType == DirectoryAttributeTypes.BuiltIn
                );
            attributes.Should().ContainKey(attr1)
                .WhoseValue.Should().Match<DirectoryAttributeValue>(x =>
                    x.OdataType == EntraOdataTypes.DirectoryAttributes.String &&
                    x.Value.As<JsonElement>().GetString() == val1 &&
                    x.AttributeType == DirectoryAttributeTypes.DirectorySchemaExtension
                );
            attributes.Should().ContainKey(attr2)
                .WhoseValue.Should().Match<DirectoryAttributeValue>(x =>
                    x.OdataType == EntraOdataTypes.DirectoryAttributes.Int64 &&
                    x.Value.As<JsonElement>().GetInt64() == val2 &&
                    x.AttributeType == DirectoryAttributeTypes.DirectorySchemaExtension
                );
            attributes.Should().ContainKey(attr3)
                .WhoseValue.Should().Match<DirectoryAttributeValue>(x =>
                    x.OdataType == EntraOdataTypes.DirectoryAttributes.Boolean &&
                    x.Value.As<JsonElement>().GetBoolean() == val3 &&
                    x.AttributeType == DirectoryAttributeTypes.DirectorySchemaExtension
                );

            evt.CorrelationId.Should().Be(payload.AuthenticationContext.CorrelationId);
        }
    }

    [Fact]
    public void Deserializes_MinimalEventRequest_Correctly()
    {
        // Arrange
        var fixture = new Fixture();

        var tenantId = fixture.Create<Guid>();
        var appId = fixture.Create<Guid>();
        var listenerId = fixture.Create<Guid>();
        var extensionId = fixture.Create<Guid>();
        var correlationId = fixture.Create<Guid>();

        var json =
        $$"""
        {
          "type": "microsoft.graph.authenticationEvent.attributeCollectionStart",
          "source": "/tenants/{{tenantId}}/applications/{{appId}}",
          "data": {
            "@odata.type": "microsoft.graph.onAttributeCollectionStartCalloutData",
            "tenantId": "{{tenantId}}",
            "authenticationEventListenerId": "{{listenerId}}",
            "customAuthenticationExtensionId": "{{extensionId}}",
            "authenticationContext": {
              "correlationId": "{{correlationId}}"
            }
          }
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<EntraEvent>(json);

        // Assert
        using (new AssertionScope())
        {
            var evt = result.Should().BeOfType<AttributeCollectionStartEvent>().Which;
            evt.Validate();

            evt.Type.Should().Be(EntraEventTypes.AttributeCollectionStart);
            evt.Source.Should().Be($"/tenants/{tenantId}/applications/{appId}");

            var payload = evt.Data;
            payload.Should().NotBeNull();
            payload.OdataType.Should().Be(EntraOdataTypes.AttributeCollectionStart.CalloutData);
            payload.TenantId.Should().Be(tenantId);
            payload.AuthenticationEventListenerId.Should().Be(listenerId);
            payload.CustomAuthenticationExtensionId.Should().Be(extensionId);

            var ctx = payload.AuthenticationContext;
            ctx.Should().NotBeNull();
            ctx.CorrelationId.Should().Be(correlationId);

            evt.CorrelationId.Should().Be(payload.AuthenticationContext.CorrelationId);
        }
    }
}
