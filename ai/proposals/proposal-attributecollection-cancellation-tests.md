# Technical Design Proposal

## Metadata

| Field | Value |
|---------|---------|
| Proposal ID | proposal-attributecollection-cancellation-tests |
| Status | Draft |
| Author | KubaAI |
| Created | 2026-11-04 |
| Last Updated | 2026-11-04 |
| Branch | feature/proposal-attributecollection-cancellation-tests |
| Related Specification | AttributeCollectionStart Handler base behavior |
| Related ADRs | |
| Related Reviews | |

---

## Executive Summary

Add missing unit test coverage for the OperationCanceledException catch path in AttributeCollectionStartHandlerBase.HandleAsync. This ensures cancellation is logged at Information level and the exception is rethrown without converting to a default response. No production code changes; only unit tests and supportive test utilities are added.

---

## Business Context

Reliable cancellation handling is critical for graceful shutdowns and client-requested aborts. Verifying the logging contract and rethrow behavior prevents regressions and documents expected semantics.

### Problem Statement

The OperationCanceledException catch block in AttributeCollectionStartHandlerBase is not exercised by current tests, leaving observable behavior (log level/message and rethrow) unverified.

### Expected Value

- Increased test coverage for cancellation path.
- Confidence that cancellation is logged and propagated.
- Prevent regressions in exception handling semantics.

### Success Criteria

- New unit test(s) fail if the catch block behavior changes.
- Coverage tools show the catch block executed by tests.

---

## Goals

### Functional Goals

- Add a unit test that triggers OperationCanceledException from HandleCoreAsync and validates behavior.
- Keep scope limited to tests; do not modify production code.

### Non-Functional Goals

- Maintainability: self-contained test utilities.
- Reliability: deterministic, fast tests.

---

## Scope

### In Scope

- New unit test(s) in Entra.EventHandlers.UnitTests targeting AttributeCollectionStartHandlerBase cancellation behavior.
- Optional new test-only handler to simulate cancellation.

### Out of Scope

- Any changes to production handler code or logging implementation.
- Broader refactors of test infrastructure.

---

## Architecture Impact

None. Tests only.

### Affected Layers

- Application: Unit tests for handler base.

### Affected Components

- core/Entra.EventHandlers.UnitTests

---

## Affected Projects

- Entra.EventHandlers.UnitTests
- Entra.EventHandlers.TestHelpers (test-only utility class addition if needed)

---

## Proposed Changes

### New Types

#### TestCancellableAttributeCollectionStartHandler (tests only)

Purpose:
- A test-only handler that deterministically throws OperationCanceledException from HandleCoreAsync, optionally after observing the provided CancellationToken.

Responsibilities:
- Override HandleCoreAsync and return Task.FromCanceled<AttributeCollectionStartResponse>(cancellationToken) or directly throw new OperationCanceledException(cancellationToken).
- Record CoreTest metadata (reuse HandlerCoreTest) to assert invocation and captured token.

Public Visibility:
- Internal/public within test assembly only.

---

### Modified Types

None.

---

### Deprecated Types

None.

---

## Dependency Injection Impact

None.

---

## Public API Impact

None.

---

## Backward Compatibility

N/A (tests only).

---

## Acceptance Criteria

### Functional

- [ ] A unit test triggers OperationCanceledException within AttributeCollectionStartHandlerBase.HandleAsync and asserts that:
  - [ ] The exception is rethrown to the caller (Assert.ThrowsAsync<OperationCanceledException>).
  - [ ] A single Information-level log entry exists with the message containing "was canceled".
  - [ ] HandleCoreAsync is invoked exactly once and receives the provided CancellationToken.
  - [ ] No default error response is produced.

### Quality

- [ ] All tests pass.
- [ ] No analyzer warnings introduced.
- [ ] No production files changed.

### Documentation

- [ ] Inline test comments clarify intent and behavior.

---

## Testing Strategy

### Unit Tests

Add test in core/Entra.EventHandlers.UnitTests/Handlers/Base/AttributeCollectionStartHandlerBaseTests.cs (or a new dedicated test class) named HandleAsync_Cancellation_PropagatesAndLogsInformation.

