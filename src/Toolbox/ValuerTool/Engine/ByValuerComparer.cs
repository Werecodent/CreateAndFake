namespace Werecodent.CreateAndFake.ValuerTool.Engine;

/// <summary>Checks if two objects are equal using the <paramref name="valuer"/>.</summary>
/// <typeparam name="T">Supported type for comparisons.</typeparam>
/// <param name="valuer"><inheritdoc cref="_valuer" path="/summary"/></param>
public sealed class ByValuerComparer<T>(IValuer valuer) : IEqualityComparer<T>
{
    /// <inheritdoc cref="IValuer" path="/summary"/>
    private readonly IValuer _valuer = valuer ?? throw new ArgumentNullException(nameof(valuer));

    /// <inheritdoc/>
    public bool Equals(T? x, T? y)
    {
        return _valuer.Equals(x, y);
    }

    /// <inheritdoc/>
    public int GetHashCode(T? obj)
    {
        return _valuer.GetHashCode(obj);
    }
}
