# Summary
<details open><summary>Summary</summary>

|||
|:---|:---|
| Generated on: | 06/15/2026 - 21:07:53 |
| Coverage date: | 06/15/2026 - 21:07:23 - 06/15/2026 - 21:07:51 |
| Parser: | MultiReport (5x Cobertura) |
| Assemblies: | 5 |
| Classes: | 84 |
| Files: | 79 |
| **Line coverage:** | 74.9% (583 of 778) |
| Covered lines: | 583 |
| Uncovered lines: | 195 |
| Coverable lines: | 778 |
| Total lines: | 3462 |
| **Branch coverage:** | 49.3% (75 of 152) |
| Covered branches: | 75 |
| Total branches: | 152 |
| **Method coverage:** | [Feature is only available for sponsors](https://reportgenerator.io/pro) |

</details>

## Coverage
<details><summary>Entra.EventHandlers - 100%</summary>

|**Name**|**Line**|**Branch**|
|:---|---:|---:|
|**Entra.EventHandlers**|**100%**|**46.4%**|
|Entra.EventHandlers.Builders.ActionBuilders.PrefillValuesBuilder|100%||
|Entra.EventHandlers.Builders.EntraEventResponses|100%||
|Entra.EventHandlers.Builders.ResponseBuilders.AttributeCollectionStartRespo<br/>nseBuilder|100%|100%|
|Entra.EventHandlers.Builders.ResponseBuilders.AttributeCollectionSubmitResp<br/>onseBuilder|100%|100%|
|Entra.EventHandlers.Builders.ResponseBuilders.EmailOtpSendResponseBuilder|100%|100%|
|Entra.EventHandlers.Builders.ResponseBuilders.PasswordSubmitResponseBuilder|100%|100%|
|Entra.EventHandlers.Builders.ResponseBuilders.TokenIssuanceStartResponseBui<br/>lder|100%|100%|
|Entra.EventHandlers.Handlers.Base.AttributeCollectionStartHandlerBase|100%|25%|
|Entra.EventHandlers.Handlers.Base.AttributeCollectionSubmitHandlerBase|100%|25%|
|Entra.EventHandlers.Handlers.Base.EmailOtpSendHandlerBase|100%|25%|
|Entra.EventHandlers.Handlers.Base.PasswordSubmitHandlerBase|100%|50%|
|Entra.EventHandlers.Handlers.Base.TokenIssuanceStartHandlerBase|100%|25%|
|Entra.EventHandlers.Protocol.PasswordSubmit.DecryptedPasswordContext|100%||

</details>
<details><summary>Entra.EventHandlers.Abstractions - 100%</summary>

|**Name**|**Line**|**Branch**|
|:---|---:|---:|
|**Entra.EventHandlers.Abstractions**|**100%**|**100%**|
|Entra.EventHandlers.Abstractions.Actions.ContinueAction|100%||
|Entra.EventHandlers.Abstractions.Actions.ModifyAttributeValuesAction|100%||
|Entra.EventHandlers.Abstractions.Actions.PasswordSubmitAction|100%||
|Entra.EventHandlers.Abstractions.Actions.ProvideClaimsForTokenAction|100%||
|Entra.EventHandlers.Abstractions.Actions.SetPrefillValuesAction|100%||
|Entra.EventHandlers.Abstractions.Actions.ShowBlockPageAction|100%||
|Entra.EventHandlers.Abstractions.Actions.ShowValidationErrorAction|100%||
|Entra.EventHandlers.Abstractions.Actions.Types.ContinueActionType|100%||
|Entra.EventHandlers.Abstractions.Actions.Types.PasswordSubmitActionType|100%||
|Entra.EventHandlers.Abstractions.Actions.Types.ShowBlockPageActionType|100%||
|Entra.EventHandlers.Abstractions.Errors.EntraDeserializationException|100%||
|Entra.EventHandlers.Abstractions.Errors.EntraErrorResponse|100%||
|Entra.EventHandlers.Abstractions.Errors.EntraHandlerNotFoundException|100%||
|Entra.EventHandlers.Abstractions.Errors.EntraValidationException|100%||
|Entra.EventHandlers.Abstractions.Events.AttributeCollectionStartEvent|100%||
|Entra.EventHandlers.Abstractions.Events.AttributeCollectionStartEventPayloa<br/>d|100%||
|Entra.EventHandlers.Abstractions.Events.AttributeCollectionSubmitEvent|100%||
|Entra.EventHandlers.Abstractions.Events.AttributeCollectionSubmitEventPaylo<br/>ad|100%||
|Entra.EventHandlers.Abstractions.Events.EmailOtpSendEvent|100%||
|Entra.EventHandlers.Abstractions.Events.EmailOtpSendEventPayload|100%||
|Entra.EventHandlers.Abstractions.Events.EntraEvent`1|100%||
|Entra.EventHandlers.Abstractions.Events.EntraEventPayload|100%|100%|
|Entra.EventHandlers.Abstractions.Events.PasswordSubmitEvent|100%||
|Entra.EventHandlers.Abstractions.Events.PasswordSubmitEventPayload|100%||
|Entra.EventHandlers.Abstractions.Events.TokenIssuanceStartEvent|100%||
|Entra.EventHandlers.Abstractions.Events.TokenIssuanceStartEventPayload|100%||
|Entra.EventHandlers.Abstractions.Protocol.Authentication.AuthenticationCont<br/>ext|100%||
|Entra.EventHandlers.Abstractions.Protocol.Authentication.ClientInfo|100%||
|Entra.EventHandlers.Abstractions.Protocol.Authentication.ServicePrincipalIn<br/>fo|100%||
|Entra.EventHandlers.Abstractions.Protocol.Authentication.UserInfo|100%||
|Entra.EventHandlers.Abstractions.Protocol.Otp.OtpContext|100%||
|Entra.EventHandlers.Abstractions.Protocol.SignUp.DirectoryAttributeValue|100%||
|Entra.EventHandlers.Abstractions.Protocol.SignUp.IdentityInfo|100%||
|Entra.EventHandlers.Abstractions.Protocol.SignUp.UserSignUpInfo|100%||
|Entra.EventHandlers.Abstractions.Responses.AttributeCollectionStartResponse<br/>Payload|100%||
|Entra.EventHandlers.Abstractions.Responses.AttributeCollectionSubmitRespons<br/>ePayload|100%||
|Entra.EventHandlers.Abstractions.Responses.EmailOtpSendResponsePayload|100%||
|Entra.EventHandlers.Abstractions.Responses.EntraEventResponse`1|100%||
|Entra.EventHandlers.Abstractions.Responses.EntraEventResponsePayload|100%||
|Entra.EventHandlers.Abstractions.Responses.PasswordSubmitResponsePayload|100%||
|Entra.EventHandlers.Abstractions.Responses.TokenIssuanceStartResponsePayloa<br/>d|100%||

</details>
<details><summary>Entra.EventHandlers.AspNetCore - 32.7%</summary>

|**Name**|**Line**|**Branch**|
|:---|---:|---:|
|**Entra.EventHandlers.AspNetCore**|**32.7%**|**13.8%**|
|Entra.EventHandlers.AspNetCore.Abstractions.EntraEndpointBase|54%|0%|
|Entra.EventHandlers.AspNetCore.Adapters.RequestAdapter|0%|0%|
|Entra.EventHandlers.AspNetCore.Adapters.ResponseAdapter|0%||
|Entra.EventHandlers.AspNetCore.Base.AttributeCollectionStartEndpointBase|28.5%||
|Entra.EventHandlers.AspNetCore.Base.AttributeCollectionSubmitEndpointBase|28.5%||
|Entra.EventHandlers.AspNetCore.Base.EmailOtpSendEndpointBase|28.5%||
|Entra.EventHandlers.AspNetCore.Base.PasswordSubmitEndpointBase|28.5%||
|Entra.EventHandlers.AspNetCore.Base.TokenIssuanceStartEndpointBase|28.5%||
|Entra.EventHandlers.AspNetCore.DI.ServiceCollectionExtensions|100%||
|Entra.EventHandlers.AspNetCore.Endpoints.AttributeCollectionStartEndpoint|25%||
|Entra.EventHandlers.AspNetCore.Endpoints.AttributeCollectionSubmitEndpoint|25%||
|Entra.EventHandlers.AspNetCore.Endpoints.EmailOtpSendEndpoint|25%||
|Entra.EventHandlers.AspNetCore.Endpoints.EntraEventRouterEndpoint|25%||
|Entra.EventHandlers.AspNetCore.Endpoints.PasswordSubmitEndpoint|25%||
|Entra.EventHandlers.AspNetCore.Endpoints.TokenIssuanceStartEndpoint|25%||
|Entra.EventHandlers.AspNetCore.Extensions.EntraEndpointMappingExtensions|0%||
|Entra.EventHandlers.AspNetCore.Routing.EntraEventRouterEndpointBase|80%|16.6%|

</details>
<details><summary>Entra.EventHandlers.AzureFunctions - 40.8%</summary>

|**Name**|**Line**|**Branch**|
|:---|---:|---:|
|**Entra.EventHandlers.AzureFunctions**|**40.8%**|**68.4%**|
|Entra.EventHandlers.AzureFunctions.Abstractions.EntraFunctionBase|82.3%|0%|
|Entra.EventHandlers.AzureFunctions.Adapters.RequestAdapter|0%|0%|
|Entra.EventHandlers.AzureFunctions.Adapters.ResponseAdapter|0%||
|Entra.EventHandlers.AzureFunctions.Base.AttributeCollectionStartFunctionBas<br/>e|0%||
|Entra.EventHandlers.AzureFunctions.Base.AttributeCollectionSubmitFunctionBa<br/>se|0%||
|Entra.EventHandlers.AzureFunctions.Base.EmailOtpSendFunctionBase|0%||
|Entra.EventHandlers.AzureFunctions.Base.PasswordSubmitFunctionBase|0%||
|Entra.EventHandlers.AzureFunctions.Base.TokenIssuanceStartFunctionBase|0%||
|Entra.EventHandlers.AzureFunctions.DI.ServiceCollectionExtensions|100%||
|Entra.EventHandlers.AzureFunctions.Routing.EntraEventRouterFunctionBase|100%|81.2%|

</details>
<details><summary>Entra.EventHandlers.Hosting - 96.5%</summary>

|**Name**|**Line**|**Branch**|
|:---|---:|---:|
|**Entra.EventHandlers.Hosting**|**96.5%**|**80%**|
|Entra.EventHandlers.Hosting.DI.ServiceCollectionExtensions|100%||
|Entra.EventHandlers.Hosting.Extensions.ExceptionExtensions|90%|91.6%|
|Entra.EventHandlers.Hosting.Resolvers.EntraEventHandlerResolver|100%|62.5%|

</details>
