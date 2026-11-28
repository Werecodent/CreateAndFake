using Werecodent.CreateAndFake.Design.Comparisons;

namespace Werecodent.CreateAndFake.ValuerTool;

/// <summary>Provides value equality with the aid of an <see cref="IValuer"/>.</summary>
/// <remarks>The <see cref="Valuer"/> tool will use the implemented behavior.</remarks>
public interface IValuerEquatable
{
    /// <param name="valuer">Handles comparison behavior for child values.</param>
    /// <inheritdoc cref="IValueEquatable.ValuesEqual"/>
    bool ValuesEqual(object? other, IValuer valuer);

    /// <param name="valuer">Handles hashing behavior for child values.</param>
    /// <inheritdoc cref="IValueEquatable.GetValueHash"/>
    int GetValueHash(IValuer valuer);
}
