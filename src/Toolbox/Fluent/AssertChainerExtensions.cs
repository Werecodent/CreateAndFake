using CreateAndFake.Design;

namespace CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class AssertChainerExtensions
{
    /// <inheritdoc cref="AssertChainer{T}.And"/>
    public static async Task<T> AndAsync<T>(this Task<AssertChainer<T>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).And;
    }
}
