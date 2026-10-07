# Architecture
 
This document explains the architectural design of the Entra.EventHandlers ecosystem and how the individual packages work together.
 
## Design Goals
 
Entra.EventHandlers was designed with the following goals:
 
- Strongly typed Microsoft Entra External ID event models
- Separation between business logic and hosting infrastructure
- Reusable handlers across hosting platforms
- Dependency injection friendly APIs
- Consistent developer experience
- Testable application code
- Forward compatibility as Microsoft Entra evolves
 
---
 
# High-Level Overview
 
The ecosystem is organized into multiple layers.
 
```text
┌───────────────────────────────────────┐
│ Entra.EventHandlers.Abstractions │
└─────────────────▲─────────────────────┘
│
┌───────────┴───────────┐
│ │
▼ ▼
┌───────────────┐ ┌────────────────────┐
│ External ID │ │ Workforce │
│ Implementation│ │ Implementation │
└───────▲───────┘ └─────────▲──────────┘
│ │
└──────────┬──────────┘
▼
┌─────────────────────────────┐
│ Shared Hosting Infrastructure│
└──────────────▲──────────────┘
│
┌──────────┴──────────┐
▼ ▼
┌────────────────┐ ┌────────────────┐
│ ASP.NET Core │ │ Azure Functions│
│ Hosting │ │ Hosting │
└────────────────┘ └────────────────┘
```
 
---
 
# Package Responsibilities
 
## Entra.EventHandlers.Abstractions
 
The abstractions package defines the public protocol contract.
 
Contents:
 
- Event models
- Response models
- Action models
- Interfaces
- Protocol constants
- Shared primitives
 
Design goals:
 
- Stable public API
- Minimal dependencies
- Reusable by external extensions
- MIT licensed
 
This package does not contain:
 
- Hosting integrations
- Dependency injection
- Logging
- Business logic
- Validation pipelines
 
---
 
## Entra.EventHandlers
 
The core package provides the implementation layer for Microsoft Entra External ID events.
 
Contents:
 
- Handler base classes
- Fluent response builders
- Validation
- Logging integration
- Correlation tracking
- Execution timing
 
Example:
 
```csharp
public class TokenIssuanceStartHandler
: TokenIssuanceStartHandlerBase
{
}
```
 
Most applications directly depend on this package.
 
---
 
## Entra.EventHandlers.Workforce
 
Provides support for Workforce authentication events.
 
Contents:
 
- Workforce event models
- Workforce response builders
- Workforce handler base classes
 
This package follows the same programming model as the External ID package.
 
---
 
# Hosting Architecture
 
One of the core design principles is complete separation between handler logic and hosting logic.
 
Business logic should not depend on:
 
- ASP.NET Core
- Azure Functions
- HTTP abstractions
- Hosting-specific concerns
 
Instead, handlers depend only on Entra.EventHandlers.
 
Example:
 
```csharp
public class EmailOtpSendHandler
: EmailOtpSendHandlerBase
{
}
```
 
The same handler can be hosted in:
 
- ASP.NET Core
- Azure Functions
 
without modification.
 
---
 
# ASP.NET Core Hosting
 
Package:
 
```text
Entra.EventHandlers.AspNetCore
```
 
Provides:
 
- Endpoint infrastructure
- Request handling
- Dependency injection integration
- Event routing
- Request and response adaptation
 
Recommended when:
 
- Building Web APIs
- Hosting inside existing ASP.NET Core applications
- Running in containers
 
---
 
# Azure Functions Hosting
 
Package:
 
```text
Entra.EventHandlers.AzureFunctions
```
 
Provides:
 
- Function infrastructure
- Event routing
- Trigger integration
- Dependency injection integration
- Request and response adaptation
 
Recommended when:
 
- Building serverless solutions
- Running on Azure Functions
- Scaling on demand
 
---
 
# Request Processing Pipeline
 
Every incoming Entra request follows the same logical flow.
 
```text
Entra Request
│
▼
Hosting Adapter
│
▼
Request Validation
│
▼
Handler Resolution
│
▼
Handler Execution
│
▼
Response Builder
│
▼
Entra Response
```
 
This ensures consistent behavior across all hosting models.
 
---
 
# Dependency Injection
 
Handlers are resolved through the standard .NET dependency injection container.
 
Registration:
 
```csharp
builder.Services.AddEntraEventHandlers();
```
 
Benefits:
 
- Constructor injection
- Service reuse
- Easy testing
- Familiar .NET patterns
 
Example:
 
```csharp
public class AttributeCollectionStartHandler(
ILogger<AttributeCollectionStartHandler> logger,
ICustomerRepository repository)
: AttributeCollectionStartHandlerBase(logger)
{
}
```
 
---
 
# Fluent Response Builders
 
Responses are created using fluent builders instead of manually constructing protocol objects.
 
Example:
 
```csharp
return EntraEventResponses
.AttributeCollectionStart()
.SetPrefillValues()
.Add("email", "user@contoso.com")
.Done()
.Build();
```
 
Benefits:
 
- Discoverable API surface
- Compile-time safety
- Reduced protocol knowledge required
- Easier maintenance
 
---
 
# Event-Centric Design
 
Each Microsoft Entra event is represented by:
 
- an event model
- a response model
- a handler base class
- event-specific response builders
 
Example:
 
```text
TokenIssuanceStartEvent
TokenIssuanceStartResponse
TokenIssuanceStartHandlerBase
TokenIssuanceStartResponseBuilder
```
 
This design keeps implementation details close to the event they belong to.
 
---
 
# Testing Strategy
 
Because handlers are isolated from hosting infrastructure, they can be tested directly.
 
Example:
 
```csharp
var response = await handler.HandleAsync(request);
```
 
No ASP.NET Core host or Azure Function runtime is required.
 
This enables:
 
- Fast unit tests
- Predictable behavior
- Easy mocking
- High test coverage
 
---
 
# Choosing Packages
 
| Scenario | Package |
|-----------|----------|
| Event contracts only | Entra.EventHandlers.Abstractions |
| External ID handlers | Entra.EventHandlers |
| Workforce handlers | Entra.EventHandlers.Workforce |
| ASP.NET Core hosting | Entra.EventHandlers.AspNetCore |
| Azure Functions hosting | Entra.EventHandlers.AzureFunctions |
 
---
 
# Key Architectural Principles
 
The Entra.EventHandlers ecosystem is built around several core principles:
 
- Strong typing over raw JSON
- Event-centric programming model
- Separation of business and hosting concerns
- Consistent APIs across events
- Dependency injection by default
- Testability first
- Hosting model independence
- Minimal developer ceremony
 
These principles allow developers to focus on business logic rather than Entra protocol details.