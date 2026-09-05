using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class TaskExceptionChainerExtensions
{
    /// <inheritdoc cref="ExceptionChainer{T}.That"/>
    public static async Task<AssertError> That<T>(this Task<ExceptionChainer<T>> origin)
        where T : Exception
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That;
    }

    /// <inheritdoc cref="ExceptionChainer{T}.With"/>
    public static async Task<T> GetCaughtException<T>(this Task<ExceptionChainer<T>> origin)
        where T : Exception
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).GetCaughtException();
    }
}
