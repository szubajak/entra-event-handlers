# Copilot Instructions

## Repository Scope

- This repository targets .NET 10.
- Read the relevant files under `ai/context/` before making architectural, API, testing, or documentation changes.
- Treat current source code, project references, `.editorconfig`, `Directory.Build.props`, and existing documentation as authoritative.

## Architecture

- Keep `Entra.EventHandlers.Abstractions` host-independent.
- Preserve dependency direction: Abstractions → Core/Workforce → Hosting → ASP.NET Core or Azure Functions.
- Keep hosting-specific code out of Abstractions and core handler business logic.
- Treat Security and Observability as optional extensions.
- Derived handlers should override `HandleCoreAsync`, not replace the shared `HandleAsync` pipeline.
- Preserve protocol validation, correlation logging, execution timing, cancellation behavior, exception handling, fallback responses, and `EntraHandlerResult<TResponse>` semantics.

## Coding Style

- Follow `.editorconfig`, nullable reference types, implicit usings, file-scoped namespaces, primary constructors, four-space indentation, and existing naming conventions.
- Use dependency injection for services.
- Avoid static mutable state and static service dependencies. Static factories and protocol constants are acceptable.
- Preserve XML documentation on public APIs.

## Testing

- Use the existing xUnit v3, FluentAssertions, NSubstitute, AutoFixture, and Microsoft Testing Platform setup.
- Prefer handler-level unit tests for business behavior.
- Use integration tests for hosting, routing, registration, transport, and external integration behavior.
- Structure tests with Arrange, Act, and Assert.
- Name unit and applicable integration tests using `MethodName_StateUnderTest_ExpectedBehavior`.
- The only accepted two-part exception is `MethodName_Success`, and it is reserved for straightforward happy-path tests.
- Preserve the `Async` suffix when it is part of the production method name.
- Test success, validation, failure, exception, cancellation, fallback-response, and dependency-interaction behavior where applicable.

## Public APIs and Packages

- Treat interfaces, event models, response models, result types, and package metadata as public contracts.
- Prefer additive changes and evaluate compatibility before changing public APIs.
- Do not add host-specific dependencies to Abstractions.
- Preserve PackageId, license, README, and existing package metadata unless the change explicitly requires updating them.

## Validation

- Build and run relevant tests after changes.
- Update documentation for public, architectural, hosting, event, security, observability, or developer-workflow changes.
- Do not implement roadmap ideas unless the user explicitly requests them.
