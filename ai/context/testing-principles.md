# Testing Principles

## Test Stack

Use the repository's existing test stack:

- xUnit v3
- FluentAssertions
- NSubstitute
- AutoFixture
- Microsoft Testing Platform

Do not introduce another test framework, assertion library, or mocking library without a specific architectural reason.

## Test Structure

Prefer the following structure:

1. Arrange
2. Act
3. Assert

Use the existing test project and shared test-helper conventions. Test classes should follow the surrounding `*Tests` naming style, and test doubles should use the existing `Test*` helper patterns where appropriate.

## Test Scope

- Test handler business behavior primarily at the handler level.
- Use integration tests for hosting, routing, registration, transport, and external integration behavior.
- Do not duplicate all handler behavior through HTTP or Azure Functions integration tests.
- Test cancellation propagation explicitly where supported.
- Test fallback responses and exception wrapping at the handler boundary.
- Test dependency registration and service lifetimes when registration behavior is part of the feature.

## Handler Tests

- Exercise the public `HandleAsync` pipeline rather than calling protected implementation details directly.
- Verify protocol validation prevents invalid requests from reaching `HandleCoreAsync`.
- Verify successful responses, fallback responses, captured exceptions, logging behavior, and cancellation behavior when relevant.
- Preserve the existing `EntraHandlerResult<TResponse>` semantics.

## Test Naming

Name unit tests and applicable integration tests using:

`MethodName_StateUnderTest_ExpectedBehavior`

The only accepted two-component exception is:

`MethodName_Success`

Use `MethodName_Success` only for a straightforward happy-path test. All other tests, including validation, failure, exception, caching, cancellation, and dependency-behavior tests, must use all three components. Preserve the production method's `Async` suffix when it is part of the method name.
