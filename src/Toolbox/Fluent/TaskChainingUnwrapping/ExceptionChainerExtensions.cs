using CreateAndFake.Design;
using CreateAndFake.Fluent.AssertCalls;
using CreateAndFake.Fluent.Chaining;

namespace CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class ExceptionChainerExtensions
{
    /// <inheritdoc cref="ExceptionChainer{T}.That"/>
    public static async Task<AssertError> ThatAsync<T>(this Task<ExceptionChainer<T>> origin)
        where T : Exception
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That;
    }

    /// <inheritdoc cref="ExceptionChainer{T}.That"/>
    public static async Task<T> WithAsync<T>(this Task<ExceptionChainer<T>> origin)
        where T : Exception
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).With;
    }
}
