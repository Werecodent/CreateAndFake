using CreateAndFake.Design.Content;

namespace CreateAndFake.Toolbox.ValuerTool;

/// <summary>Provides value equality with the aid of an <see cref="IValuer"/>.</summary>
public interface IValuerEquatable
{
    /// <param name="valuer">Handles comparison behavior for child values.</param>
    /// <inheritdoc cref="IValueEquatable.ValuesEqual"/>
    bool ValuesEqual(object? other, IValuer valuer);

    /// <param name="valuer">Handles hashing behavior for child values.</param>
    /// <inheritdoc cref="IValueEquatable.GetValueHash"/>
    int GetValueHash(IValuer valuer);
}
