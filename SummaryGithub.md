# Summary
<details open><summary>Summary</summary>

|||
|:---|:---|
| Generated on: | 09/25/2026 - 07:10:12 |
| Parser: | MultiReport (10x Cobertura) |
| Assemblies: | 8 |
| Classes: | 104 |
| Files: | 93 |
| **Line coverage:** | 85% (1084 of 1274) |
| Covered lines: | 1084 |
| Uncovered lines: | 190 |
| Coverable lines: | 1274 |
| Total lines: | 4143 |
| **Branch coverage:** | 95.7% (157 of 164) |
| Covered branches: | 157 |
| Total branches: | 164 |
| **Method coverage:** | [Feature is only available for sponsors](https://reportgenerator.io/pro) |

</details>

## Coverage
<details><summary>Entra.EventHandlers - 90.2%</summary>

|**Name**|**Line**|**Branch**|
|:---|---:|---:|
|**Entra.EventHandlers**|**90.2%**|**100%**|
|Entra.EventHandlers.Builders.ActionBuilders.PrefillValuesBuilder|100%||
|Entra.EventHandlers.Builders.EntraEventResponses|100%||
|Entra.EventHandlers.Builders.ResponseBuilders.AttributeCollectionStartRespo<br/>nseBuilder|100%|100%|
|Entra.EventHandlers.Builders.ResponseBuilders.AttributeCollectionSubmitResp<br/>onseBuilder|100%|100%|
|Entra.EventHandlers.Builders.ResponseBuilders.EmailOtpSendResponseBuilder|100%|100%|
|Entra.EventHandlers.Builders.ResponseBuilders.PasswordSubmitResponseBuilder|100%|100%|
|Entra.EventHandlers.Builders.ResponseBuilders.TokenIssuanceStartResponseBui<br/>lder|100%|100%|
|Entra.EventHandlers.Handlers.Base.AttributeCollectionStartHandlerBase|84%|100%|
|Entra.EventHandlers.Handlers.Base.AttributeCollectionSubmitHandlerBase|84%|100%|
|Entra.EventHandlers.Handlers.Base.EmailOtpSendHandlerBase|84%|100%|
|Entra.EventHandlers.Handlers.Base.PasswordSubmitHandlerBase|85.9%|100%|
|Entra.EventHandlers.Handlers.Base.TokenIssuanceStartHandlerBase|84%|100%|

</details>
<details><summary>Entra.EventHandlers.Abstractions - 96.5%</summary>

|**Name**|**Line**|**Branch**|
|:---|---:|---:|
|**Entra.EventHandlers.Abstractions**|**96.5%**|**100%**|
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
|Entra.EventHandlers.Abstractions.Actions.VerifiedIdClaimValidationFailedAct<br/>ion|100%||
|Entra.EventHandlers.Abstractions.Actions.VerifiedIdClaimValidationPassActio<br/>n|100%||
|Entra.EventHandlers.Abstractions.Errors.EntraDeserializationException|100%||
|Entra.EventHandlers.Abstractions.Errors.EntraException|100%||
|Entra.EventHandlers.Abstractions.Errors.EntraHandlerNotFoundException|100%||
|Entra.EventHandlers.Abstractions.Errors.EntraSecurityException|50%||
|Entra.EventHandlers.Abstractions.Errors.EntraValidationException|100%||
|Entra.EventHandlers.Abstractions.Events.AttributeCollectionStartEvent|100%||
|Entra.EventHandlers.Abstractions.Events.AttributeCollectionStartEventPayloa<br/>d|100%||
|Entra.EventHandlers.Abstractions.Events.AttributeCollectionSubmitEvent|100%||
|Entra.EventHandlers.Abstractions.Events.AttributeCollectionSubmitEventPaylo<br/>ad|100%||
|Entra.EventHandlers.Abstractions.Events.EmailOtpSendEvent|100%||
|Entra.EventHandlers.Abstractions.Events.EmailOtpSendEventPayload|100%||
|Entra.EventHandlers.Abstractions.Events.EntraEvent<TPayload>|100%||
|Entra.EventHandlers.Abstractions.Events.EntraEventPayload|100%|100%|
|Entra.EventHandlers.Abstractions.Events.PasswordSubmitEvent|100%||
|Entra.EventHandlers.Abstractions.Events.PasswordSubmitEventPayload|100%||
|Entra.EventHandlers.Abstractions.Events.TokenIssuanceStartEvent|100%||
|Entra.EventHandlers.Abstractions.Events.TokenIssuanceStartEventPayload|100%||
|Entra.EventHandlers.Abstractions.Events.VerifiedIdClaimValidationEvent|100%||
|Entra.EventHandlers.Abstractions.Events.VerifiedIdClaimValidationEventPaylo<br/>ad|100%||
|Entra.EventHandlers.Abstractions.Extensions.ExceptionExtensions|100%|100%|
|Entra.EventHandlers.Abstractions.Responses.AttributeCollectionStartResponse<br/>Payload|100%||
|Entra.EventHandlers.Abstractions.Responses.AttributeCollectionSubmitRespons<br/>ePayload|100%||
|Entra.EventHandlers.Abstractions.Responses.EmailOtpSendResponsePayload|100%||
|Entra.EventHandlers.Abstractions.Responses.EntraEventResponsePayload|100%||
|Entra.EventHandlers.Abstractions.Responses.PasswordSubmitResponsePayload|100%||
|Entra.EventHandlers.Abstractions.Responses.TokenIssuanceStartResponsePayloa<br/>d|100%||
|Entra.EventHandlers.Abstractions.Responses.VerifiedIdClaimValidationRespons<br/>ePayload|100%||
|Entra.EventHandlers.Abstractions.Results.EntraHandlerResult<TResponse>|0%||

</details>
<details><summary>Entra.EventHandlers.AspNetCore - 61.9%</summary>

|**Name**|**Line**|**Branch**|
|:---|---:|---:|
|**Entra.EventHandlers.AspNetCore**|**61.9%**|**100%**|
|Entra.EventHandlers.AspNetCore.Abstractions.EntraEndpointBase|75.9%|100%|
|Entra.EventHandlers.AspNetCore.Abstractions.EntraTypedEndpointBase<TEvent, <br/>TResponse>|100%|100%|
|Entra.EventHandlers.AspNetCore.Abstractions.EntraTypedEndpointBase<TEvent, <br/>TResponse>|100%||
|Entra.EventHandlers.AspNetCore.Adapters.RequestAdapter|86.3%|100%|
|Entra.EventHandlers.AspNetCore.Adapters.RequestAdapter<TEvent>|86.3%|100%|
|Entra.EventHandlers.AspNetCore.Adapters.ResponseAdapter|100%||
|Entra.EventHandlers.AspNetCore.DI.ServiceCollectionExtensions|100%|100%|
|Entra.EventHandlers.AspNetCore.Endpoints.AttributeCollectionStartEndpoint|0%||
|Entra.EventHandlers.AspNetCore.Endpoints.AttributeCollectionSubmitEndpoint|0%||
|Entra.EventHandlers.AspNetCore.Endpoints.EmailOtpSendEndpoint|0%||
|Entra.EventHandlers.AspNetCore.Endpoints.EntraEventRouterEndpoint|0%||
|Entra.EventHandlers.AspNetCore.Endpoints.PasswordSubmitEndpoint|0%||
|Entra.EventHandlers.AspNetCore.Endpoints.TokenIssuanceStartEndpoint|0%||
|Entra.EventHandlers.AspNetCore.Endpoints.VerifiedIdClaimValidationEndpoint|0%||
|Entra.EventHandlers.AspNetCore.Extensions.EntraEndpointMappingExtensions|0%||
|Entra.EventHandlers.AspNetCore.Routing.EntraEventRouterEndpointBase|100%|100%|

</details>
<details><summary>Entra.EventHandlers.AzureFunctions - 90.2%</summary>

|**Name**|**Line**|**Branch**|
|:---|---:|---:|
|**Entra.EventHandlers.AzureFunctions**|**90.2%**|**100%**|
|Entra.EventHandlers.AzureFunctions.Abstractions.EntraFunctionBase|73.3%||
|Entra.EventHandlers.AzureFunctions.Adapters.RequestAdapter|86.3%|100%|
|Entra.EventHandlers.AzureFunctions.Adapters.RequestAdapter<TEvent>|86.3%|100%|
|Entra.EventHandlers.AzureFunctions.Adapters.ResponseAdapter|100%||
|Entra.EventHandlers.AzureFunctions.Base.AttributeCollectionStartFunctionBas<br/>e|100%|100%|
|Entra.EventHandlers.AzureFunctions.Base.AttributeCollectionSubmitFunctionBa<br/>se|100%|100%|
|Entra.EventHandlers.AzureFunctions.Base.EmailOtpSendFunctionBase|100%|100%|
|Entra.EventHandlers.AzureFunctions.Base.PasswordSubmitFunctionBase|100%|100%|
|Entra.EventHandlers.AzureFunctions.Base.TokenIssuanceStartFunctionBase|100%|100%|
|Entra.EventHandlers.AzureFunctions.Base.VerifiedIdClaimValidationFunctionBa<br/>se|100%|100%|
|Entra.EventHandlers.AzureFunctions.DI.ServiceCollectionExtensions|100%||
|Entra.EventHandlers.AzureFunctions.Routing.EntraEventRouterFunctionBase|100%|100%|

</details>
<details><summary>Entra.EventHandlers.Hosting - 100%</summary>

|**Name**|**Line**|**Branch**|
|:---|---:|---:|
|**Entra.EventHandlers.Hosting**|**100%**|**100%**|
|Entra.EventHandlers.Hosting.DI.ServiceCollectionExtensions|100%|100%|
|Entra.EventHandlers.Hosting.Orchestrators.EntraEventOrchestrator|100%|100%|
|Entra.EventHandlers.Hosting.Orchestrators.EntraEventOrchestrator<TEvent, TR<br/>esponse>|100%|100%|
|Entra.EventHandlers.Hosting.Resolvers.EntraEventHandlerResolver|100%|100%|

</details>
<details><summary>Entra.EventHandlers.Observability - 95.2%</summary>

|**Name**|**Line**|**Branch**|
|:---|---:|---:|
|**Entra.EventHandlers.Observability**|**95.2%**|**100%**|
|Entra.EventHandlers.Observability.Clients.ObservabilityApiClient|28.5%||
|Entra.EventHandlers.Observability.Context.EventLogContext|100%||
|Entra.EventHandlers.Observability.Decorators.ObservabilityHandlerDecorator<<br/>TRequest, TResponse>|100%|100%|
|Entra.EventHandlers.Observability.Decorators.ObservabilityHandlerDecorator<<br/>TRequest, TResponse>|100%||
|Entra.EventHandlers.Observability.DI.ServiceCollectionExtenstions|100%|100%|
|Entra.EventHandlers.Observability.Factories.EventLogMapperFactory|100%||
|Entra.EventHandlers.Observability.Logging.EventLogPublisher|100%||
|Entra.EventHandlers.Observability.Logging.EventLogWriter|100%||
|Entra.EventHandlers.Observability.Mappers.EmailOtpSendEventLogMapper|100%||
|Entra.EventHandlers.Observability.Mappers.EventLogContextMapper|100%|100%|
|Entra.EventHandlers.Observability.Models.CustomLogEntry|100%||

</details>
<details><summary>Entra.EventHandlers.Security - 68.7%</summary>

|**Name**|**Line**|**Branch**|
|:---|---:|---:|
|**Entra.EventHandlers.Security**|**68.7%**|**61.1%**|
|Entra.EventHandlers.Security.Adapters.SecretClientAdapter|0%||
|Entra.EventHandlers.Security.Decryptors.KeyVaultPasswordContextDecryptor|70.9%|50%|
|Entra.EventHandlers.Security.Decryptors.PasswordContextPayload|50%|50%|
|Entra.EventHandlers.Security.DI.ServiceCollectionExtensions|0%|0%|
|Entra.EventHandlers.Security.Providers.KeyVaultCertificateProvider|96.2%|80%|

</details>
<details><summary>Entra.EventHandlers.Workforce - 90.8%</summary>

|**Name**|**Line**|**Branch**|
|:---|---:|---:|
|**Entra.EventHandlers.Workforce**|**90.8%**|**100%**|
|Entra.EventHandlers.Workforce.Builders.ActionBuilders.FailedClaimsBuilder|100%||
|Entra.EventHandlers.Workforce.Builders.EntraWorkforceEventResponses|100%||
|Entra.EventHandlers.Workforce.Builders.ResponseBuilders.VerifiedIdClaimVali<br/>dationResponseBuilder|100%|100%|
|Entra.EventHandlers.Workforce.Handlers.Base.VerifiedIdClaimValidationHandle<br/>rBase|84.3%|100%|

</details>
