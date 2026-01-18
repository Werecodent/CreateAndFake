using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
//using System.Runtime.CompilerServices;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.Design;

#pragma warning disable RCS1256 // False positive.

/// <summary>Handles common argument <see cref="Exception"/> cases.</summary>
public static class ArgumentGuard
{
    /// <summary>
    ///     Prevents further execution if <paramref name="value"/> is <see langword="null"/>.
    /// </summary>
    /// <param name="value">Passed parameter value.</param>
    /// <param name="name">Name of the parameter.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is null.</exception>
    [DebuggerStepThrough]
    public static void ThrowIfNull(
        [NotNull] object? value,
        /*[CallerArgumentExpression(nameof(value))]*/string? name = null
    )
    {
        if (value is null)
        {
            throw new ArgumentNullException(name);
        }
    }

    /// <summary>
    ///     Checks if <paramref name="value"/> is an asynchronous <see cref="Type"/> .
    /// </summary>
    /// <param name="value">Passed parameter value.</param>
    public static bool IsAsynchronous(object? value)
    {
        return (value is Task task && !task.IsCompleted)
            || (value?.GetType()).Inherits(typeof(IAsyncEnumerable<>));
    }

    /// <summary>
    ///     Prevents further execution if <paramref name="value"/>
    ///     is an asynchronous <see cref="Type"/>.
    /// </summary>
    /// <param name="value">Passed parameter value.</param>
    /// <param name="message">Error details for the potential <see cref="Exception"/>.</param>
    /// <exception cref="ToolException">If <paramref name="value"/> is async.</exception>
    [DebuggerStepThrough]
    public static void ThrowIfAsynchronous(object? value, string message)
    {
        if (IsAsynchronous(value))
        {
            throw new ToolException(message);
        }
    }
}

#pragma warning restore RCS1256
