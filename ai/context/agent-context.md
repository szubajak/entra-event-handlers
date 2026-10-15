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