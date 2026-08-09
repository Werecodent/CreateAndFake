using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class TaskAssertChainerExtensions
{
    /// <inheritdoc cref="AssertChainer{T}.And"/>
    public static async Task<T> And<T>(this Task<AssertChainer<T>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).And;
    }
}
