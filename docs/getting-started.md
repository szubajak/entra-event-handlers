# Getting Started with Entra.EventHandlers
 
Entra.EventHandlers is a .NET library for implementing Microsoft Entra External ID
custom authentication extension event handlers.
 
It provides strongly typed event models, event handler dispatching,
dependency injection integration, fluent response builders, and hosting integrations
for ASP.NET Core and Azure Functions.
 
## Installation
 
Install the core package:
 
```bash
dotnet add package Entra.EventHandlers
```
 
Add the integration package for your hosting model:
 
```bash
dotnet add package Entra.EventHandlers.AspNetCore
```
 
or:
 
```bash
dotnet add package Entra.EventHandlers.AzureFunctions
```
 
## Create an event handler
 
Implement an event handler for the Microsoft Entra External ID event you want to process.
 
The following example handles the Token Issuance Start event and adds custom claims
to the issued token.
 
```csharp
public class TokenIssuanceStartHandler(ILogger<TokenIssuanceStartHandler> logger)
    : TokenIssuanceStartHandlerBase(logger)
{
    protected override Task<TokenIssuanceStartResponse> HandleCoreAsync(
        TokenIssuanceStartEvent request,
        CancellationToken cancellationToken = default)
    {
        // Extract user ID (GUID)
        var userId = request.Data.AuthenticationContext?.User?.Id;

        // Example: determine roles based on user ID
        string[] roles = userId switch
        {
            // Example: special admin GUID
            var id when id == Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
                => ["Admin", "PowerUser"],

            // Default
            _ => ["User"]
        };

        // Example: add custom claims
        var customClaims = new Dictionary<string, object>
        {
            { "department", "Engineering" },
            { "roles", roles }
        };

        return Task.FromResult(
            EntraEventResponses
                .TokenIssuanceStart()
                .ProvideClaimsForToken(customClaims)
                .Build());
    }
}
```
 
## Register Entra.EventHandlers
 
Register Entra.EventHandlers and your handlers with dependency injection:
 
```csharp
builder.Services.AddEntraEventHandlers();
```
 
The same handler implementations can be used with either ASP.NET Core or Azure Functions.
 
## Choose a hosting model
 
Entra.EventHandlers separates event handler logic from the hosting integration.
 
### ASP.NET Core
 
Install `Entra.EventHandlers.AspNetCore` when hosting your custom authentication
extension in an ASP.NET Core application.
 
The integration supports both a shared Entra event router and individual event endpoints.
 
See ../samples/ApiSample for a complete ASP.NET Core example.
 
### Azure Functions
 
Install `Entra.EventHandlers.AzureFunctions` when hosting your custom authentication
extension in Azure Functions.
 
The integration supports both router functions and individual event functions.
 
See ../samples/AzureFunctionsSample for the complete
Azure Functions example included in this repository.
 
## Samples
 
Choose a sample based on what you want to learn:
 
- ../samples/Sample.Common
Shared handler implementations demonstrating fluent response builders, custom claims,
prefill values, block responses, and handler-specific business logic.
 
- ../samples/ApiSample
Complete ASP.NET Core host using the shared sample handlers.
 
- ../samples/AzureFunctionsSample
Complete Azure Functions host using the shared sample handlers.
 
- [Minimal Azure Functions sample](https://github.com/szubajak/entra-eventhandlers-azurefunctions)
Standalone Azure Functions Isolated Worker example focused on a single
`EmailOtpSend` handler and function. This is a good starting point when you want
the smallest end-to-end Azure Functions example.
 
If you are learning the handler API itself, start with `Sample.Common`.
 
If you want to build an application, choose the ASP.NET Core or Azure Functions
sample for your hosting model.
 
## Next steps
 
More detailed documentation for event handlers, dependency injection, hosting,
and testing will be added over time.
 
Until then, the sample projects show how the library components work together
in complete applications.