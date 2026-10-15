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

## Documentation

Developer-facing functionality must be documented.