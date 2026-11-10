# Entra.EventHandlers Agent Context

## Project Overview

Entra.EventHandlers is a .NET framework for building event-driven extensions for Microsoft Entra External ID.

The framework provides:

- Event orchestration
- Dependency injection integration
- Azure Functions hosting support
- ASP.NET Core hosting support
- Extensible event handler model
- Strong focus on maintainability and discoverability

## Product Goals

The framework should:

- Simplify implementation of Entra External ID event handlers
- Reduce boilerplate code
- Encourage clean architecture
- Support AI-assisted development
- Remain easy to adopt for .NET developers

## Architectural Principles

1. Extension points over customization.
2. Convention over configuration.
3. Dependency injection first.
4. Minimal hosting-specific logic.
5. Backward compatibility whenever possible.
6. Clear abstractions over implementation details.
7. Testability by default.

## Project Map

The solution is organized into the following layers:

| Project | Responsibility |
|---|---|
| `Entra.EventHandlers.Abstractions` | Public event, response, action, interface, result, protocol, and validation contracts |
| `Entra.EventHandlers` | Core handler base classes, response builders, logging, and execution pipeline |
| `Entra.EventHandlers.Workforce` | Workforce event and handler support |
| `Entra.EventHandlers.Hosting` | Handler resolution, orchestration, and shared hosting registration |
| `Entra.EventHandlers.AspNetCore` | ASP.NET Core adapters, endpoints, routing, and exception handling |
| `Entra.EventHandlers.AzureFunctions` | Azure Functions adapters, functions, and routing |
| `Entra.EventHandlers.Security` | Key Vault, certificate, RSA, and password-context security features |
| `Entra.EventHandlers.Observability` | Handler decoration, event-log mapping, publishing, and observability integration |
| `Entra.EventHandlers.TestData` | Shared test event and response factories |
| `Entra.EventHandlers.TestHelpers` | Shared test doubles and test infrastructure |

The normal dependency direction is:

```text
Abstractions
    ↓
Core / Workforce
    ↓
Hosting
    ↓
ASP.NET Core / Azure Functions
```

Security and Observability are optional extensions over the core contracts. Do not introduce host-specific dependencies into Abstractions or core handler business logic.

## Handler Pipeline

Handler base classes provide the shared `HandleAsync` pipeline. The pipeline may perform protocol validation, establish correlation logging, measure execution time, invoke `HandleCoreAsync`, handle exceptions, and create protocol-valid fallback responses.

- Derived handlers should override `HandleCoreAsync`, not replace the shared `HandleAsync` pipeline.
- Preserve protocol validation, correlation logging, execution timing, and `EntraHandlerResult<TResponse>` semantics.
- Preserve cancellation behavior and rethrow `OperationCanceledException`.
- Keep event-specific business logic in the derived handler implementation.

## Current Supported Hosts

- Azure Functions
- ASP.NET Core

## Non-Goals

- Custom workflow engine
- Business process orchestration
- Long-running operations
- Platform-specific implementations inside abstractions

## Definition of Done

A feature is complete when:

- Implementation exists
- Unit tests exist
- Documentation exists
- Backward compatibility has been evaluated
- Public APIs have been reviewed