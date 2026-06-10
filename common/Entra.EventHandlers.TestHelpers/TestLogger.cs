using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.TestHelpers;

public readonly record struct LogEntry(LogLevel Level, string Message, Exception? Exception, object? State);

public abstract class TestLoggerBase : ILogger
{
    private readonly List<LogEntry> _entries = [];
    private readonly Stack<object> _scopes = new();

    public IReadOnlyList<LogEntry> Entries => _entries;

    public IReadOnlyList<object> Scopes => [.. _scopes];

    public IDisposable BeginScope<TState>(TState state)
        where TState : notnull
    {
        var scope = new TestScope(state);
        _scopes.Push(scope);
        return scope;
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        _entries.Add(new LogEntry(
            logLevel,
            formatter(state, exception),
            exception,
            state));
    }
}

public sealed class TestLogger : TestLoggerBase
{
}

public sealed class TestLogger<T> : TestLoggerBase, ILogger<T>
{
}