Test steps:
- Arrange: create AttributeCollectionStartEvent via TestEvents. Create CancellationTokenSource and cancel it after scheduling the call, or pass a pre-canceled token.
- Use a dedicated TestCancellableAttributeCollectionStartHandler that throws OperationCanceledException using the provided token.
- Act/Assert: await SUT.HandleAsync(evt, token) and assert OperationCanceledException is thrown.
- Verify logger captured Information log with message "Entra event handling was canceled" and includes DurationMs structured state.
- Verify CoreTest.HandleCoreCallCount == 1 and CapturedCancellationToken == token.
- Verify no EntraHandlerResult is returned (because exception propagates).

### Integration Tests

None required.

### Manual Validation

Not required.

---

## Documentation Impact

None.

---

## Observability Impact

None.

---

## Security Impact

None.

---

## Performance Impact

None.

---

## Alternatives Considered

### Alternative 1: Modify existing TestAttributeCollectionStartHandler to support cancellation flag

Pros:
- Reuses existing utility.

Cons:
- Changes a commonly used test helper; increases blast radius.

Reason Rejected:
- Prefer a new narrowly-scoped test helper for cancellation to avoid side effects.

### Alternative 2: Use Task.Delay with canceled token inside HandleCoreAsync

Pros:
- Simulates realistic cancellation.

Cons:
- Slower and less deterministic; unnecessary for logic verification.

Reason Rejected:
- Directly throwing/Task.FromCanceled is simpler and faster.

---

## Risks

| Risk | Impact | Mitigation |
|--------|--------|--------|
| Log message assertion too brittle | Test may fail on minor wording changes | Assert on key substring and log level only |
| Test flakiness with timing | False negatives due to races | Use immediate Task.FromCanceled and pre-canceled tokens |

---

## Open Questions

- Should the test assert the stopwatch is stopped? Not directly accessible; rely on absence of further logs and propagation behavior.

---

## Implementation Plan

### Phase 1
- Add TestCancellableAttributeCollectionStartHandler to core/Entra.EventHandlers.UnitTests/Utils/Handlers.

### Phase 2
- Add new test HandleAsync_Cancellation_PropagatesAndLogsInformation to AttributeCollectionStartHandlerBaseTests or a new dedicated test class.

### Phase 3
- Run tests locally and in CI; ensure coverage reports include the cancellation catch block.

---

## DevAI Instructions

Implement only the changes described in this proposal.

Do not introduce unrelated refactorings.

Keep changes scoped to the affected files and projects listed above.

Concrete guidance:
- Create core/Entra.EventHandlers.UnitTests/Utils/Handlers/TestCancellableAttributeCollectionStartHandler.cs
  - Derive from AttributeCollectionStartHandlerBase
  - In HandleCoreAsync: call CoreTest.Record(cancellationToken); then return await Task.FromCanceled<AttributeCollectionStartResponse>(cancellationToken);
- In core/Entra.EventHandlers.UnitTests/Handlers/Base/AttributeCollectionStartHandlerBaseTests.cs add a new [Fact]:
  - Arrange: use Fixture to create event; var cts = new CancellationTokenSource(); var sut = new TestCancellableAttributeCollectionStartHandler(logger);
  - Use TestLogger to capture logs; pass pre-canceled token: using var cts = new CancellationTokenSource(); cts.Cancel();
  - Act/Assert: await Assert.ThrowsAsync<OperationCanceledException>(() => sut.HandleAsync(evt, cts.Token));
  - Assert logger contains Information with message containing "was canceled" and includes DurationMs state key.
  - Assert sut.CoreTest.HandleCoreCallCount == 1 and CapturedCancellationToken == cts.Token.

---

## ReviewAI Validation Checklist

- [ ] Acceptance criteria satisfied
- [ ] Tests added and passing
- [ ] No production changes
- [ ] Coverage of OperationCanceledException catch path verified

---

## Recommendation

Recommended for:

- Approval

Justification:
- Addresses a concrete test coverage gap with minimal, isolated changes and no production impact.
