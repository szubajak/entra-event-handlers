# Summary
<details open><summary>Summary</summary>

|||
|:---|:---|
| Generated on: | 08/19/2026 - 13:39:10 |
| Parser: | MultiReport (7x Cobertura) |
| Assemblies: | 7 |
| Classes: | 68 |
| Files: | 63 |
| **Line coverage:** | 87.9% (922 of 1048) |
| Covered lines: | 922 |
| Uncovered lines: | 126 |
| Coverable lines: | 1048 |
| Total lines: | 2687 |
| **Branch coverage:** | 100% (134 of 134) |
| Covered branches: | 134 |
| Total branches: | 134 |
| **Method coverage:** | [Feature is only available for sponsors](https://reportgenerator.io/pro) |

</details>

## Coverage
<details><summary>Entra.EventHandlers - 100%</summary>

|**Name**|**Line**|**Branch**|
|:---|---:|---:|
|**Entra.EventHandlers**|**100%**|**100%**|
|Entra.EventHandlers.Builders.ActionBuilders.PrefillValuesBuilder|100%||
|Entra.EventHandlers.Builders.EntraEventResponses|100%||
|Entra.EventHandlers.Builders.ResponseBuilders.AttributeCollectionStartRespo<br/>nseBuilder|100%|100%|
|Entra.EventHandlers.Builders.ResponseBuilders.AttributeCollectionSubmitResp<br/>onseBuilder|100%|100%|
|Entra.EventHandlers.Builders.ResponseBuilders.EmailOtpSendResponseBuilder|100%|100%|
|Entra.EventHandlers.Builders.ResponseBuilders.PasswordSubmitResponseBuilder|100%|100%|
|Entra.EventHandlers.Builders.ResponseBuilders.TokenIssuanceStartResponseBui<br/>lder|100%|100%|
|Entra.EventHandlers.Handlers.Base.AttributeCollectionStartHandlerBase|100%|100%|
|Entra.EventHandlers.Handlers.Base.AttributeCollectionSubmitHandlerBase|100%|100%|
|Entra.EventHandlers.Handlers.Base.EmailOtpSendHandlerBase|100%|100%|
|Entra.EventHandlers.Handlers.Base.PasswordSubmitHandlerBase|100%|100%|
|Entra.EventHandlers.Handlers.Base.TokenIssuanceStartHandlerBase|100%|100%|

</details>
<details><summary>Entra.EventHandlers.AspNetCore - 62.3%</summary>

|**Name**|**Line**|**Branch**|
|:---|---:|---:|
|**Entra.EventHandlers.AspNetCore**|**62.3%**|**100%**|
|Entra.EventHandlers.AspNetCore.Abstractions.EntraEndpointBase|100%|100%|
|Entra.EventHandlers.AspNetCore.Abstractions.EntraTypedEndpointBase<TEvent, <br/>TResponse>|0%||
|Entra.EventHandlers.AspNetCore.Abstractions.EntraTypedEndpointBase<TEvent, <br/>TResponse>|0%||
|Entra.EventHandlers.AspNetCore.Adapters.RequestAdapter|100%|100%|
|Entra.EventHandlers.AspNetCore.Adapters.RequestAdapter<TEvent>|100%|100%|
|Entra.EventHandlers.AspNetCore.Adapters.ResponseAdapter|100%||
|Entra.EventHandlers.AspNetCore.DI.ServiceCollectionExtensions|100%||
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
<details><summary>Entra.EventHandlers.AzureFunctions - 71%</summary>

|**Name**|**Line**|**Branch**|
|:---|---:|---:|
|**Entra.EventHandlers.AzureFunctions**|**71%**|**100%**|
|Entra.EventHandlers.AzureFunctions.Abstractions.EntraFunctionBase|100%|100%|
|Entra.EventHandlers.AzureFunctions.Adapters.RequestAdapter|100%|100%|
|Entra.EventHandlers.AzureFunctions.Adapters.RequestAdapter<TEvent>|100%|100%|
|Entra.EventHandlers.AzureFunctions.Adapters.ResponseAdapter|100%||
|Entra.EventHandlers.AzureFunctions.Base.AttributeCollectionStartFunctionBas<br/>e|0%||
|Entra.EventHandlers.AzureFunctions.Base.AttributeCollectionSubmitFunctionBa<br/>se|0%||
|Entra.EventHandlers.AzureFunctions.Base.EmailOtpSendFunctionBase|0%||
|Entra.EventHandlers.AzureFunctions.Base.PasswordSubmitFunctionBase|0%||
|Entra.EventHandlers.AzureFunctions.Base.TokenIssuanceStartFunctionBase|0%||
|Entra.EventHandlers.AzureFunctions.Base.VerifiedIdClaimValidationFunctionBa<br/>se|0%||
|Entra.EventHandlers.AzureFunctions.DI.ServiceCollectionExtensions|100%||
|Entra.EventHandlers.AzureFunctions.Routing.EntraEventRouterFunctionBase|100%|100%|

</details>
<details><summary>Entra.EventHandlers.Hosting - 100%</summary>

|**Name**|**Line**|**Branch**|
|:---|---:|---:|
|**Entra.EventHandlers.Hosting**|**100%**|**100%**|
|Entra.EventHandlers.Hosting.DI.ServiceCollectionExtensions|100%|100%|
|Entra.EventHandlers.Hosting.Extensions.ExceptionExtensions|100%|100%|
|Entra.EventHandlers.Hosting.Orchestrators.EntraEventOrchestrator|100%|100%|
|Entra.EventHandlers.Hosting.Orchestrators.EntraEventOrchestrator<TEvent, TR<br/>esponse>|100%|100%|
|Entra.EventHandlers.Hosting.Resolvers.EntraEventHandlerResolver|100%|100%|

</details>
<details><summary>Entra.EventHandlers.Observability - 95.1%</summary>

|**Name**|**Line**|**Branch**|
|:---|---:|---:|
|**Entra.EventHandlers.Observability**|**95.1%**|**100%**|
|Entra.EventHandlers.Observability.Clients.ObservabilityApiClient|28.5%||
|Entra.EventHandlers.Observability.Context.EventLogContext|100%||
|Entra.EventHandlers.Observability.Decorators.ObservabilityHandlerDecorator<<br/>TRequest, TResponse>|100%||
|Entra.EventHandlers.Observability.Decorators.ObservabilityHandlerDecorator<<br/>TRequest, TResponse>|100%||
|Entra.EventHandlers.Observability.DI.ServiceCollectionExtenstions|100%|100%|
|Entra.EventHandlers.Observability.Factories.EventLogMapperFactory|100%||
|Entra.EventHandlers.Observability.Logging.EventLogPublisher|100%||
|Entra.EventHandlers.Observability.Logging.EventLogWriter|100%||
|Entra.EventHandlers.Observability.Mappers.EmailOtpSendEventLogMapper|100%||
|Entra.EventHandlers.Observability.Mappers.EventLogContextMapper|100%|100%|
|Entra.EventHandlers.Observability.Models.CustomLogEntry|100%||

</details>
<details><summary>Entra.EventHandlers.TestHelpers - 93.6%</summary>

|**Name**|**Line**|**Branch**|
|:---|---:|---:|
|**Entra.EventHandlers.TestHelpers**|**93.6%**|**100%**|
|Entra.EventHandlers.TestHelpers.EventSamples|100%||
|Entra.EventHandlers.TestHelpers.HandlerCoreTest|100%|100%|
|Entra.EventHandlers.TestHelpers.TestEvent|100%||
|Entra.EventHandlers.TestHelpers.TestHandler|100%||
|Entra.EventHandlers.TestHelpers.TestLoggerBase|94.1%||
|Entra.EventHandlers.TestHelpers.TestScope|100%||
|Entra.EventHandlers.TestHelpers.TestUtils<T>|100%||
|Entra.EventHandlers.TestHelpers.ThrowingStream|20%||

</details>
<details><summary>Entra.EventHandlers.Workforce - 100%</summary>

|**Name**|**Line**|**Branch**|
|:---|---:|---:|
|**Entra.EventHandlers.Workforce**|**100%**|**100%**|
|Entra.EventHandlers.Workforce.Builders.ActionBuilders.FailedClaimsBuilder|100%||
|Entra.EventHandlers.Workforce.Builders.EntraWorkforceEventResponses|100%||
|Entra.EventHandlers.Workforce.Builders.ResponseBuilders.VerifiedIdClaimVali<br/>dationResponseBuilder|100%|100%|
|Entra.EventHandlers.Workforce.Handlers.Base.VerifiedIdClaimValidationHandle<br/>rBase|100%|100%|

</details>
