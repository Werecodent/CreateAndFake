using Werecodent.CreateAndFake.Design.Comparisons;

namespace Werecodent.CreateAndFake.ValuerTool.Engine;

/// <summary>...</summary>
/// <typeparam name="T"></typeparam>
/// <param name="valuer"></param>
public sealed class ByValuerAsyncComparer<T>(IValuer valuer) : IAsyncEqualityComparer<T>
{
    private readonly IValuer _valuer = valuer ?? throw new ArgumentNullException(nameof(valuer));

    /// <inheritdoc/>
    public Task<bool> EqualsAsync(T? x, T? y, CancellationToken canceler)
    {
        return _valuer.EqualsAsync(x, y, canceler);
    }

    /// <inheritdoc/>
    public Task<int> GetHashCodeAsync(T? obj, CancellationToken canceler)
    {
        return _valuer.GetHashCodeAsync(obj, canceler);
    }
}
