using CreateAndFake.Design.Comparisons;

namespace CreateAndFake.ValuerTool;

/// <summary>Provides value equality with the aid of an <see cref="IValuer"/>.</summary>
public interface IValuerComparable
{
    /// <param name="valuer">Handles comparison behavior for child values.</param>
    /// <inheritdoc cref="IValueEquatable.ValuesEqual"/>
    IEnumerable<Difference> Compare(object? other, IValuer valuer);

    /// <param name="valuer">Handles hashing behavior for child values.</param>
    /// <inheritdoc cref="IValueEquatable.GetValueHash"/>
    int GetValueHash(IValuer valuer);
}
