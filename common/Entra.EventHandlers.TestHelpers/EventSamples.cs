namespace Entra.EventHandlers.TestHelpers;

public static class EventSamples
{
    public static string AttributeCollectionStart(string odataType = "microsoft.graph.onAttributeCollectionStartCalloutData") =>
        $$"""
        {
          "type": "microsoft.graph.authenticationEvent.attributeCollectionStart",
          "source": "/tenants/00000000-0000-0000-0000-000000000000/applications/00000000-0000-0000-0000-000000000000",
          "data": {
            "@odata.type": "{{odataType}}",
            "tenantId": "00000000-0000-0000-0000-000000000000",
            "authenticationEventListenerId": "00000000-0000-0000-0000-000000000000",
            "customAuthenticationExtensionId": "00000000-0000-0000-0000-000000000000",
            "authenticationContext": {
              "correlationId": "00000000-0000-0000-0000-000000000000"
            }
          }
        }
        """;

    public static string AttributeCollectionSubmit(string odataType = "microsoft.graph.onAttributeCollectionSubmitCalloutData") =>
        $$"""
        {
          "type": "microsoft.graph.authenticationEvent.attributeCollectionSubmit",
          "source": "/tenants/00000000-0000-0000-0000-000000000000/applications/00000000-0000-0000-0000-000000000000",
          "data": {
            "@odata.type": "{{odataType}}",
            "tenantId": "00000000-0000-0000-0000-000000000000",
            "authenticationEventListenerId": "00000000-0000-0000-0000-000000000000",
            "customAuthenticationExtensionId": "00000000-0000-0000-0000-000000000000",
            "authenticationContext": {
              "correlationId": "00000000-0000-0000-0000-000000000000"
            },
            "userSignUpInfo": {}
          }
        }
        """;
    public static string TokenIssuanceStart(string odataType = "microsoft.graph.onTokenIssuanceStartCalloutData") =>
       $$"""
       {
         "type": "microsoft.graph.authenticationEvent.tokenIssuanceStart",
         "source": "/tenants/00000000-0000-0000-0000-000000000000/applications/00000000-0000-0000-0000-000000000000",
         "data": {
           "@odata.type": "{{odataType}}",
           "tenantId": "00000000-0000-0000-0000-000000000000",
           "authenticationEventListenerId": "00000000-0000-0000-0000-000000000000",
           "customAuthenticationExtensionId": "00000000-0000-0000-0000-000000000000",
           "authenticationContext": {
             "correlationId": "00000000-0000-0000-0000-000000000000"
           }
         }
       }
       """;

    public static string EmailOtpSend(string odataType = "microsoft.graph.onOtpSendCalloutData") =>
        $$"""
        {
          "type": "microsoft.graph.authenticationEvent.emailOtpSend",
          "source": "/tenants/00000000-0000-0000-0000-000000000000/applications/00000000-0000-0000-0000-000000000000",
          "data": {
            "@odata.type": "{{odataType}}",
            "otpContext": {
              "identifier": "someone@example.com",
              "oneTimeCode": "123456"
            },
            "tenantId": "00000000-0000-0000-0000-000000000000",
            "authenticationEventListenerId": "00000000-0000-0000-0000-000000000000",
            "customAuthenticationExtensionId": "00000000-0000-0000-0000-000000000000",
            "authenticationContext": {
              "correlationId": "00000000-0000-0000-0000-000000000000"
            }
          }
        }
        """;

    public static string PasswordSubmit(string odataType = "microsoft.graph.onPasswordSubmitCalloutData") =>
        $$"""
        {
          "type": "microsoft.graph.authenticationEvent.passwordSubmit",
          "source": "/tenants/00000000-0000-0000-0000-000000000000/applications/00000000-0000-0000-0000-000000000000",
          "data": {
            "@odata.type": "{{odataType}}",
            "tenantId": "00000000-0000-0000-0000-000000000000",
            "authenticationEventListenerId": "00000000-0000-0000-0000-000000000000",
            "customAuthenticationExtensionId": "00000000-0000-0000-0000-000000000000",
            "encryptedPasswordContext": "dummy-jwe",
            "authenticationContext": {
              "correlationId": "00000000-0000-0000-0000-000000000000"
            }
          }
        }
    """;
}
