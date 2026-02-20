using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Extensions;

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
    /// <param name="valueA">First passed parameter value.</param>
    /// <param name="valueB">Second passed parameter value.</param>
    /// <param name="nameA">Parameter name for <paramref name="valueA"/>.</param>
    /// <param name="nameB">Parameter name for <paramref name="valueB"/>.</param>
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

    /// <inheritdoc cref="ThrowIfNull(object,object,string,string)"/>
    /// <param name="valueC">Third passed parameter value.</param>
    /// <param name="nameC">Parameter name for <paramref name="valueC"/>.</param>
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

    /// <inheritdoc cref="ThrowIfNull(object,object,object,string,string,string)"/>
    /// <param name="valueD">Fourth passed parameter value.</param>
    /// <param name="nameD">Parameter name for <paramref name="valueD"/>.</param>
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

    /// <inheritdoc cref="ThrowIfNull(object,object,object,object,string,string,string,string)"/>
    /// <param name="valueE">Fifth passed parameter value.</param>
    /// <param name="nameE">Parameter name for <paramref name="valueE"/>.</param>
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

    /// <inheritdoc cref="ThrowIfNull(
    ///     object,object,object,object,object,string,string,string,string,string)"/>
    /// <param name="valueF">Sixth passed parameter value.</param>
    /// <param name="nameF">Parameter name for <paramref name="valueF"/>.</param>
    [DebuggerStepThrough]
    public static void ThrowIfNull(
        [NotNull] object? valueA,
        [NotNull] object? valueB,
        [NotNull] object? valueC,
        [NotNull] object? valueD,
        [NotNull] object? valueE,
        [NotNull] object? valueF,
        [CallerArgumentExpression(nameof(valueA))] string? nameA = null,
        [CallerArgumentExpression(nameof(valueB))] string? nameB = null,
        [CallerArgumentExpression(nameof(valueC))] string? nameC = null,
        [CallerArgumentExpression(nameof(valueD))] string? nameD = null,
        [CallerArgumentExpression(nameof(valueE))] string? nameE = null,
        [CallerArgumentExpression(nameof(valueF))] string? nameF = null
    )
    {
        ThrowIfNull(valueA, nameA);
        ThrowIfNull(valueB, nameB);
        ThrowIfNull(valueC, nameC);
        ThrowIfNull(valueD, nameD);
        ThrowIfNull(valueE, nameE);
        ThrowIfNull(valueF, nameF);
    }

    /// <inheritdoc cref="ThrowIfNull(
    ///     object,object,object,object,object,object,string,string,string,string,string,string)"/>
    /// <param name="valueG">Seventh passed parameter value.</param>
    /// <param name="nameG">Parameter name for <paramref name="valueG"/>.</param>
    [DebuggerStepThrough]
    public static void ThrowIfNull(
        [NotNull] object? valueA,
        [NotNull] object? valueB,
        [NotNull] object? valueC,
        [NotNull] object? valueD,
        [NotNull] object? valueE,
        [NotNull] object? valueF,
        [NotNull] object? valueG,
        [CallerArgumentExpression(nameof(valueA))] string? nameA = null,
        [CallerArgumentExpression(nameof(valueB))] string? nameB = null,
        [CallerArgumentExpression(nameof(valueC))] string? nameC = null,
        [CallerArgumentExpression(nameof(valueD))] string? nameD = null,
        [CallerArgumentExpression(nameof(valueE))] string? nameE = null,
        [CallerArgumentExpression(nameof(valueF))] string? nameF = null,
        [CallerArgumentExpression(nameof(valueG))] string? nameG = null
    )
    {
        ThrowIfNull(valueA, nameA);
        ThrowIfNull(valueB, nameB);
        ThrowIfNull(valueC, nameC);
        ThrowIfNull(valueD, nameD);
        ThrowIfNull(valueE, nameE);
        ThrowIfNull(valueF, nameF);
        ThrowIfNull(valueG, nameG);
    }

    /// <inheritdoc cref="ThrowIfNull(
    ///     object,object,object,object,object,object,object,
    ///     string,string,string,string,string,string,string)"/>
    /// <param name="valueH">Eighth passed parameter value.</param>
    /// <param name="nameH">Parameter name for <paramref name="valueH"/>.</param>
    [DebuggerStepThrough]
    public static void ThrowIfNull(
        [NotNull] object? valueA,
        [NotNull] object? valueB,
        [NotNull] object? valueC,
        [NotNull] object? valueD,
        [NotNull] object? valueE,
        [NotNull] object? valueF,
        [NotNull] object? valueG,
        [NotNull] object? valueH,
        [CallerArgumentExpression(nameof(valueA))] string? nameA = null,
        [CallerArgumentExpression(nameof(valueB))] string? nameB = null,
        [CallerArgumentExpression(nameof(valueC))] string? nameC = null,
        [CallerArgumentExpression(nameof(valueD))] string? nameD = null,
        [CallerArgumentExpression(nameof(valueE))] string? nameE = null,
        [CallerArgumentExpression(nameof(valueF))] string? nameF = null,
        [CallerArgumentExpression(nameof(valueG))] string? nameG = null,
        [CallerArgumentExpression(nameof(valueH))] string? nameH = null
    )
    {
        ThrowIfNull(valueA, nameA);
        ThrowIfNull(valueB, nameB);
        ThrowIfNull(valueC, nameC);
        ThrowIfNull(valueD, nameD);
        ThrowIfNull(valueE, nameE);
        ThrowIfNull(valueF, nameF);
        ThrowIfNull(valueG, nameG);
        ThrowIfNull(valueH, nameH);
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
    /// <exception cref="AsynchronousAccessException">
    ///     If <paramref name="value"/> is an asynchronous <see cref="Type"/>.
    /// </exception>
    [DebuggerStepThrough]
    public static void ThrowIfAsynchronous(object? value, string message)
    {
        if (IsAsynchronous(value))
        {
            throw new AsynchronousAccessException(message);
        }
    }
}

#pragma warning restore RCS1256
