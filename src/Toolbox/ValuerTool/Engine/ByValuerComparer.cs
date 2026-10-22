namespace Werecodent.CreateAndFake.ValuerTool.Engine;

/// <summary>...</summary>
/// <typeparam name="T"></typeparam>
/// <param name="valuer"></param>
public sealed class ByValuerComparer<T>(IValuer valuer) : IEqualityComparer<T>
{
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
