using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.Design;

/// <summary>Handles common argument exception cases.</summary>
public static class ArgumentGuard
{
    /// <summary>Prevents further execution if the parameter is null.</summary>
    /// <param name="value">Passed parameter value.</param>
    /// <param name="name">Name of the parameter.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is null.</exception>
    [DebuggerStepThrough]
    public static void ThrowIfNull([NotNull] object? value, string name)
    {
        if (value is null)
        {
            throw new ArgumentNullException(name);
        }
    }

    /// <summary>Prevents further execution if the parameter is asynchronous.</summary>
    /// <param name="value">Passed parameter value.</param>
    /// <param name="message">Error message for the potential exception.</param>
    [DebuggerStepThrough]
    public static void ThrowIfAsync(object? value, string message)
    {
        if (
            (value is Task task && !task.IsCompleted)
            || (value?.GetType().Inherits(typeof(IAsyncEnumerable<>)) ?? false)
        )
        {
            throw new ToolException(message);
        }
    }
}
