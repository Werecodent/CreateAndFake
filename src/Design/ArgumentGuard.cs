using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
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
        [CallerArgumentExpression(nameof(value))] string? name = null
    )
    {
        if (value is null)
        {
            throw new ArgumentNullException(name);
        }
    }

    /// <summary>
    ///     Prevents further execution if any of the values are <see langword="null"/>.
    /// </summary>
    /// <param name="valueA">Passed parameter value.</param>
    /// <param name="valueB">Passed parameter value.</param>
    /// <param name="nameA">Name of the parameter.</param>
    /// <param name="nameB">Name of the parameter.</param>
    /// <exception cref="ArgumentNullException">If any value is null.</exception>
    [DebuggerStepThrough]
    public static void ThrowIfNull(
        [NotNull] object? valueA,
        [NotNull] object? valueB,
        [CallerArgumentExpression(nameof(valueA))] string? nameA = null,
        [CallerArgumentExpression(nameof(valueB))] string? nameB = null
    )
    {
        ThrowIfNull(valueA, nameA);
        ThrowIfNull(valueB, nameB);
    }

    /// <inheritdoc cref="ThrowIfNull(object,object,string,string)"/>>
    /// <param name="valueC">Passed parameter value.</param>
    /// <param name="nameC">Name of the parameter.</param>
    [DebuggerStepThrough]
    public static void ThrowIfNull(
        [NotNull] object? valueA,
        [NotNull] object? valueB,
        [NotNull] object? valueC,
        [CallerArgumentExpression(nameof(valueA))] string? nameA = null,
        [CallerArgumentExpression(nameof(valueB))] string? nameB = null,
        [CallerArgumentExpression(nameof(valueC))] string? nameC = null
    )
    {
        ThrowIfNull(valueA, nameA);
        ThrowIfNull(valueB, nameB);
        ThrowIfNull(valueC, nameC);
    }

    /// <inheritdoc cref="ThrowIfNull(object,object,object,string,string,string)"/>>
    /// <param name="valueD">Passed parameter value.</param>
    /// <param name="nameD">Name of the parameter.</param>
    [DebuggerStepThrough]
    public static void ThrowIfNull(
        [NotNull] object? valueA,
        [NotNull] object? valueB,
        [NotNull] object? valueC,
        [NotNull] object? valueD,
        [CallerArgumentExpression(nameof(valueA))] string? nameA = null,
        [CallerArgumentExpression(nameof(valueB))] string? nameB = null,
        [CallerArgumentExpression(nameof(valueC))] string? nameC = null,
        [CallerArgumentExpression(nameof(valueD))] string? nameD = null
    )
    {
        ThrowIfNull(valueA, nameA);
        ThrowIfNull(valueB, nameB);
        ThrowIfNull(valueC, nameC);
        ThrowIfNull(valueD, nameD);
    }

    /// <inheritdoc cref="ThrowIfNull(object,object,object,string,string,string)"/>>
    /// <param name="valueE">Passed parameter value.</param>
    /// <param name="nameE">Name of the parameter.</param>
    [DebuggerStepThrough]
    public static void ThrowIfNull(
        [NotNull] object? valueA,
        [NotNull] object? valueB,
        [NotNull] object? valueC,
        [NotNull] object? valueD,
        [NotNull] object? valueE,
        [CallerArgumentExpression(nameof(valueA))] string? nameA = null,
        [CallerArgumentExpression(nameof(valueB))] string? nameB = null,
        [CallerArgumentExpression(nameof(valueC))] string? nameC = null,
        [CallerArgumentExpression(nameof(valueD))] string? nameD = null,
        [CallerArgumentExpression(nameof(valueE))] string? nameE = null
    )
    {
        ThrowIfNull(valueA, nameA);
        ThrowIfNull(valueB, nameB);
        ThrowIfNull(valueC, nameC);
        ThrowIfNull(valueD, nameD);
        ThrowIfNull(valueE, nameE);
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
