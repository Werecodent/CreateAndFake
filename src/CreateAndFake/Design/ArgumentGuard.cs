using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace CreateAndFake.Design;

/// <summary>Handles common argument exception cases.</summary>
internal static class ArgumentGuard
{
    /// <summary>Prevents further execution if the parameter is null.</summary>
    /// <param name="value">Passed parameter value.</param>
    /// <param name="name">Name of the parameter.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is null.</exception>
    [DebuggerStepperBoundary]
    internal static void ThrowIfNull([NotNull] object? value, string name)
    {
        if (value is null)
        {
            throw new ArgumentNullException(name);
        }
    }
}
