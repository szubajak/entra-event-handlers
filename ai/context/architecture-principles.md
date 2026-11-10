# Architecture Principles

## Layering

- Host-agnostic design
- Abstractions before implementations
- Dependency direction from contracts toward implementations and hosting adapters
- Hosting-specific dependencies remain outside Abstractions and core business logic

## Extension Points

- Extension points over one-off customization
- Stable public interfaces for handler and hosting boundaries
- Strongly typed contracts for events, responses, actions, and handler results

## Runtime Design

- Dependency injection first
- Correlation-aware structured logging
- Explicit cancellation propagation
- Protocol validation before event-specific business logic
- Consistent exception and fallback-response behavior

## Compatibility and Documentation

- Backward compatibility by default
- Documentation as a first-class artifact
- Optional cross-cutting extensions, such as Security and Observability, must not leak into the core contracts
