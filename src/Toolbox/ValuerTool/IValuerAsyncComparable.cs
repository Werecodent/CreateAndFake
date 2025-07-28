using CreateAndFake.Design.Content;

namespace CreateAndFake.ValuerTool;

/// <summary>Provides value equality with the aid of an <see cref="IValuer"/>.</summary>
public interface IValuerAsyncComparable
{
    /// <param name="valuer">Handles comparison behavior for child values.</param>
    /// <inheritdoc cref="IValueEquatable.ValuesEqual"/>
    IAsyncEnumerable<Difference> CompareAsync(object? other, IValuer valuer);

    /// <param name="valuer">Handles hashing behavior for child values.</param>
    /// <inheritdoc cref="IValueEquatable.GetValueHash"/>
    Task<int> GetValueHashAsync(IValuer valuer);
}
