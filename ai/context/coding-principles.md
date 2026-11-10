# Coding Principles

## Public APIs

Public APIs are long-term contracts.

Changes to public APIs require:

- Compatibility review
- Migration strategy
- Documentation update

## Dependency Injection

Prefer dependency injection.

Avoid static dependencies.

## Abstractions

Create abstractions only when:

- Multiple implementations are expected.
- Hosting independence is required.

## Testing

Every new feature should include:

- Unit tests
- Happy path scenarios
- Failure scenarios

### Unit Test Naming

Name every unit test using:

`MethodName_StateUnderTest_ExpectedBehavior`

Use the production method name as the first component, describe the relevant input or condition as the second component, and describe the observable result as the third component. Preserve the method's `Async` suffix when it is part of the production method name.

The only two-component exception is:

`MethodName_Success`

Use this form only for a straightforward happy-path test. All other tests, including failure, validation, exception, caching, cancellation, and dependency-behavior tests, must use all three components.

Examples:

- `HandleAsync_InvalidRequest_ReturnsBlockPageResponse`
- `GetRsaAsync_CachedRsaExists_ReturnsCachedRsa`
- `Deserialize_InvalidOdataType_ThrowsEntraValidationException`
- `Serialize_ContinueWithDefaultBehaviorAction_ReturnsExpectedJson`
- `Map_Success`

Apply the same naming convention consistently to integration tests when they exercise a method-level behavior.

## Documentation

Developer-facing functionality must be documented.